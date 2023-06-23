using Microsoft.EntityFrameworkCore;

using MyWebApp.Data;
using MyWebApp.Models;

namespace MyWebApp.Interfaces;

public interface IDbOperations<T>
{
  bool CheckNull();
  Task Create(T entity);
  Task Update(T entity);
  Task Delete(int id);
  Task<List<T>> List();
  Task<T?> GetById(int id);
}

public class PersonOperations : IDbOperations<Person>
{
  private readonly PersonContext _context;

  public PersonOperations(PersonContext context)
  {
    _context = context;
  }

  public bool CheckNull()
  {
    return _context.Person == null;
  }

  public async Task Create(Person entity)
  {
    _context.Add(entity);
    await _context.SaveChangesAsync();
  }

  public async Task Update(Person entity)
  {
    _context.Update(entity);
    await _context.SaveChangesAsync();
  }

  public async Task Delete(int id)
  {
    var person = await _context.Person.FindAsync(id);
    if (person != null)
    {
      _context.Person.Remove(person);
    }
    await _context.SaveChangesAsync();
  }

  public async Task<List<Person>> List()
  {
    return await _context.Person.ToListAsync();
  }

  public async Task<Person?> GetById(int id)
  {
    return await _context.Person.FirstOrDefaultAsync(m => m.Id == id);
  }
}