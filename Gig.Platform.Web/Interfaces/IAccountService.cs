namespace Gig.Platform.Web.Interfaces
{
    public interface IAccountService
    {
        Task<RegistrationResponseDto> RegisterAsync(RegistrationRequestDto dto);
        Task<AccountResponseDto> Login(AccountRequestDto dto);
    }
}
