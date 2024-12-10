using Gig.Platform.Web.Interfaces;

namespace Gig.Platform.Web.Services
{
    public class ChatService(HttpClient httpClient) : IChatService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<IEnumerable<AccountResponseDto>> GetChatPartnersAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"api/messages/partners/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<AccountResponseDto>>();
        }
    }
}
