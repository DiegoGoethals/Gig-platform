using Gig.Platform.Core.Entities;
using Gig.Platform.Core.Interfaces.Repositories;
using Gig.Platform.Core.Interfaces.Services;
using Gig.Platform.Core.Services;
using Gig.Platform.Infrastructure.Data;
using Gig.Platform.Infrastructure.Repositories;
using MailKit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add database service
builder.Services.AddDbContext<ApplicationDbContext>
    (options => options
    .UseSqlServer(builder.Configuration.GetConnectionString("DefaultDb")));

// Add identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
{
    //configure options for testing purposes
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
}).AddEntityFrameworkStores<ApplicationDbContext>()
     .AddSignInManager<SignInManager<ApplicationUser>>()
    .AddDefaultTokenProviders();

// Add JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidAudience = builder.Configuration["JWTConfiguration:Audience"],
        ValidIssuer = builder.Configuration["JWTConfiguration:Issuer"],
        IssuerSigningKey =
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTConfiguration:SecretKey"]))
    };
});

builder.Services.AddCors();

// Add repositories to the container.
builder.Services.AddScoped<IJobRepository, JobRepository>();
builder.Services.AddScoped<ISkillRepository, SkillRepository>();
builder.Services.AddScoped<IUserRepository<ApplicationUser>, UserRepository>();
builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();
builder.Services.AddScoped<IApplicationStatusRepository, ApplicationStatusRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();

// Add services to the container.
builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddScoped<ISkillService, SkillService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IApplicationService, ApplicationService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddTransient<Gig.Platform.Core.Interfaces.Services.IMailService, Gig.Platform.Core.Services.MailService>();
builder.Services.AddHttpClient<GeocodingService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

    // Add JWT authentication
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter JWT Bearer token **only**",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    };
    c.AddSecurityDefinition("Bearer", securityScheme);

    var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    };
    c.AddSecurityRequirement(securityRequirement);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(options =>
{
    options.AllowAnyOrigin();
    options.AllowAnyHeader();
    options.AllowAnyMethod();
});

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        // Set the status code to 403 Forbidden
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        // Write a meaningful error message to the response
        await context.Response.WriteAsync("Access Forbidden");
    });
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
