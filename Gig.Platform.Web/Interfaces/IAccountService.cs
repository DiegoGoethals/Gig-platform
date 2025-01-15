namespace Gig.Platform.Web.Interfaces
{
    public interface IAccountService
    {
        Task<RegistrationResponseDto?> RegisterAsync(RegistrationRequestDto dto);
        Task<AccountResponseDto?> Login(AccountRequestDto dto);
        Task<UserDetailsResponseDto> GetUserDetailsAsync(Guid id);
        Task<UserDetailsResponseDto> UpdateUserDetailsAsync(Guid id, RegistrationRequestDto dto);
    }
}
