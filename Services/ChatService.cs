using MiniSheenasNest.Data;
using MiniSheenasNest.Data.Repository;
using MiniSheenasNest.Models;
using MiniSheenasNest.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniSheenasNest.Services
{
    public class ChatService : IChatService
    {
        private readonly IMessageRepository _messageRepo;

        public ChatService(IMessageRepository messageRepo)
        {
            _messageRepo = messageRepo;
        }

        public async Task<IEnumerable<Messages>> GetConversationAsync(string userA, string userB)
        {
            // The repository will look for messages where 
            // (SenderName == userA && ReceiverName == userB)
            //   OR
            // (SenderName == userB && ReceiverName == userA)
            return await _messageRepo.GetConversationAsync(userA, userB);
        }

        public async Task<Messages> SendMessageAsync(string senderName, string receiverName, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("Message content cannot be empty.");

            var message = new Messages
            {
                SenderName = senderName,
                ReceiverName = receiverName,
                Content = content,
                SentAt = DateTime.UtcNow
            };

            await _messageRepo.AddMessageAsync(message);
            await _messageRepo.SaveChangesAsync();
            return message;
        }

        public async Task MarkMessageAsReadAsync(int messageId)
        {
            var message = await _messageRepo.GetMessageByIdAsync(messageId);
            if (message == null) return;

            message.IsRead = true;
            message.ReadAt = DateTime.UtcNow;

            await _messageRepo.UpdateMessageAsync(message);
            await _messageRepo.SaveChangesAsync();
        }

        public async Task MarkAllMessagesAsReadAsync(string senderName, string receiverName)
        {
            // 1) Fetch unread messages where:
            //   SenderName == senderName
            //   ReceiverName == receiverName
            //   IsRead == false
            var unreadMessages = await _messageRepo.GetUnreadMessagesAsync(senderName, receiverName);

            if (!unreadMessages.Any()) return; // nothing to update

            // 2) Mark them all as read
            foreach (var msg in unreadMessages)
            {
                msg.IsRead = true;
                msg.ReadAt = DateTime.UtcNow;
                await _messageRepo.UpdateMessageAsync(msg);
            }

            // 3) Commit changes
            await _messageRepo.SaveChangesAsync();
        }
        public async Task<Messages> GetMessageByIdAsync(int messageId)
        {
            return await _messageRepo.GetMessageByIdAsync(messageId);
        }

        public async Task UpdateMessageAsync(Messages message)
        {
            await _messageRepo.UpdateMessageAsync(message);
            await _messageRepo.SaveChangesAsync();
        }

    }
}
