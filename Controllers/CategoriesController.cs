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
  public class CategoriesController : ControllerBase
  {
    private readonly StoreContext _context;

    public CategoriesController(StoreContext context)
    {
      _context = context;
    }

    // GET: api/Categories
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryData>>> GetCategory()
    {
      if (_context.Category == null)
      {
        return NotFound();
      }
      return await _context.Category.Include(c => c.Products).Select(
        c => new CategoryData
        {
          CategoryId = c.CategoryId,
          CategoryName = c.CategoryName,
          Products = c.Products.Select(
            p => new ProductData
            {
              ProductId = p.ProductId,
              ProductName = p.ProductName,
              Manufacture = p.Manufacture
            }
          ).ToList()
        }
      ).ToListAsync();
    }

    // GET: api/Categories/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetCategory(int id)
    {
      if (_context.Category == null)
      {
        return NotFound();
      }
      var category = await _context.Category.FindAsync(id);

      if (category == null)
      {
        return NotFound();
      }

      _context.Entry(category).Collection(c => c.Products).Load();

      return category;
    }

    // PUT: api/Categories/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCategory(int id, Category category)
    {
      if (id != category.CategoryId)
      {
        return BadRequest();
      }

      _context.Entry(category).State = EntityState.Modified;

      var productIdList = category.Products.Select(p => p.ProductId).ToList();
      // Remove any ProductCategory records that are not in the new list.
      foreach (var productCategory in _context.ProductCategory)
      {
        if (productCategory.CategoryId == id && !productIdList.Contains(productCategory.ProductId))
        {
          _context.ProductCategory.Remove(productCategory);
        }
      }
      // Add any ProductCategory records that reference an existing product and did not exist before.
      for (int i = 0; i < productIdList.Count; i++)
      {
        var productId = productIdList[i];
        var product = await _context.Product.FindAsync(productId);
        var productCategory = await _context.ProductCategory.FindAsync(productId, id);
        if (productCategory == null && product != null)
        {
          _context.ProductCategory.Add(new ProductCategory
          {
            ProductId = productId,
            CategoryId = id
          });
        }
      }

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!CategoryExists(id))
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

    // POST: api/Categories
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Category>> PostCategory(Category category)
    {
      if (_context.Category == null)
      {
        return Problem("Entity set 'StoreContext.Category' is null.");
      }
      category.CategoryId = 0;  // Force a new ID.
      // Add the category without products.
      var products = category.Products;
      category.Products = new();
      _context.Category.Add(category);
      await _context.SaveChangesAsync();

      // Update products using the PutCategory method.
      category.Products = products;
      await PutCategory(category.CategoryId, category);

      return CreatedAtAction("GetCategory", new { id = category.CategoryId }, category);
    }

    // DELETE: api/Categories/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
      if (_context.Category == null)
      {
        return NotFound();
      }
      var category = await _context.Category.FindAsync(id);
      if (category == null)
      {
        return NotFound();
      }

      _context.Category.Remove(category);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private bool CategoryExists(int id)
    {
      return (_context.Category?.Any(e => e.CategoryId == id)).GetValueOrDefault();
    }
  }
}
