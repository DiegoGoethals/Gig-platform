namespace Gig.Platform.Web.Interfaces
{
    public interface IJobService
    {
        Task<IEnumerable<JobResponseDto>> GetAllJobsForEmployeesAsync();
        Task<IEnumerable<JobResponseDto>> GetAllJobsByEmployerAsync(Guid employerId);
        Task<JobResponseDto> AddJobAsync(JobRequestDto jobRequestDto);
        Task<JobResponseDto> GetJobByIdAsync(Guid jobId);
        Task<JobResponseDto> UpdateJobAsync(Guid jobId, JobRequestDto jobRequestDto);
    }
}
