using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chim_En_DOTNET.Models
{
  public class Product
  {
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string Sku { get; set; }

    [Required]
    public string Title { get; set; }
    public string Slug { get; set; }
    public string Description { get; set; }
    public string FullDescription { get; set; }
    public int Price { get; set; }

    public ICollection<ProductImage> Images { get; set; }

    public double Promotion { get; set; }

    public int Available { get; set; }

    public bool Active { get; set; }

    public Category Category { get; set; }
    public DateTime CreatedAt { get; set; }
    public Product()
    {
      this.CreatedAt = DateTime.Now;
      this.Available = 0;
      this.Active = false;
      this.Promotion = 0;
      if (this.Title != null)
      {
        this.Slug = this.Title;
      }
    }

    public int GetPrice()
    {
      return Convert.ToInt32(this.Price * this.Promotion);
    }
  }
}