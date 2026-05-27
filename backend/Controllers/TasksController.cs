using backend.Models;
using backend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [ApiController]
    [Route("tasks")]
    public class TasksController : Controller
    {
        private readonly AppDbContext _context;
        public TasksController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<TaskItem>> GetTasks()
        {
            return await _context.Tasks.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask(Guid id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(TaskItem task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return Ok(new { id = task.Id });
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateTask(Guid id, UpdateTaskDto updatedTask)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(updatedTask.Title))
                task.Title = updatedTask.Title;

            if (updatedTask.IsDone.HasValue)
                task.IsDone = updatedTask.IsDone.Value;

            await _context.SaveChangesAsync();

            return Ok(task);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
                return NotFound();

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAllTasks()
        {
            var allTasks = _context.Tasks.ToList();

            if (!allTasks.Any())
                return NoContent();

            _context.Tasks.RemoveRange(allTasks);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
