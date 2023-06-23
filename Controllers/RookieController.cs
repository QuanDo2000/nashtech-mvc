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
}