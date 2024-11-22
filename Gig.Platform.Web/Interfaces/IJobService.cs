namespace Gig.Platform.Web.Interfaces
{
    public interface IJobService
    {
        Task<IEnumerable<JobResponseDto>> GetAllJobsForEmployeesAsync();
    }
}
