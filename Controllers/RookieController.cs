using Microsoft.AspNetCore.Mvc;
using MyWebApp.Models;
using MyWebApp.Data;

namespace MyWebApp.Controllers;

public class RookieController : Controller
{
  private readonly ILogger<RookieController> _logger;
  private readonly PersonContext _context;

  public RookieController(ILogger<RookieController> logger, PersonContext context)
  {
    _logger = logger;
    _context = context;
  }

  public IActionResult Index()
  {
    return View();
  }

  public IActionResult A1E1()
  {
    var males = from p in _context.Person where p.Gender == "Male" select p;

    return View(males);
  }

  public IActionResult A1E2()
  {
    var oldest = (from p in _context.Person orderby p.DateOfBirth select p).FirstOrDefault();

    if (oldest == null)
    {
      return NotFound();
    }

    return View(oldest);
  }

  public IActionResult A1E3()
  {
    var fullname = from p in _context.Person select p.FullName;

    return View(fullname);
  }

  public IActionResult A1E4(int query = 0)
  {
    var year2000 = from p in _context.Person where p.DateOfBirth.Year == 2000 select p;
    ViewData["query"] = query;
    switch (query)
    {
      case 0:
        return View(year2000);
      case 1:
        var more2000 = from p in _context.Person where p.DateOfBirth.Year > 2000 select p;
        return View(more2000);
      case 2:
        var less2000 = from p in _context.Person where p.DateOfBirth.Year < 2000 select p;
        return View(less2000);
      default:
        return View(year2000);
    }
  }
}