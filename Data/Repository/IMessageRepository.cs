using MiniSheenasNest.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniSheenasNest.Data.Repository
{
    public interface IMessageRepository
    {
        Task<Messages> GetMessageByIdAsync(int messageId); 
        Task<IEnumerable<Messages>> GetConversationAsync(string userA, string userB);
        Task<IEnumerable<Messages>> GetUnreadMessagesAsync(string userA, string userB);
        Task AddMessageAsync(Messages message);
        Task UpdateMessageAsync(Messages message);
        Task SaveChangesAsync();
    }

}
