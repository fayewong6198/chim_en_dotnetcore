namespace Chim_En_DOTNET.Models
{
  public class PaymentProductDetail
  {
    public int Id { get; set; }
    public int PaymentId { get; set; }

    public Payment Payment { get; set; }

    public int ProductId { get; set; }

    public string ProductName { get; set; }

    public int ProductQuantity { get; set; }
    public double ProductAmount { get; set; }
    public int ProductPrice { get; set; }
    public double ProductPromotion { get; set; }

    public static double GetTotalPrice(PaymentProductDetail paymentProductDetail)
    {
      return paymentProductDetail.ProductPrice * paymentProductDetail.ProductAmount * paymentProductDetail.ProductPromotion;
    }
  }
}