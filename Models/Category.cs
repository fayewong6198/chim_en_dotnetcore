using System;
using System.Collections.Generic;

namespace Chim_En_DOTNET.Models
{
  public class Category
  {
    public int Id { get; set; }
    public string title { get; set; }
    public ICollection<Product> Products { get; set; }
    public DateTime CreatedAt { get; set; }
    public Category()
    {
      this.CreatedAt = DateTime.Now;
    }
  }
}