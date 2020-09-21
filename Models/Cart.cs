using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Chim_En_DOTNET.Models
{
  public class Cart
  {
    public int Id { get; set; }

    public string SessionId { get; set; }
    public string ApplicationUserId { get; set; }
    public ApplicationUser User { get; set; }

    public DateTime UpdatedAt { get; set; }
    public ICollection<CartItem> CartItems { get; set; }

    public Cart()
    {
      this.UpdatedAt = DateTime.Now;
    }

    public static int GetTotalPrice(Cart cart)
    {
      int price = 0;

      foreach (var cartItem in cart.CartItems)
      {
        price += CartItem.GetTotalItemPrice(cartItem);
      }

      return price;
    }
  }
}