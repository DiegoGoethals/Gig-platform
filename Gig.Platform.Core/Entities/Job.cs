using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gig.Platform.Core.Entities
{
    public class Job : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Salary { get; set; }
        public Guid EmployerId { get; set; }
        public ApplicationUser Employer { get; set; }
        public ICollection<Application> Applications { get; set; }
        public ICollection<Skill> Skills { get; set; }
    }
}
