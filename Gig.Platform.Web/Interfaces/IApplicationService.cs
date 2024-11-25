namespace Gig.Platform.Web.Interfaces
{
    public interface IApplicationService
    {
        Task<ApplicationResponseDto> ApplyAsync(ApplicationRequestDto dto);
        Task<IEnumerable<ApplicationResponseDto>> GetAllByJobAsync(Guid jobId);
    }
}
