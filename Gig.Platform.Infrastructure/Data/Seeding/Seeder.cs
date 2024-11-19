using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gig.Platform.Infrastructure.Data.Seeding
{
    public class Seeder
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            var statuses = new List<ApplicationStatus>()
            {
                new ApplicationStatus { Id = Guid.NewGuid(), Name = "Pending" },
                new ApplicationStatus { Id = Guid.NewGuid(), Name = "Approved" },
                new ApplicationStatus { Id = Guid.NewGuid(), Name = "Rejected" }
            };
            modelBuilder.Entity<ApplicationStatus>().HasData(statuses);

            var employerRole = new IdentityRole<Guid> { Id = Guid.NewGuid(), Name = "Employer", NormalizedName = "EMPLOYER" };
            var employeeRole = new IdentityRole<Guid> { Id = Guid.NewGuid(), Name = "Employee", NormalizedName = "EMPLOYEE" };
            modelBuilder.Entity<IdentityRole<Guid>>().HasData(employerRole, employeeRole);

            var skills = new List<Skill>()
            {
                new Skill { Id = Guid.NewGuid(), Name = "Tech Assistance" },
                new Skill { Id = Guid.NewGuid(), Name = "Home Services" },
                new Skill { Id = Guid.NewGuid(), Name = "Tutoring" },
                new Skill { Id = Guid.NewGuid(), Name = "Transportation & Delivery" },
                new Skill { Id = Guid.NewGuid(), Name = "Personal Care" },
                new Skill { Id = Guid.NewGuid(), Name = "Event Assistance" },
                new Skill { Id = Guid.NewGuid(), Name = "Outdoor Services" },
                new Skill { Id = Guid.NewGuid(), Name = "Miscellaneous" }
            };

            modelBuilder.Entity<Skill>().HasData(skills);
        }
    }
}
