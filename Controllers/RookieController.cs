using Microsoft.AspNetCore.Mvc;

using MyWebApp.Models;

namespace MyWebApp.Controllers;

public class RookieController : Controller
{
  private readonly ILogger<RookieController> _logger;

  public RookieController(ILogger<RookieController> logger)
  {
    _logger = logger;
  }

  public IActionResult Index()
  {
    return View();
  }
}