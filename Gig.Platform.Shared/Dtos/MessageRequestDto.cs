namespace Gig.Platform.Shared.Dtos
{
    public class MessageRequestDto
    {
        public string Content { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
    }
}
