namespace Chim_En_DOTNET.Models
{
  public class PaymentProductDetail
  {
    public int Id { get; set; }
    public int PaymentId { get; set; }

    public Payment Payment { get; set; }

    public int ProductId { get; set; }

    public string ProductName { get; set; }
    public int ProductAmount { get; set; }
    public int ProductPrice { get; set; }
    public int ProductPromotion { get; set; }

    public int GetTotalPrice()
    {
      return this.ProductPrice * this.ProductAmount * this.ProductPromotion;
    }
  }
}