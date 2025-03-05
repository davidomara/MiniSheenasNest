using Microsoft.AspNetCore.SignalR;
using MiniSheenasNest.Services;
using System.Threading.Tasks;

namespace MiniSheenasNest.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;

        public ChatHub(IChatService chatService)
        {
            _chatService = chatService;
        }

        // Called by clients on connection to identify the user’s group
        public async Task JoinUserGroup(int userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
        }

        // Called by clients to send a message in real-time
        public async Task SendMessage(string SenderName, string ReceiverName, string content)
        {
            // Save to database
            var message = await _chatService.SendMessageAsync(SenderName, ReceiverName, content);

            // Broadcast to sender & receiver
            await Clients.Group($"user_{ReceiverName}").SendAsync("ReceiveMessage", message);
            await Clients.Group($"user_{SenderName}").SendAsync("ReceiveMessage", message);
        }

        // Potential Enhancement: Mark a message as read in real-time
        public async Task MarkMessageRead(int messageId)
        {
            await _chatService.MarkMessageAsReadAsync(messageId);
            // Broadcast the update to whoever needs it
            await Clients.All.SendAsync("MessageRead", messageId);
        }
    }
}
