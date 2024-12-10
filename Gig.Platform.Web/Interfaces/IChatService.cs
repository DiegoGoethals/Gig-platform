namespace Gig.Platform.Web.Interfaces
{
    public interface IChatService
    {
        Task<IEnumerable<AccountResponseDto>> GetChatPartnersAsync(Guid id);
        Task<MessageResponseDto> SendMessageAsync(MessageRequestDto messageRequestDto);
        Task<IEnumerable<MessageResponseDto>> GetConversationAsync(Guid id1, Guid id2);
    }
}
