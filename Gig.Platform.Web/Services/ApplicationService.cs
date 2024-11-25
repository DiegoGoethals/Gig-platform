using Gig.Platform.Shared.Dtos;
using Gig.Platform.Web.Interfaces;
using Gig.Platform.Web.Services.Special_services;

namespace Gig.Platform.Web.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly HttpClient _httpClient;
        private readonly JwtService _jwtService;

        public ApplicationService(HttpClient httpClient, JwtService jwtService)
        {
            _httpClient = httpClient;
            _jwtService = jwtService;
        }

        public async Task<ApplicationResponseDto> ApplyAsync(ApplicationRequestDto dto)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/applications")
            {
                Content = JsonContent.Create(dto)
            };

            var token = await _jwtService.GetTokenAsync();

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ApplicationResponseDto>();
        }

        public async Task<IEnumerable<ApplicationResponseDto>> GetAllByJobAsync(Guid jobId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/applications/job/{jobId}");
            
            var token = await _jwtService.GetTokenAsync();

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<ApplicationResponseDto>>();
        }
    }
}
