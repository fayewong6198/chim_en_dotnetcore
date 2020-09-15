using System;
namespace Chim_En_DOTNET.Helpers
{
  public class PaginationFilter
  {
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public PaginationFilter()
    {
      this.PageNumber = 1;
      this.PageSize = 10;
    }
    public PaginationFilter(int pageNumber, int pageSize)
    {

      this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
      this.PageSize = pageSize > 1000 ? 1000 : pageSize;
    }
  }

  public class ProductFilter : PaginationFilter
  {
    public int CategoryId { get; set; }
    public string Search { get; set; }
    public string OrderBy { get; set; }
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


    public ProductFilter(int pageNumber, int pageSize, int categoryId, string search, string orderBy, bool? active, int available) : base(pageNumber, pageSize)
    {
      this.CategoryId = categoryId <= -1 ? -1 : categoryId;
      this.Search = search;
      this.OrderBy = orderBy;
      this.Active = active;
      this.Available = Available;
    }
  }
}