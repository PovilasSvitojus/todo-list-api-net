using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoList.Models;
using TodoList.Repo;

namespace TodoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class TasksController : ControllerBase
    {
        private readonly TodoListContext _context;

        public TasksController(TodoListContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Task>>> GetAllTasks()
        {
            return await _context.Tasks.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Task>> GetTaskByID(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            else
            {
                return task;
            }
        }

        [HttpGet("status")]
        public async Task<ActionResult<Models.Task>> GetByStatus([FromQuery(Name = "status")] string status)
        {
            List<Models.Task> tasksByStatus = new List<Models.Task>();  
            TasksRepo taskRepo = new TasksRepo(_context);
            tasksByStatus = taskRepo.GetTasksByStatus(status);

            return Ok(new { tasksByStatus });
        }

        [HttpPost]
        public async Task<ActionResult<Models.Task>> AddNewTask(Models.Task newTask)
        {
            _context.Tasks.Add(newTask);
            await _context.SaveChangesAsync();
            return CreatedAtAction("AddNewTask", new { id = newTask.TaskId }, newTask);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<Models.Task>> UpdateTaskById(int id, Models.Task updTask)
        {
            if (id != updTask.TaskId)
            {
                return BadRequest();
            }

            _context.Entry(updTask).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException dbce)
            {
                return NotFound();
            }
            return Ok(updTask);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<Models.Task>> DeleteTaskById(int id)
        {
            var Task = await _context.Tasks.FindAsync(id);
            if (Task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(Task);
            await _context.SaveChangesAsync();

            return Task;
        }

    }
}
