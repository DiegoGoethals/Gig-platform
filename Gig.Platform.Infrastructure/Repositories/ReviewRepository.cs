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
    public class ReviewRepository : BaseRepository<Review>, IReviewRepository
    {
        public ReviewRepository(ApplicationDbContext applicationDbContext, ILogger<IBaseRepository<Review>> logger)
            : base(applicationDbContext, logger)
        {
        }

        public async Task<IEnumerable<Review>> GetByRevieweeIdAsync(Guid revieweeId)
        {
            return await _table.Where(x => x.RevieweeId == revieweeId)
                .Include(r => r.Reviewer)
                .Include(r => r.Reviewee)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetByReviewerIdAsync(Guid reviewerId)
        {
            return await _table.Where(x => x.ReviewerId == reviewerId).ToListAsync();
        }
    }
}
