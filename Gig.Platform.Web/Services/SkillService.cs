using Gig.Platform.Web.Interfaces;

namespace Gig.Platform.Web.Services
{
    public class SkillService : ISkillService
    {
        private readonly HttpClient _httpClient;

        public SkillService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<SkillResponseDto>> GetSkillsAsync()
        {
            var response = await _httpClient.GetAsync("api/skills");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<SkillResponseDto>>();
        }
    }
}
