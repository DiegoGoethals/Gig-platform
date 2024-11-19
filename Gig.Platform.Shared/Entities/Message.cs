using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gig.Platform.Shared.Entities
{
    public class Message : BaseEntity
    {
        public string Content { get; set; }
        public Guid SenderId { get; set; }
        public ApplicationUser Sender { get; set; }
        public Guid ReceiverId { get; set; }
        public ApplicationUser Receiver { get; set; }
    }
}
