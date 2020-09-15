using System;

namespace Chim_En_DOTNET.Models
{
  public class Reply
  {
    public int Id { get; set; }
    public int ReviewId { get; set; }

    public string FullName { get; set; }
    public Review Review { get; set; }

    public string Content { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public string ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
    public Reply()
    {
      this.CreatedAt = DateTime.Now;
    }
  }
}