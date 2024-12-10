using Gig.Platform.Web.Interfaces;

namespace Gig.Platform.Web.Services
{
    public class AccountService(HttpClient httpClient) : IAccountService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<RegistrationResponseDto> RegisterAsync(RegistrationRequestDto dto)
        {
            var response = await _httpClient.PostAsync("api/auth/register", JsonContent.Create(dto));
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<RegistrationResponseDto>();
        }

        public async Task<AccountResponseDto> Login(AccountRequestDto dto)
        {
            var response = await _httpClient.PostAsync("api/auth/login", JsonContent.Create(dto));
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<AccountResponseDto>();
        }

        public async Task<UserDetailsResponseDto> GetUserDetailsAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"api/auth/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<UserDetailsResponseDto>();
        }
    }
}
