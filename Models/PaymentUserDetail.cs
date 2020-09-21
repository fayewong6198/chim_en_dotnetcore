namespace Chim_En_DOTNET.Models
{
  public class PaymentUserDetail
  {
    public int Id { get; set; }
    public int PaymentId { get; set; }

    public Payment Payment { get; set; }
    public string UserId { get; set; }

    public string FullName { get; set; }
    public string Email { get; set; }
    public string Mobile { get; set; }
    public string City { get; set; }
    public string Disrtict { get; set; }
    public string Address { get; set; }

  }
}