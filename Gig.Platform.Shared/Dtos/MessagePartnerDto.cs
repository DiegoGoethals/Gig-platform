using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gig.Platform.Shared.Dtos
{
    public class MessagePartnerDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string LastMessage { get; set; }
        public DateTime LastMessageDate { get; set; }
        public Guid LastMessageSenderId { get; set; }
    }
}
