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
  public class ProductsController : ControllerBase
  {
    private readonly StoreContext _context;

    public ProductsController(StoreContext context)
    {
      _context = context;
    }

    // GET: api/Products
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductData>>> GetProduct()
    {
      if (_context.Product == null)
      {
        return NotFound();
      }
      return await _context.Product.Include(p => p.Categories).Select(
        p => new ProductData
        {
          ProductId = p.ProductId,
          ProductName = p.ProductName,
          Manufacture = p.Manufacture,
          Categories = p.Categories.Select(
            c => new CategoryData
            {
              CategoryId = c.CategoryId,
              CategoryName = c.CategoryName
            }
          ).ToList()
        }
      ).ToListAsync();
    }

    // GET: api/Products/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
      if (_context.Product == null)
      {
        return NotFound();
      }
      var product = await _context.Product.FindAsync(id);

      if (product == null)
      {
        return NotFound();
      }

      _context.Entry(product).Collection(p => p.Categories).Load();

      return product;
    }

    // PUT: api/Products/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProduct(int id, Product product)
    {
      if (id != product.ProductId)
      {
        return BadRequest();
      }

      _context.Entry(product).State = EntityState.Modified;

      var categoryIdList = product.Categories.Select(c => c.CategoryId).ToList();
      // Remove any ProductCategory records that don't exist in the new list.
      foreach (var productCategory in _context.ProductCategory)
      {
        if (productCategory.ProductId == id && !categoryIdList.Contains(productCategory.CategoryId))
        {
          _context.ProductCategory.Remove(productCategory);
        }
      }
      // Add any ProductCategory records that reference and existing category and don't exist in the old list.
      for (int i = 0; i < categoryIdList.Count; i++)
      {
        var categoryId = categoryIdList[i];
        var category = await _context.Category.FindAsync(categoryId);
        var productCategory = await _context.ProductCategory.FindAsync(id, categoryId);
        if (productCategory == null && category != null)
        {
          _context.ProductCategory.Add(new ProductCategory { ProductId = id, CategoryId = categoryId });
        }
      }

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!ProductExists(id))
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

    // POST: api/Products
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Product>> PostProduct(Product product)
    {
      if (_context.Product == null)
      {
        return Problem("Entity set 'StoreContext.Product' is null.");
      }
      product.ProductId = 0;  // Force a new ProductId.
      // Add product without categories.
      var categories = product.Categories;
      product.Categories = new();
      _context.Product.Add(product);
      await _context.SaveChangesAsync();

      // Update categories using the PutProduct method.
      product.Categories = categories;
      await PutProduct(product.ProductId, product);

      return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
    }

    // DELETE: api/Products/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
      if (_context.Product == null)
      {
        return NotFound();
      }
      var product = await _context.Product.FindAsync(id);
      if (product == null)
      {
        return NotFound();
      }

      _context.Product.Remove(product);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private bool ProductExists(int id)
    {
      return (_context.Product?.Any(e => e.ProductId == id)).GetValueOrDefault();
    }
  }
}
