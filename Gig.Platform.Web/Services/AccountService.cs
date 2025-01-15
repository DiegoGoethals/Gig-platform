using Gig.Platform.Web.Interfaces;

namespace Gig.Platform.Web.Services
{
    public class AccountService(HttpClient httpClient) : IAccountService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<RegistrationResponseDto?> RegisterAsync(RegistrationRequestDto dto)
        {
            var response = await _httpClient.PostAsync("api/auth/register", JsonContent.Create(dto));

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<RegistrationResponseDto>();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var errorContent = await response.Content.ReadFromJsonAsync<Dictionary<string, string[]>>();
                if (errorContent != null)
                {
                    var errors = string.Join("; ", errorContent.Values.SelectMany(v => v));
                    throw new Exception(errors);
                }

                throw new Exception("An unknown error occurred.");
            }

            throw new Exception($"Unexpected error: {response.StatusCode}");
        }

        public async Task<AccountResponseDto?> Login(AccountRequestDto dto)
        {
            var response = await _httpClient.PostAsync("api/auth/login", JsonContent.Create(dto));

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AccountResponseDto>();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var errorContent = await response.Content.ReadFromJsonAsync<Dictionary<string, string[]>>();
                if (errorContent != null)
                {
                    var errors = string.Join("; ", errorContent.Values.SelectMany(v => v));
                    throw new Exception(errors);
                }

                throw new Exception("An unknown error occurred.");
            }

            throw new Exception($"Unexpected error: {response.StatusCode}");
        }

        public async Task<UserDetailsResponseDto> GetUserDetailsAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"api/auth/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<UserDetailsResponseDto>();
        }

        public async Task<UserDetailsResponseDto> UpdateUserDetailsAsync(Guid id, RegistrationRequestDto dto)
        {
            var response = await _httpClient.PutAsync($"api/auth/{id}", JsonContent.Create(dto));
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<UserDetailsResponseDto>();
        }
    }
}
