namespace Gig.Platform.Shared.Dtos
{
    public class ReviewRequestDto
    {
        public int Rating { get; set; }
        public string Comment { get; set; }
        public Guid ReviewerId { get; set; }
        public Guid RevieweeId { get; set; }
    }
}
