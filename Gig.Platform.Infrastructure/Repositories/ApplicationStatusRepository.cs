using Gig.Platform.Core.Entities;
using Gig.Platform.Core.Interfaces.Repositories;
using Gig.Platform.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gig.Platform.Infrastructure.Repositories
{
    public class ApplicationStatusRepository : BaseRepository<ApplicationStatus>, IApplicationStatusRepository
    {
        public ApplicationStatusRepository(ApplicationDbContext applicationDbContext, ILogger<IBaseRepository<ApplicationStatus>> logger)
            : base(applicationDbContext, logger)
        {
        }
    }
}
