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

        public TasksController(ApiContext context)
        {
            _context = context;
        }

        [HttpGet("{id?}", Name = "GetTask")]
        //public IActionResult GetTask(long? id)
        public IActionResult GetTask(long? id)
        {
            if (id.HasValue && id.Value != 0)
            {
                var task = _context.Tasks.FirstOrDefault(t => t.id == id);
                if (task == null)
                {
                    return NotFound();
                }
                //return Ok(task); // Return a specific task
                return new JsonResult(task);
            }
            else
            {
                // If no 'id' is provided or 'id' is 0, return the list of all tasks
                var tasks = _context.Tasks.ToList();
                if (tasks == null)
                {
                    return NotFound();
                }
                return new JsonResult(tasks); // Return a list of tasks as JSON
            }
        }


        // Get Requst But Filtered by ID, Type And Status
        // [Required] Status
        // [Optional] ID, Type
        [HttpGet("list/", Name = "GetTaskList")]
        public IActionResult GetTaskList(long? id, string? _type, string _status)
        {

            try
            {
                if (_context.Tasks == null)
                {
                    return NotFound();
                }
                else
                {

                    var filteredTasks = _context.Tasks.Where(task =>
                                        (!id.HasValue || task.id == id) &&
                                        (_type == null || task.type == _type) &&
                                        task.status == _status
                                         ).ToList();

                    if (filteredTasks.Count > 0)
                    {
                        return new JsonResult(filteredTasks);
                    }
                    else
                    {
                        return NotFound();
                    }
                }



            }
            catch (Exception ex)
            { return StatusCode(500, "An error occurred"); }

           
        }



        //// GET: api/Tasks/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Tasks>> GetTask(long id)
        //{
        //    if (_context.Tasks == null)
        //    {
        //        return NotFound();
        //    }
        //    var tasks = await _context.Tasks.FindAsync(id);

        //    if (tasks == null)
        //    {
        //        return NotFound();
        //    }

        //    return tasks;
        //}

        // PUT: api/Tasks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [HttpPut("{id}", Name = "UpdateTask")]
        public async Task<IActionResult> PutTask(long id, Tasks tasks)
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
        [HttpPost(Name = "PostTask")]
        public async Task<ActionResult<Tasks>> PostTask(Tasks tasks)
        {
            if (_context.Tasks == null)
            {
                return Problem("Entity set 'ApiContext.Tasks'  is null.");
            }
            _context.Tasks.Add(tasks);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTask", new { id = tasks.id }, tasks);
        }

        // DELETE: api/Tasks/5
        [HttpDelete("{id}", Name = "DeleteTask")]
        public async Task<IActionResult> DeleteTask(long id)
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

        // PUT: api/Tasks/5
        [HttpPut("Store/{id}", Name ="StoreTask")]
        public async Task<IActionResult> StoreTask(long id, Tasks tasks)
        {
            try
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
                    return NotFound();
                }

                //return CreatedAtAction()
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred");
            }

          
        }


        private bool TasksExists(long id)
        {
            return (_context.Tasks?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
