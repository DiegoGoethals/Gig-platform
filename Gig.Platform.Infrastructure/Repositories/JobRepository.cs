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

        public async Task<IEnumerable<Job>> GetJobsByDistance(double latitude, double longitude, double distance)
        {
            var allJobs = await _table
                .Include(j => j.Applications)
                    .ThenInclude(a => a.Candidate)
                .Include(j => j.Applications)
                    .ThenInclude(a => a.Status)
                .Include(j => j.Skills)
                .ToListAsync();

            var filteredJobs = allJobs.Where(j =>
            {
                double jobLatitude = j.Latitude;
                double jobLongitude = j.Longitude;

                double jobDistance = CalculateDistance(latitude, longitude, jobLatitude, jobLongitude);

                return jobDistance <= distance;
            });

            return filteredJobs;
        }

        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6371; // Radius of the Earth in km
            var phi1 = lat1 * Math.PI / 180;
            var phi2 = lat2 * Math.PI / 180;
            var deltaPhi = (lat2 - lat1) * Math.PI / 180;
            var deltaLambda = (lon2 - lon1) * Math.PI / 180;

            // Haversine formula
            var a = Math.Sin(deltaPhi / 2) * Math.Sin(deltaPhi / 2) +
                    Math.Cos(phi1) * Math.Cos(phi2) *
                    Math.Sin(deltaLambda / 2) * Math.Sin(deltaLambda / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            // Distance in kilometers
            var distance = R * c;

            return distance;
        }
    }
}
