using Gig.Platform.Core.Services.Models;
using Gig.Platform.Shared.Dtos;

namespace Gig.Platform.Core.Interfaces.Services
{
    public interface IAccountService
    {
        Task<ResultModel<IEnumerable<ApplicationUser>>> GetAllAsync();
        Task<ResultModel<ApplicationUser>> GetByUserNameAsync(string userName);
        Task<ResultModel<ApplicationUser>> Register(ApplicationUser user, IEnumerable<string> skills);
        Task<ResultModel<ApplicationUser>> GetByIdAsync(Guid userId);
        Task<ResultModel<ApplicationUser>> UpdateAsync(Guid userId, IEnumerable<string> skills, string profilePicture, string bio);
    }
}
