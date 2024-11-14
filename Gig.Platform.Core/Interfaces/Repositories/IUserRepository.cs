using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gig.Platform.Core.Interfaces.Repositories
{
    public interface IUserRepository<ApplicationUser>
    {
        Task<IEnumerable<ApplicationUser>> GetAllAsync();
        Task<ApplicationUser> GetByIdAsync(Guid id);
        Task<ApplicationUser> GetByUserNameAsync(string userName);
        IQueryable<ApplicationUser> GetAll();
        Task<bool> UpdateAsync(ApplicationUser toUpdate);
        Task<bool> CheckIfExistsAsync(Guid id);
    }
}
