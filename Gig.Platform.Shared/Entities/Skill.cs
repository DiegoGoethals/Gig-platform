using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gig.Platform.Shared.Entities
{
    public class Skill : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Job> Jobs { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }
    }
}
