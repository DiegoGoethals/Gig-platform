namespace Gig.Platform.Shared.Dtos
{
    public class MessageResponseDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public DateTime Created { get; set; }
    }
}
