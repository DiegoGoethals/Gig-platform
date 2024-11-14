using Gig.Platform.Core.Entities;
using Gig.Platform.Core.Interfaces.Repositories;
using Gig.Platform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gig.Platform.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository<ApplicationUser>
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ILogger<UserRepository> _logger;
        protected readonly DbSet<ApplicationUser> _table;

        public UserRepository(ApplicationDbContext applicationDbContext, ILogger<UserRepository> logger)
        {
            _applicationDbContext = applicationDbContext;
            _table = _applicationDbContext.Set<ApplicationUser>();
            _logger = logger;
        }

        public IQueryable<ApplicationUser> GetAll()
        {
            return _table.AsQueryable();
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
        {
            return await _table
                .ToListAsync();
        }

        public async Task<ApplicationUser> GetByIdAsync(Guid id)
        {
            return await _table
                .Include(u => u.Skills)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<ApplicationUser> GetByUserNameAsync(string userName)
        {
            return await _table
                .FirstOrDefaultAsync(t => t.UserName == userName);
        }

        public async Task<bool> UpdateAsync(ApplicationUser toUpdate)
        {
            _table.Update(toUpdate);
            return await SaveChangesAsync();
        }
        private async Task<bool> SaveChangesAsync()
        {
            try
            {
                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException dbUpdateException)
            {
                _logger.LogError(dbUpdateException.Message);
                return false;
            }
        }

        public async Task<bool> CheckIfExistsAsync(Guid id)
        {
            return await _table.AnyAsync(t => t.Id == id);
        }
    }
}
