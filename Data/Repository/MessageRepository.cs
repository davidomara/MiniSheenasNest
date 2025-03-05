using Microsoft.EntityFrameworkCore;
using MiniSheenasNest.Data.Repository;
using MiniSheenasNest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSheenasNest.Data.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext _context;

        public MessageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Messages> GetMessageByIdAsync(int messageId)
        {
            return await _context.Messages.FindAsync(messageId);
        }

        public async Task<IEnumerable<Messages>> GetConversationAsync(string userA, string userB)
        {
            // Filter for messages between the two given names, in either direction
            return await _context.Messages
                .Where(m =>
                    (m.SenderName == userA && m.ReceiverName == userB) ||
                    (m.SenderName == userB && m.ReceiverName == userA))
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Messages>> GetUnreadMessagesAsync(string senderName, string receiverName)
        {
            return await _context.Messages
                .Where(m => m.SenderName == senderName
                            && m.ReceiverName == receiverName
                            && m.IsRead == false)
                .ToListAsync();
        }

        public async Task AddMessageAsync(Messages message)
        {
            await _context.Messages.AddAsync(message);
        }

        public Task UpdateMessageAsync(Messages message)
        {
            _context.Messages.Update(message);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
