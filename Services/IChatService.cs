using MiniSheenasNest.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniSheenasNest.Services
{
    public interface IChatService
    {
        Task<IEnumerable<Messages>> GetConversationAsync(string userA, string userB);
        Task<Messages> SendMessageAsync(string senderName, string receiverName, string content);
        Task MarkMessageAsReadAsync(int messageId);
        Task MarkAllMessagesAsReadAsync(string senderName, string receiverName);
        Task<Messages> GetMessageByIdAsync(int messageId);
        Task UpdateMessageAsync(Messages message);
    }
}
