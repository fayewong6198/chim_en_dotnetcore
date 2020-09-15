using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Chim_En_DOTNET.Data;
using Chim_En_DOTNET.Models;
using System.Security.Claims;

namespace Chim_En_DOTNET.Controllers.API
{
  [Route("api/[controller]")]
  [ApiController]
  public class UserActionsController : ControllerBase
  {
    private readonly ApplicationDbContext _context;

    public UserActionsController(ApplicationDbContext context)
    {
      _context = context;

    }

    [HttpPost("Cart")]
    public async Task<IActionResult> AddToCart(AddToCartParams addToCartParams)
    {
      var product = await _context.Products.FindAsync(addToCartParams.ProductId);

      if (product == null)
      {
        return NotFound(new { msg = "Product not found" });
      }


      string sessionId = HttpContext.Request.Headers["SessionId"].FirstOrDefault();

      if (string.IsNullOrEmpty(sessionId) && !User.Identity.IsAuthenticated)
      {
        Console.WriteLine(User.Identity.IsAuthenticated);
        return Unauthorized();

      }


      Cart cart = new Cart();
      if (User.Identity.IsAuthenticated)
      {
        string userId = User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier)).Value;
        cart = await _context.Carts.Where(c => c.ApplicationUserId.Equals(userId)).FirstOrDefaultAsync();

        if (cart == null)
        {
          cart = new Cart();
          cart.ApplicationUserId = userId;
          _context.Carts.Add(cart);
          await _context.SaveChangesAsync();
        }
      }
      else if (!string.IsNullOrEmpty(sessionId))
      {
        cart = await _context.Carts.Where(c => c.SessionId.Equals(sessionId)).FirstOrDefaultAsync();
        if (cart == null)
        {
          cart = new Cart();
          cart.SessionId = sessionId;
          _context.Carts.Add(cart);
          await _context.SaveChangesAsync();
        }
      }

      var cartItem = await _context.CartItems.Where(c => c.Id == cart.Id && c.ProductId == addToCartParams.ProductId).FirstOrDefaultAsync();

      if (cartItem == null)
      {
        if (product.Available < addToCartParams.Quantity)
        {
          return BadRequest(new { msg = $"Product only contains {product.Available} items less" });
        }
        cartItem = new CartItem();
        cartItem.CartId = cart.Id;
        cartItem.ProductId = addToCartParams.ProductId;
        cartItem.Quantity = addToCartParams.Quantity;
        _context.CartItems.Add(cartItem);
      }

      else
      {
        if (product.Available < addToCartParams.Quantity + cartItem.Quantity)
        {
          return BadRequest(new { msg = $"Product only contains {product.Available} items less" });
        }
        cartItem.Quantity = cartItem.Quantity + addToCartParams.Quantity;
      }
      await _context.SaveChangesAsync();



      if (User.Identity.IsAuthenticated)
      {
        string userId = User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier)).Value;
        cart = await _context.Carts.Where(c => c.ApplicationUserId.Equals(userId)).Include(c => c.CartItems).FirstOrDefaultAsync();
      }

      else
      {
        cart = await _context.Carts.Where(c => c.SessionId.Equals(sessionId)).Include(c => c.CartItems).FirstOrDefaultAsync();

      }
      return Ok(cart);
    }

    [HttpPut("Cart")]
    public async Task<IActionResult> ChangeQuantity(ChangeQuantityParams changeQuantityParams)
    {
      var product = await _context.Products.FindAsync(changeQuantityParams.ProductId);

      if (product == null)
      {
        return NotFound(new { msg = "Product not found" });
      }

      string sessionId = HttpContext.Request.Headers["SessionId"].FirstOrDefault();

      if (string.IsNullOrEmpty(sessionId) && !User.Identity.IsAuthenticated)
      {
        return Unauthorized();
      }

      Cart cart = new Cart();
      CartItem cartItem = new CartItem();
      if (User.Identity.IsAuthenticated)
      {
        string userId = User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier)).Value;
        cart = await _context.Carts.Where(c => c.ApplicationUserId.Equals(userId)).FirstOrDefaultAsync();

      }
      else if (!string.IsNullOrEmpty(sessionId))
      {
        cart = await _context.Carts.Where(c => c.SessionId.Equals(sessionId)).Include(c => c.CartItems).FirstOrDefaultAsync();
      }

      if (cart == null)
      {
        return BadRequest(new { msg = "Cart not found" });
      }

      cartItem = await _context.CartItems.Where(c => c.CartId == cart.Id && c.ProductId == changeQuantityParams.ProductId).FirstOrDefaultAsync();
      if (cartItem == null)
      {
        return BadRequest(new { msg = "CartItem not found" });
      }

      if (product.Available < changeQuantityParams.Quantity)
      {
        return BadRequest(new { msg = $"Product only contains {product.Available} items less" });
      }

      cartItem.Quantity = changeQuantityParams.Quantity;
      if (cartItem.Quantity <= 0)
      {
        _context.CartItems.Remove(cartItem);
      }

      await _context.SaveChangesAsync();

      if (User.Identity.IsAuthenticated)
      {
        string userId = User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier)).Value;
        cart = await _context.Carts.Where(c => c.ApplicationUserId.Equals(userId)).Include(c => c.CartItems).FirstOrDefaultAsync();
      }
      else
      {
        cart = await _context.Carts.Where(c => c.SessionId.Equals(sessionId)).Include(c => c.CartItems).FirstOrDefaultAsync();

      }
      return Ok(cart);

    }

    [HttpDelete("Cart/{productId}")]
    public async Task<IActionResult> DeleteFromCart(int productId)
    {
      var product = await _context.Products.FindAsync(productId);

      if (product == null)
      {
        return NotFound(new { msg = "Product not found" });
      }

      string sessionId = HttpContext.Request.Headers["SessionId"].FirstOrDefault();

      if (string.IsNullOrEmpty(sessionId) && !User.Identity.IsAuthenticated)
      {
        return Unauthorized();
      }

      Cart cart = new Cart();
      CartItem cartItem = new CartItem();
      if (User.Identity.IsAuthenticated)
      {
        string userId = User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier)).Value;
        cart = await _context.Carts.Where(c => c.ApplicationUserId.Equals(userId)).FirstOrDefaultAsync();



      }
      else if (!string.IsNullOrEmpty(sessionId))
      {
        cart = await _context.Carts.Where(c => c.SessionId.Equals(sessionId)).FirstOrDefaultAsync();

      }

      if (cart == null)
      {
        return NotFound(new { msg = "Cart not found" });
      }
      cartItem = await _context.CartItems.Where(c => c.CartId == cart.Id && c.ProductId == productId).FirstOrDefaultAsync();

      if (cartItem == null)
      {
        return NotFound(new { msg = "CartItem not found" });
      }

      _context.CartItems.Remove(cartItem);
      await _context.SaveChangesAsync();
      if (User.Identity.IsAuthenticated)
      {
        string userId = User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier)).Value;
        cart = await _context.Carts.Where(c => c.ApplicationUserId.Equals(userId)).Include(c => c.CartItems).FirstOrDefaultAsync();
      }
      else
      {
        cart = await _context.Carts.Where(c => c.SessionId.Equals(sessionId)).Include(c => c.CartItems).FirstOrDefaultAsync();

      }
      return Ok(cart);
    }

    [HttpGet("Cart")]
    public async Task<IActionResult> GetCart()
    {
      string sessionId = HttpContext.Request.Headers["SessionId"].FirstOrDefault();

      if (User.Identity.IsAuthenticated)
      {
        string userId = User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier)).Value;

        var cart = await _context.Carts.Where(c => c.ApplicationUserId.Equals(userId)).Include(c => c.CartItems).AsNoTracking().FirstOrDefaultAsync();
        if (cart == null)
        {
          cart = new Cart();
          cart.ApplicationUserId = userId;
          _context.Carts.Add(cart);
          await _context.SaveChangesAsync();
        }

        return Ok(cart);
      }

      else if (!string.IsNullOrEmpty(sessionId))
      {
        var cart = await _context.Carts.Where(c => c.SessionId.Equals(sessionId)).Include(c => c.CartItems).AsNoTracking().FirstOrDefaultAsync();
        if (cart == null)
        {
          cart = new Cart();
          cart.SessionId = sessionId;
          _context.Carts.Add(cart);
          await _context.SaveChangesAsync();
        }

        return Ok(cart);
      }

      return Unauthorized();
    }

    [HttpPost("CreatePayment")]
    public async Task<IActionResult> CreatePayment()
    {
      string sessionId = HttpContext.Request.Headers["SessionId"].FirstOrDefault();
      if (User.Identity.IsAuthenticated)
      {
        return Ok();

      }
      else if (!string.IsNullOrEmpty(sessionId))
      {
        return Ok();
      }
      return Unauthorized();
    }
  }

  public class AddToCartParams
  {
    public int ProductId { get; set; }
    public int Quantity { get; set; }
  }

  public class ChangeQuantityParams
  {
    public int ProductId { get; set; }
    public int Quantity { get; set; }
  }

  public class PaymentParams
  {

  }
}