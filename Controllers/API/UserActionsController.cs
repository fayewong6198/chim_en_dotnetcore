using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Chim_En_DOTNET.Data;
using Chim_En_DOTNET.Models;


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
    public async Task<IActionResult> AddToCart()
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

}