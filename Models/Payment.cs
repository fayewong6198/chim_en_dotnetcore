using System;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;


namespace Chim_En_DOTNET.Models
{
  public class Payment
  {
    public int Id { get; set; }
    public string ApplicationUserId { get; set; }

    public ApplicationUser User { get; set; }
    public string DeviceId { get; set; }

    public PaymentMethodChoice PaymentMethod { get; set; }

    public int Amount { get; set; }

    public int ShipFee { get; set; }

    public string Note { get; set; }

    public ICollection<PaymentProductDetail> PaymentProductDetails { get; set; }
    public ICollection<PaymentUserDetail> PaymentUserDetails { get; set; }
    public StatusChoice Status { get; set; }
    public int Total { get; set; }

    public DateTime CreatedAt { get; set; }

    public Payment()
    {
      this.Status = StatusChoice.Pending;
      this.CreatedAt = DateTime.Now;
      this.ShipFee = 0;
      this.Note = "";
      this.Amount = 0;
    }
  }
  public enum PaymentMethodChoice : int
  {
    COD = 0,
    Paypal = 1
  }

  public enum StatusChoice : int
  {
    Pending = 0,
    Processing = 1,
    Complete = 2,
    Cancel = 3,
    Paid = 4
  }
}