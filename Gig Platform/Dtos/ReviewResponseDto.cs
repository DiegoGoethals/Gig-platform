namespace Gig_Platform.Dtos
{
    public class ReviewResponseDto
    {
        public Guid Id { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public string ReviewerName { get; set; }
        public Guid ReviewerId { get; set; }
        public Guid RevieweeId { get; set; }
    }
}
