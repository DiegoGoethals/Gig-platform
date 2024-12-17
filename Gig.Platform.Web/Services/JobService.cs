using Gig.Platform.Shared.Dtos;
using Gig.Platform.Web.Interfaces;
using Gig.Platform.Web.Services.Special_services;

namespace Gig.Platform.Web.Services
{
    public class JobService : IJobService
    {
        private readonly HttpClient _httpClient;
        private readonly JwtService _jwtService;

        public JobService(HttpClient httpClient, JwtService jwtService)
        {
            _httpClient = httpClient;
            _jwtService = jwtService;
        }

        public async Task<IEnumerable<JobResponseDto>> GetAllJobsForEmployeesAsync()
        {
            var response = await _httpClient.GetAsync("api/jobs");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<JobResponseDto>>();
        }

        public async Task<IEnumerable<JobResponseDto>> GetAllJobsByEmployerAsync(Guid employerId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/jobs/employer/{employerId}");

            var token = await _jwtService.GetTokenAsync();

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<JobResponseDto>>();
        }

        public async Task<JobResponseDto> AddJobAsync(JobRequestDto jobRequestDto)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/jobs")
            {
                Content = JsonContent.Create(jobRequestDto)
            };

            var token = await _jwtService.GetTokenAsync();

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<JobResponseDto>();
        }

        public async Task<JobResponseDto> GetJobByIdAsync(Guid jobId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/jobs/{jobId}");

            var token = await _jwtService.GetTokenAsync();

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<JobResponseDto>();
        }

        public async Task<JobResponseDto> UpdateJobAsync(Guid jobId, JobRequestDto jobRequestDto)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, $"api/jobs/{jobId}")
            {
                Content = JsonContent.Create(jobRequestDto)
            };
            var token = await _jwtService.GetTokenAsync();
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<JobResponseDto>();
        }
    }
}
