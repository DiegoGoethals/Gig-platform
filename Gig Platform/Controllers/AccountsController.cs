using Gig.Platform.Core.Interfaces.Services;
using Gig.Platform.Core.Services;
using MailKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Gig_Platform.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IReviewService _reviewService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly Gig.Platform.Core.Interfaces.Services.IMailService _mailService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LinkGenerator _linkGenerator;
        private readonly SupabaseStorageService _supabaseStorageService;

        public AccountsController(IAccountService accountService, IReviewService reviewService, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, Gig.Platform.Core.Interfaces.Services.IMailService mailService, IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator, SupabaseStorageService supabaseStorageService)
        {
            _accountService = accountService;
            _reviewService = reviewService;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _mailService = mailService;
            _httpContextAccessor = httpContextAccessor;
            _linkGenerator = linkGenerator;
            _supabaseStorageService = supabaseStorageService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(AccountRequestDto accountRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _signInManager.PasswordSignInAsync(accountRequestDto.UserName, accountRequestDto.Password, false, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Wrong credentials!");
                return BadRequest(ModelState);
            }
            var user = await _userManager.FindByNameAsync(accountRequestDto.UserName);

            var claims = await _userManager.GetClaimsAsync(user);

            var issuer = _configuration.GetValue<string>("JWTConfiguration:Issuer");
            var audience = _configuration.GetValue<string>("JWTConfiguration:Audience");
            var expiration = DateTime.Now.AddDays(_configuration.GetValue<int>("JWTConfiguration:ExpirationInDays"));
            var key = Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JWTConfiguration:SecretKey"));
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            var signinCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                notBefore: DateTime.Now,
                expires: expiration,
                claims: claims,
                signingCredentials: signinCredentials
                );

            var serializedToken = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new AccountResponseDto { Id = user.Id, UserName = user.UserName, Token = serializedToken });
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegistrationRequestDto registrationRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _userManager.FindByNameAsync(registrationRequestDto.UserName) == null)
            {
                var user = new ApplicationUser
                {
                    UserName = registrationRequestDto.UserName,
                    Email = registrationRequestDto.Email,
                    Firstname = registrationRequestDto.Firstname,
                    Lastname = registrationRequestDto.Lastname,
                    Skills = new List<Skill>(),
                    Created = DateTime.Now,
                    Updated = DateTime.Now,
                    Birthday = registrationRequestDto.Birthday,
                    ProfilePicture = await _supabaseStorageService.UploadFileAsync($"{registrationRequestDto.UserName}{registrationRequestDto.FileExtension}", registrationRequestDto.FileData),
                    Bio = registrationRequestDto.Bio
                };

                var registrationResult = await _accountService.Register(user, registrationRequestDto.Skills);

                if (registrationResult.IsSucces)
                {
                    var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);
                    if (result.Succeeded)
                    {
                        _ = await _userManager.AddToRoleAsync(user, registrationRequestDto.Role);

                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Role, registrationRequestDto.Role),
                            new Claim(ClaimTypes.DateOfBirth, user.Birthday.ToString()),
                            new Claim(ClaimTypes.Name, user.UserName),
                            new Claim(ClaimTypes.Email, user.Email),
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                        };

                        result = await _userManager.AddClaimsAsync(user, claims);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                            return BadRequest(ModelState);
                        }
                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var confirmationLink = _linkGenerator.GetUriByAction
                        (
                            action: "ValidateEmail",
                            controller: "Accounts",
                            scheme: _httpContextAccessor.HttpContext.Request.Scheme,
                            host: _httpContextAccessor.HttpContext.Request.Host,
                            values: new { id = user.Id, token }
                        );

                        await _mailService.SendValidationEmail(user, confirmationLink, _configuration["Email:Account"], _configuration["Email:ApiKey"]);

                        return CreatedAtAction(nameof(Register), new { id = registrationResult.Value.Id }, new RegistrationResponseDto
                        {
                            Id = registrationResult.Value.Id,
                            UserName = registrationResult.Value.UserName
                        });
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return BadRequest(ModelState);
                    }
                }
            }
            return BadRequest();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var result = await _accountService.GetByIdAsync(id);
            if (result.IsSucces)
            {
                var reviewsResult = await _reviewService.GetAllByRevieweeIdAsync(id);
                var reviews = new List<ReviewResponseDto>();
                if (reviewsResult.IsSucces)
                {
                    reviews = reviewsResult.Value.Select(r => new ReviewResponseDto
                    {
                        ReviewerId = r.ReviewerId,
                        ReviewerName = r.Reviewer.UserName,
                        Id = r.Id,
                        RevieweeId = r.RevieweeId,
                        Rating = r.Rating,
                        Comment = r.Comment
                    }).ToList();
                }
                return Ok(new UserDetailsResponseDto
                {
                    UserName = result.Value.UserName,
                    Id = result.Value.Id,
                    Created = result.Value.Created,
                    Skills = result.Value.Skills.Any() ? result.Value.Skills.Select(s => s.Name).ToList() : new List<string>(),
                    Reviews = reviews,
                    ProfilePictureUrl = result.Value.ProfilePicture
                });
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
            return BadRequest(ModelState.Values);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ValidateEmail(Guid id, string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                ModelState.AddModelError("", "Token is null.");
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                ModelState.AddModelError("", "User not found.");
                return BadRequest(ModelState);
            }

            token = token.Replace(" ", "+");
            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                return Redirect("https://localhost:7126/home/confirmemail");
            }
            else
            {
                ModelState.AddModelError("", "Invalid token.");
                return BadRequest(ModelState);
            }
        }
    }
}
