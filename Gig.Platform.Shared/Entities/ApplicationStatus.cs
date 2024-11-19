using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gig.Platform.Shared.Entities
{
    public class ApplicationStatus : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Application> Applications { get; set; }

    }
}
