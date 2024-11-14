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
    public class SkillRepository : BaseRepository<Skill>, ISkillRepository
    {
        public SkillRepository(ApplicationDbContext applicationDbContext, ILogger<IBaseRepository<Skill>> logger)
            : base(applicationDbContext, logger)
        {
        }

        public async Task<Skill> GetByName(string name)
        {
            return await _table
                .FirstOrDefaultAsync(t => t.Name == name);
        }
    }
}
