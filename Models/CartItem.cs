using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chim_En_DOTNET.Models
{
  public class CartItem
  {
    public int Id { get; set; }
    public int CartId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public Cart Cart { get; set; }
    public Product Product { get; set; }

    public int GetTotalItemPrice()
    {
      return this.Quantity * this.Product.GetPrice();
    }

    public CartItem()
    {
      this.Quantity = 1;
    }
  }
}