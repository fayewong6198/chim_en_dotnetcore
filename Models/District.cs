using System.Collections.Generic;

namespace Chim_En_DOTNET.Models
{
  public class District
  {
    public int Id { get; set; }
    public string Name { get; set; }

    public int CityId { get; set; }
    public City City { get; set; }

    public int ShipFee { get; set; }

    public ICollection<Ward> Wards { get; set; }
    public District()
    {
      this.ShipFee = 25000;
    }
  }
}