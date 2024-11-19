using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gig.Platform.Core.Interfaces.Repositories
{
    public interface IReviewRepository : IBaseRepository<Review>
    {
        Task<IEnumerable<Review>> GetByRevieweeIdAsync(Guid revieweeId);
        Task<IEnumerable<Review>> GetByReviewerIdAsync(Guid reviewerId);
    }
}
