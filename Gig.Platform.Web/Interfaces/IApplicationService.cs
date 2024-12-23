namespace Gig.Platform.Web.Interfaces
{
    public interface IApplicationService
    {
        Task<ApplicationResponseDto> ApplyAsync(ApplicationRequestDto dto);
        Task<IEnumerable<ApplicationResponseDto>> GetAllByJobAsync(Guid jobId);
        Task<IEnumerable<ApplicationStatusDto>> GetAllStatusesAsync();
        Task HandleApplicationAsync(Guid applicationId, ApplicationStatusDto status);
        Task<IEnumerable<ApplicationResponseDto>> GetAllByCandidateAsync(Guid candidateId);
    }
}
