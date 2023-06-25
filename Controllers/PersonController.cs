using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using MyWebApp.Data;
using MyWebApp.Models;
using MyWebApp.Services;

namespace MyWebApp.Controllers
{
  public class PersonController : Controller
  {
    // private readonly PersonContext _context;
    private readonly IDbOperations<Person> _context;

    public PersonController(IDbOperations<Person> context)
    {
      _context = context;
    }

    // GET: Person
    public async Task<IActionResult> Index()
    {
      return _context.CheckNull() ?
                    Problem("Entity set 'PersonContext.Person'  is null.") :
                    View(await _context.List());
    }

    // GET: Person/Details/5
    public async Task<IActionResult> Details(int? id)
    {
      if (id == null || _context.CheckNull())
      {
        return NotFound();
      }

      var person = await _context.GetById(id.Value);
      if (person == null)
      {
        return NotFound();
      }

      return View(person);
    }

    // GET: Person/Create
    public IActionResult Create()
    {
      return View();
    }

    // POST: Person/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Gender,DateOfBirth,PhoneNumber,BirthPlace,IsGraduated")] Person person)
    {
      if (ModelState.IsValid)
      {
        await _context.Create(person);
        return RedirectToAction(nameof(Index));
      }
      return View(person);
    }

    // GET: Person/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null || _context.CheckNull())
      {
        return NotFound();
      }

      var person = await _context.GetById(id.Value);
      if (person == null)
      {
        return NotFound();
      }
      return View(person);
    }

    // POST: Person/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Gender,DateOfBirth,PhoneNumber,BirthPlace,IsGraduated")] Person person)
    {
      if (id != person.Id)
      {
        return NotFound();
      }

      if (ModelState.IsValid)
      {
        try
        {
          await _context.Update(person);
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!PersonExists(person.Id))
          {
            return NotFound();
          }
          else
          {
            throw;
          }
        }
        return RedirectToAction(nameof(Index));
      }
      return View(person);
    }

    // GET: Person/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null || _context.CheckNull())
      {
        return NotFound();
      }

      var person = await _context.GetById(id.Value);
      if (person == null)
      {
        return NotFound();
      }

      return View(person);
    }

    // POST: Person/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      if (_context.CheckNull())
      {
        return Problem("Entity set 'PersonContext.Person'  is null.");
      }
      var person = await _context.GetById(id);
      if (person == null)
      {
        return NotFound();
      }

      await _context.Delete(id);

      return RedirectToAction(nameof(DeleteConfirmed), new { name = person.FullName });
    }

    public IActionResult DeleteConfirmed(string name)
    {
      ViewData["name"] = name;
      return View();
    }

    private bool PersonExists(int id)
    {
      return _context.CheckNull() ?
                    throw new Exception("Entity set 'PersonContext.Person'  is null.") :
                    _context.GetById(id) != null;
    }
  }
}
