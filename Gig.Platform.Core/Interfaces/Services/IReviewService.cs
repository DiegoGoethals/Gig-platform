using Gig.Platform.Core.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gig.Platform.Core.Interfaces.Services
{
    public interface IReviewService
    {
        Task<ResultModel<Review>> CreateAsync(int rating, string comment, Guid reviewerId, Guid revieweeId);
        Task<ResultModel<Review>> UpdateAsync(Guid id, int rating, string comment);
        Task<ResultModel<Review>> DeleteAsync(Guid id);
        Task<ResultModel<IEnumerable<Review>>> GetAllByRevieweeIdAsync(Guid revieweeId);
        Task<ResultModel<IEnumerable<Review>>> GetAllByReviewerIdAsync(Guid reviewerId);
        Task<ResultModel<Review>> GetByIdAsync(Guid id);
    }
}
