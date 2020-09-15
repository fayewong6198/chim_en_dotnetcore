using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Chim_En_DOTNET.Data;
using Chim_En_DOTNET.Models;
using System.IO;
using Slugify;
using Chim_En_DOTNET.Helpers;

namespace Chim_En_DOTNET.Controllers_API
{
  [Route("api/[controller]")]
  [ApiController]
  public class ProductsController : ControllerBase
  {
    private readonly ApplicationDbContext _context;

    public ProductsController(ApplicationDbContext context)
    {
      _context = context;
    }

    // GET: api/Products
    [HttpGet]
    public async Task<ActionResult> GetProducts([FromQuery] ProductFilter filter)
    {
      var validFilter = new ProductFilter(filter.PageNumber, filter.PageSize, filter.CategoryId, filter.Search, filter.OrderBy, filter.Active, filter.Available);
      var products = from p in _context.Products select p;

      // Filter
      if (validFilter.CategoryId >= 0)
      {
        products = products.Where(p => p.CategoryId == validFilter.CategoryId);
      }

      if (!String.IsNullOrEmpty(validFilter.Search))
      {
        products = products.Where(p => p.Title.ToLower().Contains(validFilter.Search.ToLower()));
      }

      if (validFilter.Active != null)
      {
        products = products.Where(p => p.Active == validFilter.Active);
      }

      products = products.Where(p => p.Available >= validFilter.Available);

      // Sort
      Console.WriteLine(validFilter.OrderBy.ToLower());

      switch (validFilter.OrderBy.ToLower())
      {
        case "title":
          products = products.OrderBy(p => p.Title);
          break;
        case "-title":
          products = products.OrderByDescending(p => p.Title);
          break;
        case "price":
          products = products.OrderBy(p => p.Price);
          break;
        case "-price":
          products = products.OrderByDescending(p => p.Price);
          break;
        case "-createdAt":
        case "-createdat":
          products = products.OrderByDescending(p => p.CreatedAt);
          break;
        default:
          products = products.OrderBy(p => p.CreatedAt);
          break;
      }

      // Pagination
      products = products.Include(p => p.Category).Include(p => p.Images)
                      .Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize);
      var data = await products.AsNoTracking().ToListAsync();

      var totalRecord = await _context.Products.CountAsync();
      return Ok(new PagedResponse<List<Product>>(data, validFilter.PageNumber, validFilter.PageSize, totalRecord));
    }

    // GET: api/Products/5
    [HttpGet("{slug}")]
    public async Task<ActionResult> GetProduct(string slug)
    {

      var product = await _context.Products.Include(p => p.Category).Include(p => p.Images).AsNoTracking().FirstOrDefaultAsync(p => p.Slug.Equals(slug));

      if (product == null)
      {
        return NotFound();
      }

      return Ok(new Response<Product>(product));
    }

    // PUT: api/Products/5
    // To protect from overposting attacks, enable the specific properties you want to bind to, for
    // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProduct(int id, Product product)
    {
      if (id != product.Id)
      {
        return BadRequest();
      }

      if (String.IsNullOrEmpty(product.Slug) && !String.IsNullOrEmpty(product.Title))
      {
        SlugHelper helper = new SlugHelper();
        product.Slug = helper.GenerateSlug(product.Title);
      }

      _context.Entry(product).State = EntityState.Modified;

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

      return Ok(product);
    }

    // POST: api/Products
    // To protect from overposting attacks, enable the specific properties you want to bind to, for
    // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
    [HttpPost]
    public async Task<ActionResult> PostProduct(Product product)
    {
      if (String.IsNullOrEmpty(product.Slug) && !String.IsNullOrEmpty(product.Title))
      {
        SlugHelper helper = new SlugHelper();
        product.Slug = helper.GenerateSlug(product.Title);
      }
      _context.Products.Add(product);
      await _context.SaveChangesAsync();

      Product createdProduct = await _context.Products.Include(p => p.Images).Include(p => p.Category).AsNoTracking().FirstOrDefaultAsync(p => p.Id == product.Id);
      return Ok(createdProduct);
    }

    // DELETE: api/Products/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<Product>> DeleteProduct(int id)
    {
      var product = await _context.Products.FindAsync(id);
      if (product == null)
      {
        return NotFound();
      }

      _context.Products.Remove(product);
      await _context.SaveChangesAsync();

      return product;
    }

    [HttpPost("{productId}/images")]
    public async Task<IActionResult> CreateProductImages(int productId, [FromForm] ProductImagesParams productImagesParams)
    {

      // Delete Old Image
      var files = productImagesParams.Files;
      // var productId = productImagesParams.ProductId;

      var productImages = await _context.ProductImages.Where(p => p.ProductId == productId).ToListAsync();
      foreach (var productImage in productImages)
      {
        // var filePath = Path.Combine("wwwroot/", productImage.Url);
        var filePath = String.Concat("wwwroot", productImage.Url);
        Console.WriteLine(filePath);
        try
        {
          System.IO.File.Delete(filePath);

        }
        catch
        {
          Console.WriteLine("Path not found");
        }
      }

      _context.ProductImages.RemoveRange(productImages);

      await _context.SaveChangesAsync();

      Console.WriteLine(productId);
      long size = files.Sum(f => f.Length);

      foreach (var formFile in files)
      {
        if (formFile.Length > 0)
        {
          var url = String.Concat(Path.GetRandomFileName(), ".jpg");
          var filePath = Path.Combine("wwwroot/ProductImages/", url);

          using (var stream = System.IO.File.Create(filePath))
          {
            await formFile.CopyToAsync(stream);
          }

          ProductImage productImage = new ProductImage();
          productImage.ProductId = productId;
          productImage.Url = String.Concat("/ProductImages/", url);
          _context.Add(productImage);
          await _context.SaveChangesAsync();
        }
      }


      return Ok(new
      {
        count = files.Count,
        size
      });
    }

    private async void DeleteOldImages(int productId)
    {
      var productImages = await _context.ProductImages.Where(p => p.ProductId == productId).ToListAsync();
      foreach (var productImage in productImages)
      {
        var filePath = Path.Combine("wwwroot/", productImage.Url);
        Console.WriteLine(filePath);
        try
        {
          System.IO.File.Delete(filePath);

        }
        catch
        {
          Console.WriteLine("Path not found");
        }
      }

      _context.ProductImages.RemoveRange(productImages);

      await _context.SaveChangesAsync();
    }

    private bool ProductExists(int id)
    {
      return _context.Products.Any(e => e.Id == id);
    }
  }

  public class ProductImagesParams
  {
    // public int ProductId { get; set; }
    public List<IFormFile> Files { get; set; }
  }
}
