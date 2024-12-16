namespace Gig.Platform.Web.Interfaces
{
    public interface IReviewService
    {
        Task<ReviewResponseDto> CreateReviewAsync(ReviewRequestDto dto);
        Task<ReviewResponseDto> UpdateReviewAsync(Guid id, ReviewRequestDto dto);
    }
}
