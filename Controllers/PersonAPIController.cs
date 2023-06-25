using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using MyWebApp.Data;
using MyWebApp.Services;
using MyWebApp.Models;

namespace MyWebApp.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class PersonAPIController : ControllerBase
  {
    // private readonly PersonContext _context;
    private readonly IDbOperations<Person> _context;

    public PersonAPIController(IDbOperations<Person> context)
    {
      _context = context;
    }

    // GET: api/PersonAPI
    // Filter by name: api/PersonAPI?name=John
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Person>>> GetPerson([FromQuery(Name = "name")] string? name, [FromQuery(Name = "gender")] string? gender, [FromQuery(Name = "birth_place")] string? birthPlace)
    {
      if (_context.CheckNull())
      {
        return NotFound();
      }
      var filters = new Dictionary<string, string>();
      if (name != null)
      {
        filters.Add(PersonOperations.FilterNames.Name, name);
      }
      if (gender != null)
      {
        filters.Add(PersonOperations.FilterNames.Gender, gender);
      }
      if (birthPlace != null)
      {
        filters.Add(PersonOperations.FilterNames.BirthPlace, birthPlace);
      }
      return await _context.GetByFilters(filters);
    }

    // GET: api/PersonAPI/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Person>> GetPerson(int id)
    {
      if (_context.CheckNull())
      {
        return NotFound();
      }
      var person = await _context.GetById(id);

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

      try
      {
        await _context.Update(person);
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
      if (_context.CheckNull())
      {
        return Problem("Entity set 'PersonContext.Person'  is null.");
      }
      await _context.Create(person);

      return CreatedAtAction("GetPerson", new { id = person.Id }, person);
    }

    // DELETE: api/PersonAPI/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePerson(int id)
    {
      if (_context.CheckNull())
      {
        return NotFound();
      }
      var person = await _context.GetById(id);
      if (person == null)
      {
        return NotFound();
      }

      await _context.Delete(id);
      return NoContent();
    }

    private bool PersonExists(int id)
    {
      return _context.GetById(id) != null;
    }
  }
}
