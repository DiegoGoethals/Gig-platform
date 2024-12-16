using Gig.Platform.Web.Interfaces;

namespace Gig.Platform.Web.Services
{
    public class ReviewService(HttpClient httpClient) : IReviewService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<ReviewResponseDto> CreateReviewAsync(ReviewRequestDto dto)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/reviews")
            {
                Content = JsonContent.Create(dto)
            };
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ReviewResponseDto>();
        }

        public async Task<ReviewResponseDto> UpdateReviewAsync(Guid id, ReviewRequestDto dto)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, $"api/reviews/{id}")
            {
                Content = JsonContent.Create(dto)
            };
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ReviewResponseDto>();
        }

        public async Task DeleteReviewAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/reviews/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}