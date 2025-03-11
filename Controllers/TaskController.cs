using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MiniSheenasNest.Models;
using MiniSheenasNest.Services;

namespace MiniSheenasNest.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public TaskController(ITaskService taskService, UserManager<IdentityUser> userManager,
                                 RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _taskService = taskService;
        }

        // GET: /Task/Index
        public async Task<IActionResult> Index()
        {
            var tasks = await _taskService.GetAllTasksAsync();
            return View(tasks);
        }

        // GET: /Task/Create
        public async Task<IActionResult> Create()
        {
            // Get all users from Identity
            var users = _userManager.Users.ToList();
            // Create a select list using Id as the value and Email as the display text
            ViewBag.UsersList = new SelectList(users, "Id", "Email");
            return View();
        }

        // POST: /Task/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskItem task)
        {
            if (ModelState.IsValid)
            {
                await _taskService.CreateTaskAsync(task);
                return RedirectToAction(nameof(Index));
            }

            // Repopulate the drop-down list if the model state is invalid
            var users = _userManager.Users.ToList();
            ViewBag.UsersList = new SelectList(users, "Id", "Email", task.AssignedUserId);
            return View(task);
        }

        // GET: /Task/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
                return NotFound();

            // Populate the drop-down list for editing
            var users = _userManager.Users.ToList();
            ViewBag.UsersList = new SelectList(users, "Id", "Email", task.AssignedUserId);
            return View(task);
        }

        // POST: /Task/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TaskItem task)
        {
            if (id != task.TaskItemId)
                return BadRequest();

            if (ModelState.IsValid)
            {
                await _taskService.UpdateTaskAsync(task);
                return RedirectToAction(nameof(Index));
            }

            var users = _userManager.Users.ToList();
            ViewBag.UsersList = new SelectList(users, "Id", "Email", task.AssignedUserId);
            return View(task);
        }

        // (Optional) GET: /Task/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
                return NotFound();
            return View(task);
        }

        // (Optional) POST: /Task/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _taskService.DeleteTaskAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateTaskStatusModel model)
        {
            // model.TaskId and model.NewStatus are received
            // Update the task via your service
            await _taskService.UpdateTaskStatusAsync(model.TaskId, model.NewStatus);
            return Json(new { success = true });
        }
    }
}