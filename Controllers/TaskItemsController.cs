using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using MyWebApp.Data;
using MyWebApp.Models;

namespace MyWebApp.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class TaskItemsController : ControllerBase
  {
    private readonly TaskContext _context;

    public TaskItemsController(TaskContext context)
    {
      _context = context;
    }

    // GET: api/TaskItems
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetTaskItems()
    {
      if (_context.TaskItems == null)
      {
        return NotFound();
      }
      return await _context.TaskItems.ToListAsync();
    }

    // GET: api/TaskItems/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItem>> GetTaskItem(long id)
    {
      if (_context.TaskItems == null)
      {
        return NotFound();
      }
      var taskItem = await _context.TaskItems.FindAsync(id);

      if (taskItem == null)
      {
        return NotFound();
      }

      return taskItem;
    }

    // PUT: api/TaskItems/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTaskItem(long id, TaskItem taskItem)
    {
      if (id != taskItem.Id)
      {
        return BadRequest();
      }

      _context.Entry(taskItem).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!TaskItemExists(id))
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

    // POST: api/TaskItems
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<TaskItem>> PostTaskItem(TaskItem taskItem)
    {
      if (_context.TaskItems == null)
      {
        return Problem("Entity set 'TaskContext.TaskItems'  is null.");
      }
      taskItem = new TaskItem { Title = taskItem.Title, IsComplete = taskItem.IsComplete };
      _context.TaskItems.Add(taskItem);
      await _context.SaveChangesAsync();

      return CreatedAtAction("GetTaskItem", new { id = taskItem.Id }, taskItem);
    }

    // DELETE: api/TaskItems/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTaskItem(long id)
    {
      if (_context.TaskItems == null)
      {
        return NotFound();
      }
      var taskItem = await _context.TaskItems.FindAsync(id);
      if (taskItem == null)
      {
        return NotFound();
      }

      _context.TaskItems.Remove(taskItem);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    // POST: api/TaskItems/BulkAdd
    [Route("BulkAdd")]
    [HttpPost]
    public async Task<ActionResult<List<TaskItem>>> PostTaskItems(List<TaskItem> taskItems)
    {
      if (_context.TaskItems == null)
      {
        return Problem("Entity set 'TaskContext.TaskItems'  is null.");
      }

      // Remove Id values from the taskItems
      taskItems.ForEach(t => new TaskItem { Title = t.Title, IsComplete = t.IsComplete });

      _context.TaskItems.AddRange(taskItems);
      await _context.SaveChangesAsync();

      return CreatedAtAction("GetTaskItems", taskItems);
    }

    // DELETE: api/TaskItems/BulkDelete
    [Route("BulkDelete")]
    [HttpDelete]
    public async Task<IActionResult> DeleteTaskItems(List<long> ids)
    {
      if (_context.TaskItems == null)
      {
        return NotFound();
      }
      var taskItems = await _context.TaskItems.Where(t => ids.Contains(t.Id)).ToListAsync();
      if (taskItems == null)
      {
        return NotFound();
      }

      _context.TaskItems.RemoveRange(taskItems);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private bool TaskItemExists(long id)
    {
      return (_context.TaskItems?.Any(e => e.Id == id)).GetValueOrDefault();
    }
  }
}
