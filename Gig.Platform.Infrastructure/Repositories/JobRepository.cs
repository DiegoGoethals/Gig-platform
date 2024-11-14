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
    public class JobRepository : BaseRepository<Job>, IJobRepository
    {
        public JobRepository(ApplicationDbContext applicationDbContext, ILogger<IBaseRepository<Job>> logger)
            : base(applicationDbContext, logger)
        {
        }

        public async override Task<IEnumerable<Job>> GetAllAsync()
        {
            return await _table.Include(j => j.Applications)
                .ThenInclude(a => a.Candidate)
                .Include(j => j.Applications)
                .ThenInclude(a => a.Status)
                .Include(j => j.Skills)
                .ToListAsync();
        }
    }
}
