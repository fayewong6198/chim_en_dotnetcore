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

    [HttpPost("AddToCart")]
    public async Task<IActionResult> AddToCart(AddToCartParams addToCartParams)
    {
      Console.WriteLine(User.Identity.IsAuthenticated);

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
        cartItem = new CartItem();
        cartItem.CartId = cart.Id;
        cartItem.ProductId = addToCartParams.ProductId;
        cartItem.Quantity = addToCartParams.Quantity;
        _context.CartItems.Add(cartItem);
      }

      else
      {
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

    [HttpPost("ChangeQuantity")]
    public async Task<IActionResult> ChangeQuantity()
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

    [HttpPost("DeleteFromCart/{productId}")]
    public async Task<IActionResult> DeleteFromCart(string productId)
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