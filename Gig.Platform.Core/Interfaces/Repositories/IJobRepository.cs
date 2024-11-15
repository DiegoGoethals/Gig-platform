using Gig.Platform.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gig.Platform.Core.Interfaces.Repositories
{
    public interface IJobRepository : IBaseRepository<Job>
    {
        public Task<IEnumerable<Job>> GetJobsByDistance(double latitude, double longitude, double distance);
    }
}
