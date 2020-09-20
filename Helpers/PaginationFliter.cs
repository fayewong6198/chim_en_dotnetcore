using System;
using Chim_En_DOTNET.Models;
namespace Chim_En_DOTNET.Helpers
{
  public class PaginationFilter
  {
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string Search { get; set; }
    public string OrderBy { get; set; }
    public PaginationFilter()
    {
      this.PageNumber = 1;
      this.PageSize = 10;
      this.Search = "";
      this.OrderBy = "";
    }
    public PaginationFilter(int pageNumber, int pageSize, string search, string orderBy)
    {
      this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
      this.PageSize = pageSize > 1000 ? 1000 : pageSize;
      this.Search = search;
      this.OrderBy = orderBy;
    }
  }

  public class ProductFilter : PaginationFilter
  {
    public int CategoryId { get; set; }

    public bool? Active { get; set; }
    public int Available { get; set; }
    public ProductFilter() : base()
    {
      this.CategoryId = -1;
      this.Search = "";
      this.OrderBy = "";
      this.Active = null;
      this.Available = 0;
    }

    public ProductFilter(int pageNumber, int pageSize, int categoryId, string search, string orderBy, bool? active, int available) : base(pageNumber, pageSize, search, orderBy)
    {
      this.CategoryId = categoryId <= -1 ? -1 : categoryId;
      this.Active = active;
      this.Available = Available;
    }
  }

  public class CategoryFilter : PaginationFilter
  {
    public string Title { get; set; }
    public CategoryFilter() : base()
    {
      this.Title = "";
    }

    public CategoryFilter(int pageNumber, int pageSize, string search, string orderBy, string title) : base(pageNumber, pageSize, search, orderBy)
    {
      this.Title = title;
    }
  }

  public class UserFilter : PaginationFilter
  {
    public bool? IsSuperUser { get; set; }
    public bool? IsStaff { get; set; }

    public UserFilter() : base()
    {
      this.IsSuperUser = null;
      this.IsStaff = null;
    }

    public UserFilter(int pageNumber, int pageSize, string search, string orderBy, bool? isSuperUser, bool? isStaff) : base(pageNumber, pageSize, search, orderBy)
    {
      this.IsSuperUser = isSuperUser;
      this.IsStaff = isStaff;

    }
  }

  public class PaymentFilter : PaginationFilter
  {

    public string UserId { get; set; }
    public string DeviceId { get; set; }
    public PaymentMethodChoice? PaymentMethod { get; set; }
    public StatusChoice? Status { get; set; }
    public int? Total { get; set; }
    public DateTime? CreatedAt { get; set; }

    public PaymentFilter() : base()
    {
      this.UserId = "";
      this.DeviceId = "";
      this.PaymentMethod = null;
      this.Status = null;
      this.Total = null;
      this.CreatedAt = null;
    }

    public PaymentFilter(int pageNumber, int pageSize, string search, string orderBy, string userId, string deviceId, PaymentMethodChoice? paymentMethod, StatusChoice? status, int? total, DateTime? createdAt) : base(pageNumber, pageSize, search, orderBy)
    {
      UserId = userId;
      DeviceId = deviceId;
      PaymentMethod = paymentMethod;
      Status = status;
      Total = total;
      CreatedAt = createdAt;
    }

  }

  public class ReviewFilter : PaginationFilter
  {
    public string ApplicationUserId { get; set; }
    public int? Rating { get; set; }
    public int? ProductId { get; set; }

    public ReviewFilter() : base()
    {
      this.ApplicationUserId = "";
      this.Rating = null;
    }

    public ReviewFilter(int pageNumber, int pageSize, string search, string orderBy, string applicationUserId, int? rating, int? productId) : base(pageNumber, pageSize, search, orderBy)
    {
      ApplicationUserId = applicationUserId;
      Rating = rating;
      this.ProductId = productId;
    }
  }

  public class CartFilter : PaginationFilter
  {

  }
}