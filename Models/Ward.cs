namespace Chim_En_DOTNET.Models
{
  public class Ward
  {
    public int Id { get; set; }

    public int DistrictId { get; set; }

    public int ShipFee { get; set; }
    public District District { get; set; }

    public Ward()
    {
      this.ShipFee = 25000;
    }
  }
}