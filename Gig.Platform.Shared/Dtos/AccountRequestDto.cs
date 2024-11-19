using System.ComponentModel.DataAnnotations;

namespace Gig.Platform.Shared.Dtos
{
    public class AccountRequestDto
    {
        [Required(ErrorMessage = "Username missing!")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password missing!")]
        public string Password { get; set; }
    }
}
