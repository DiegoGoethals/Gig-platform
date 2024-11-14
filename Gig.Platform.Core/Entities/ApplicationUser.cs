using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gig.Platform.Core.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Fullname => $"{Firstname} {Lastname}";
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public DateTime Birthday { get; set; }
        public ICollection<Application> Applications { get; set; }
        public ICollection<Job> Jobs { get; set; }
        public ICollection<Skill> Skills { get; set; }
        public ICollection<Message> SendMessages { get; set; }
        public ICollection<Message> ReceivedMessages { get; set; }
        public ICollection<Review> MadeReviews { get; set; }
        public ICollection<Review> ReceivedReviews { get; set; }
    }
}
