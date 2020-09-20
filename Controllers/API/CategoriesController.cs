using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Chim_En_DOTNET.Data;
using Chim_En_DOTNET.Models;
using Chim_En_DOTNET.Helpers;

namespace Chim_En_DOTNET.Controllers_API
{
  [Route("api/[controller]")]
  [ApiController]
  public class CategoriesController : ControllerBase
  {
    private readonly ApplicationDbContext _context;

    public CategoriesController(ApplicationDbContext context)
    {
      _context = context;
    }

    // GET: api/Categories
    [HttpGet]
    public async Task<IActionResult> GetCategories([FromQuery] CategoryFilter categoryFilter)
    {
      CategoryFilter validFilter = new CategoryFilter(categoryFilter.PageNumber, categoryFilter.PageSize, categoryFilter.Search, categoryFilter.OrderBy, categoryFilter.Title);

      var categories = from c in _context.Categories select c;

      // Filter
      if (!string.IsNullOrEmpty(validFilter.Title))
      {
        categories = categories.Where(c => c.title == validFilter.Title);
      }

      if (!string.IsNullOrEmpty(validFilter.Search))
      {
        categories = categories.Where(c => c.title.ToLower().Contains(validFilter.Search.ToLower()));
      }

      // Sort
      switch (validFilter.OrderBy.ToLower())
      {
        case "title":
          categories = categories.OrderBy(c => c.title);
          break;
        case "-title":
          categories = categories.OrderByDescending(c => c.title);
          break;
        case "-createdat":
          categories = categories.OrderByDescending(c => c.CreatedAt);
          break;
        default:
          categories = categories.OrderBy(c => c.CreatedAt);
          break;
      }

      // Pagination
      categories = categories.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize);

      var data = await categories.ToListAsync();

      int totalRecord = await _context.Categories.CountAsync();
      return Ok(new PagedResponse<List<Category>>(data, validFilter.PageNumber, validFilter.PageSize, totalRecord));
    }

    // GET: api/Categories/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetCategory(int id)
    {
      var category = await _context.Categories.FindAsync(id);

      if (category == null)
      {
        return NotFound();
      }

      return category;
    }

    // PUT: api/Categories/5
    // To protect from overposting attacks, enable the specific properties you want to bind to, for
    // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCategory(int id, Category category)
    {
      if (id != category.Id)
      {
        return BadRequest();
      }

      _context.Entry(category).State = EntityState.Modified;

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
    // To protect from overposting attacks, enable the specific properties you want to bind to, for
    // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
    [HttpPost]
    public async Task<ActionResult<Category>> PostCategory(Category category)
    {
      _context.Categories.Add(category);
      await _context.SaveChangesAsync();

      return CreatedAtAction("GetCategory", new { id = category.Id }, category);
    }

    // DELETE: api/Categories/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<Category>> DeleteCategory(int id)
    {
      var category = await _context.Categories.FindAsync(id);
      if (category == null)
      {
        return NotFound();
      }

      _context.Categories.Remove(category);
      await _context.SaveChangesAsync();

      return category;
    }

    private bool CategoryExists(int id)
    {
      return _context.Categories.Any(e => e.Id == id);
    }
  }
}
