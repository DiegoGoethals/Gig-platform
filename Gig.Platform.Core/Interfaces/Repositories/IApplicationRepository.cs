using Gig.Platform.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gig.Platform.Core.Interfaces.Repositories
{
    public interface IApplicationRepository : IBaseRepository<Application>
    {
        Task<IEnumerable<Application>> GetAllByJobAsync(Guid jobId);
        Task<IEnumerable<Application>> GetAllByCandidateAsync(Guid candidateId);
    }
}
