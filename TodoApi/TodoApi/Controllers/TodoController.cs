using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoController(TodoContext context)
        {
            _context = context;

            if (_context.TodoItems.Count() == 0)
            {
                // Create a new TodoItem if collection is empty,
                // which means you can't delete all TodoItems.
                _context.TodoItems.Add(new TodoItem { ID = "1", Name = "Item1", Notes = "Test1" });
                _context.SaveChanges();
            }
        }

        // GET: api/Todo
        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            var currentUser = HttpContext.User;

            return await _context.TodoItems.ToListAsync();
        }

        // GET: api/Todo/5
        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            var currentUser = HttpContext.User;

            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        // POST: api/Todo
        [HttpPost, Authorize]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem item)
        {
            var currentUser = HttpContext.User;

            _context.TodoItems.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItem), new { id = item.ID }, item);
        }

        // PUT: api/Todo/5
        [HttpPut("{id}"), Authorize]
        public async Task<IActionResult> PutTodoItem(string id, TodoItem item)
        {
            var currentUser = HttpContext.User;

            if (id != item.ID)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Todo/5
        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> DeleteTodoItem(string id)
        {
            var currentUser = HttpContext.User;

            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}