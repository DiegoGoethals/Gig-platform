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
    public class ApplicationRepository : BaseRepository<Application>, IApplicationRepository
    {
        public ApplicationRepository(ApplicationDbContext applicationDbContext, ILogger<IBaseRepository<Application>> logger)
            : base(applicationDbContext, logger)
        {
        }

        public async Task<IEnumerable<Application>> GetAllByJobAsync(Guid jobId)
        {
            return await _table
                .Where(t => t.JobId == jobId)
                .Include(t => t.Status)
                .Include(t => t.Candidate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Application>> GetAllByCandidateAsync(Guid candidateId)
        {
            return await _table
                .Where(t => t.CandidateId == candidateId)
                .Include(t => t.Status)
                .Include(t => t.Job)
                .ToListAsync();
        }

        public async override Task<Application> GetByIdAsync(Guid id)
        {
            return await _table
                .Include(t => t.Status)
                .Include(t => t.Candidate)
                .Include(t => t.Job)
                .FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}
