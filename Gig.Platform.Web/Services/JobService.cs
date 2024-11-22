using Gig.Platform.Web.Interfaces;

namespace Gig.Platform.Web.Services
{
    public class JobService : IJobService
    {
        private readonly HttpClient _httpClient;

        public JobService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<JobResponseDto>> GetAllJobsForEmployeesAsync()
        {
            var response = await _httpClient.GetAsync("api/jobs");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<JobResponseDto>>();
        }
}
