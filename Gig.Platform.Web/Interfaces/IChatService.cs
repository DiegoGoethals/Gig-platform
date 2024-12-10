namespace Gig.Platform.Web.Interfaces
{
    public interface IChatService
    {
        Task<IEnumerable<AccountResponseDto>> GetChatPartnersAsync(Guid id);
    }
}
