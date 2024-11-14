using Gig.Platform.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gig.Platform.Core.Interfaces.Repositories
{
    public interface IMessageRepository : IBaseRepository<Message>
    {
        Task<IEnumerable<Message>> GetConversationAsync(Guid id1, Guid id2);
        Task<IEnumerable<ApplicationUser>> GetAllConversationPartnersAsync(Guid userId);
    }
}
