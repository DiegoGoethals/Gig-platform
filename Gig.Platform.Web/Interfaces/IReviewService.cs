namespace Gig.Platform.Web.Interfaces
{
    public interface IReviewService
    {
        Task<ReviewResponseDto> CreateReviewAsync(ReviewRequestDto dto);
    }
}
