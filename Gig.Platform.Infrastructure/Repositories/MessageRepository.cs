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
    public class MessageRepository : BaseRepository<Message>, IMessageRepository
    {
        public MessageRepository(ApplicationDbContext applicationDbContext, ILogger<IBaseRepository<Message>> logger)
            : base(applicationDbContext, logger)
        {
        }

        public async Task<IEnumerable<Message>> GetConversationAsync(Guid id1, Guid id2)
        {
            return await _table.Where(x => (x.SenderId == id1 && x.ReceiverId == id2) ||
            (x.SenderId == id2 && x.ReceiverId == id1))
            .OrderBy(x => x.Created)
            .Include(m => m.Sender)
            .Include(m => m.Receiver)
            .ToListAsync();
        }

        public async Task<IEnumerable<(ApplicationUser Partner, Message LastMessage)>> GetAllConversationPartnersAsync(Guid userId)
        {
            var messages = await _table
                .Where(x => x.SenderId == userId || x.ReceiverId == userId)
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .OrderByDescending(x => x.Created)
                .ToListAsync();

            var conversationPartnersWithLastMessages = messages
                .Where(m => m.SenderId != userId || m.ReceiverId != userId)
                .GroupBy(m => m.SenderId == userId ? m.Receiver : m.Sender)
                .Select(g => (
                    Partner: g.Key,
                    LastMessage: g.First()
                ))
                .ToList();

            return conversationPartnersWithLastMessages;
        }
    }
}
