namespace Gig.Platform.Shared.Dtos
{
    public class ApplicationResponseDto
    {
        public Guid Id { get; set; }
        public Guid JobId { get; set; }
        public Guid CandidateId { get; set; }
        public string CandidateName { get; set; }
        public string ApplicationStatus { get; set; }
    }
}
