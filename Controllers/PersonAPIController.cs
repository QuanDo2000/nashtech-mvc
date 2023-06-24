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
  public class PersonAPIController : ControllerBase
  {
    private readonly PersonContext _context;

    public PersonAPIController(PersonContext context)
    {
      _context = context;
    }

    // GET: api/PersonAPI
    // Filter by name: api/PersonAPI?name=John
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Person>>> GetPerson([FromQuery(Name = "name")] string? name, [FromQuery(Name = "gender")] string? gender, [FromQuery(Name = "birth_place")] string? birthPlace)
    {
      if (_context.Person == null)
      {
        return NotFound();
      }
      if (name == null && gender == null && birthPlace == null)
      {
        return await _context.Person.ToListAsync();
      }
      var query = _context.Person.AsQueryable();
      if (name != null)
      {
        query = query.Where(p => (p.FirstName != null && EF.Functions.Like(p.FirstName, $"%{name}%")) || (p.LastName != null && EF.Functions.Like(p.LastName, $"%{name}%")));
      }
      if (gender != null)
      {
        query = query.Where(p => p.Gender == gender);
      }
      if (birthPlace != null)
      {
        query = query.Where(p => p.BirthPlace == birthPlace);
      }
      return await query.ToListAsync();
    }

    // GET: api/PersonAPI/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Person>> GetPerson(int id)
    {
      if (_context.Person == null)
      {
        return NotFound();
      }
      var person = await _context.Person.FindAsync(id);

      if (person == null)
      {
        return NotFound();
      }

      return person;
    }

    // PUT: api/PersonAPI/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPerson(int id, Person person)
    {
      if (id != person.Id)
      {
        return BadRequest();
      }

      _context.Entry(person).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!PersonExists(id))
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

    // POST: api/PersonAPI
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Person>> PostPerson(Person person)
    {
      if (_context.Person == null)
      {
        return Problem("Entity set 'PersonContext.Person'  is null.");
      }
      _context.Person.Add(person);
      await _context.SaveChangesAsync();

      return CreatedAtAction("GetPerson", new { id = person.Id }, person);
    }

    // DELETE: api/PersonAPI/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePerson(int id)
    {
      if (_context.Person == null)
      {
        return NotFound();
      }
      var person = await _context.Person.FindAsync(id);
      if (person == null)
      {
        return NotFound();
      }

      _context.Person.Remove(person);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private bool PersonExists(int id)
    {
      return (_context.Person?.Any(e => e.Id == id)).GetValueOrDefault();
    }
  }
}
