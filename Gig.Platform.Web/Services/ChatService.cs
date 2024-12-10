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

        public async Task<MessageResponseDto> SendMessageAsync(MessageRequestDto messageRequestDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/messages", messageRequestDto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<MessageResponseDto>();
        }

        public async Task<IEnumerable<MessageResponseDto>> GetConversationAsync(Guid id1, Guid id2)
        {
            var response = await _httpClient.GetAsync($"api/messages/conversation/{id1}/{id2}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<MessageResponseDto>>();
        }
    }
}
