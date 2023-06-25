using Microsoft.EntityFrameworkCore;

using MyWebApp.Data;
using MyWebApp.Models;

namespace MyWebApp.Services;

public class PersonOperations : IDbOperations<Person>
{
  private readonly PersonContext _context;

  public class FilterNames
  {
    public const string Name = "name";
    public const string Gender = "gender";
    public const string BirthPlace = "birth_place";
  }

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

  public async Task<List<Person>> GetByFilters(Dictionary<string, string> filters)
  {
    var query = _context.Person.AsQueryable();
    if (filters.ContainsKey(FilterNames.Name))
    {
      query = query.Where(p => (p.FirstName != null && EF.Functions.Like(p.FirstName, $"%{filters[FilterNames.Name]}%")) || (p.LastName != null && EF.Functions.Like(p.LastName, $"%{filters[FilterNames.Name]}%")));
    }
    if (filters.ContainsKey(FilterNames.Gender))
    {
      query = query.Where(p => p.Gender == filters[FilterNames.Gender]);
    }
    if (filters.ContainsKey(FilterNames.BirthPlace))
    {
      query = query.Where(p => p.BirthPlace == filters[FilterNames.BirthPlace]);
    }
    return await query.ToListAsync();
  }
}