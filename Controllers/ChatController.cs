using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniSheenasNest.Services;
using System.Threading.Tasks;

namespace MiniSheenasNest.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        // GET: /Chat/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Identify which user is currently logged in (either "Sheena" or "Nord")
            var (currentUser, otherUser) = DetermineUsersByEmail();

            // Load all messages between these two names
            var conversation = await _chatService.GetConversationAsync(currentUser, otherUser);

            // 2) Mark all messages that were sent *to* the current user as read
            //    (only if the user is actually the receiver for those messages)
            await _chatService.MarkAllMessagesAsReadAsync(otherUser, currentUser);


            // 3) Reload the conversation or keep the same data in memory:
            //    If you need the updated read states, re-fetch the conversation.
            //    Alternatively, if your list is referencing the same EF context, the changes might auto-reflect.
            conversation = await _chatService.GetConversationAsync(currentUser, otherUser);

            ViewBag.CurrentUser = currentUser; // e.g. "Sheena"
            ViewBag.OtherUser = otherUser;     // e.g. "Nord"

            return View(conversation);
        }

        // POST: /Chat/PostMessage
        [HttpPost]
        public async Task<IActionResult> PostMessage(string content)
        {
            var (currentUser, otherUser) = DetermineUsersByEmail();

            if (string.IsNullOrWhiteSpace(content))
            {
                ModelState.AddModelError("", "Message content cannot be empty.");
            }
            else
            {
                // Send the message from the logged-in user to the other
                await _chatService.SendMessageAsync(currentUser, otherUser, content);
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Looks at the current user's email and returns ("Sheena", "Nord")
        /// or ("Nord", "Sheena"), depending on who is logged in.
        /// </summary>
        private (string currentUser, string otherUser) DetermineUsersByEmail()
        {
            // Check the email claim:
            var userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value
                            ?? User.Identity.Name;

            // Hard-code your logic: if it's Sheena's email => currentUser = "Sheena"
            // else => assume it's Nord.
            if (userEmail?.Equals("sheena@gmail.com",
                System.StringComparison.OrdinalIgnoreCase) == true)
            {
                return ("Sheena", "Nord");
            }
            else
            {
                return ("Nord", "Sheena");
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditMessage(int messageId)
        {
            var (currentUser, _) = DetermineUsersByEmail();
            // e.g., currentUser = "Sheena" or "Nord"

            // 1) Fetch the message
            var message = await _chatService.GetMessageByIdAsync(messageId);
            if (message == null)
            {
                return NotFound(); // or redirect, or show an error
            }

            // 2) Check if the logged-in user is the sender and the message is unread
            if (message.SenderName != currentUser)
            {
                return Forbid("You can't edit someone else's message.");
            }

            if (message.IsRead)
            {
                return Forbid("You can't edit a message that has already been read.");
            }

            // 3) Pass the message to a view that shows an edit form
            return View(message);
        }
        [HttpPost]
        public async Task<IActionResult> EditMessage(int messageId, string updatedContent)
        {
            var (currentUser, _) = DetermineUsersByEmail();

            // 1) Fetch the message again
            var message = await _chatService.GetMessageByIdAsync(messageId);
            if (message == null)
            {
                return NotFound();
            }

            // 2) Validate conditions again
            if (message.SenderName != currentUser)
            {
                return Forbid("You can't edit someone else's message.");
            }

            if (message.IsRead)
            {
                return Forbid("You can't edit a message that has been read.");
            }

            // 3) If all good, update the content
            if (string.IsNullOrWhiteSpace(updatedContent))
            {
                ModelState.AddModelError("", "Message content cannot be empty.");
                return View("EditMessage", message);
            }

            message.Content = updatedContent;
            message.UpdatedAt = DateTime.Today;

            await _chatService.UpdateMessageAsync(message);

            // 4) Redirect back to chat or some confirmation
            return RedirectToAction(nameof(Index));
        }

    }
}
