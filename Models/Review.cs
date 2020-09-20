using System;
using System.Collections.Generic;

namespace Chim_En_DOTNET.Models
{
  public class Review
  {
    public int Id { get; set; }
    public string ApplicationUserId { get; set; }
    public string DeviceId {get;set;}
    public string FullName { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }
    public int Rating { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ApplicationUser ApplicationUser { get; set; }

    public int ProductId { get; set; }

    public Product Product { get; set; }

    public ICollection<Reply> Replies { get; set; }

    public Review()
    {
      this.CreatedAt = DateTime.Now;
    }

  }
}