using System;

namespace Chim_En_DOTNET.Helpers
{
  public class PagedResponse<T> : Response<T>
  {
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int FirstPage { get; set; }
    public int LastPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
    public int NextPage { get; set; }
    public int PreviousPage { get; set; }
    public PagedResponse(T data, int pageNumber, int pageSize, int totalRecords)
    {
      this.PageNumber = pageNumber;
      this.PageSize = pageSize;
      this.Data = data;
      this.Message = null;
      this.Succeeded = true;
      this.Errors = null;
      this.FirstPage = 1;
      this.TotalRecords = totalRecords;
      this.LastPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(this.TotalRecords) / Convert.ToDouble(this.PageSize)));
      this.PreviousPage = this.PageNumber <= 1 ? 1 : this.PageNumber - 1;
      this.NextPage = this.PageNumber == this.LastPage ? this.LastPage : this.PageNumber + 1;
    }
  }


  public class ProductsFilterResponse<T> : PagedResponse<T>
  {
    public int CategoryId { get; set; }
    public ProductsFilterResponse(T data, int pageNumber, int pageSize, int totalRecords, int categoryId) : base(data, pageNumber, pageSize, totalRecords)
    {
      this.CategoryId = categoryId;
    }
  }
}