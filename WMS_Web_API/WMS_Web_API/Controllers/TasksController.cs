using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WMS_Web_API;

namespace WMS_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ApiContext _context;
        
        public class ListDisplay
        { 
            public string task_id { get; set; }
        }


        public TasksController(ApiContext context)
        {
            _context = context;
        }

        // GET: api/Tasks
        [HttpGet]
        //public async Task<ActionResult<IEnumerable<Tasks>>> GetTasks()
        public ActionResult<ListDisplay> GetTasks()
        {
            if (_context.Tasks == null)
            {
                return NotFound();
            }

            var newList = new ListDisplay();

            foreach (var item in _context.Tasks)
            {
                newList.task_id = item.id.ToString();

            }
            return newList;
            //return await _context.Tasks.ToListAsync();
        }

        // GET: api/Tasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tasks>> GetTasks(long id)
        {
          if (_context.Tasks == null)
          {
              return NotFound();
          }
            var tasks = await _context.Tasks.FindAsync(id);

            if (tasks == null)
            {
                return NotFound();
            }

            return tasks;
        }

        // PUT: api/Tasks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTasks(long id, Tasks tasks)
        {
            if (id != tasks.id)
            {
                return BadRequest();
            }

            _context.Entry(tasks).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TasksExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Tasks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tasks>> PostTasks(Tasks tasks)
        {
          if (_context.Tasks == null)
          {
              return Problem("Entity set 'ApiContext.Tasks'  is null.");
          }
            _context.Tasks.Add(tasks);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTasks", new { id = tasks.id }, tasks);             
        }

        // DELETE: api/Tasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTasks(long id)
        {
            if (_context.Tasks == null)
            {
                return NotFound();
            }
            var tasks = await _context.Tasks.FindAsync(id);
            if (tasks == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(tasks);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TasksExists(long id)
        {
            return (_context.Tasks?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
