namespace Chim_En_DOTNET.Models
{
  public class Address
  {
    public int Id { get; set; }

    public string ApplicationUserId { get; set; }
    public string FullName { get; set; }
    public string Company { get; set; }
    public string PhoneNumber { get; set; }

    public string AddressString { get; set; }

    public int CityId { get; set; }

    public int DistrictId { get; set; }

    public int WardId { get; set; }
    public City City { get; set; }

    public District District { get; set; }
    public Ward Ward { get; set; }

    public ApplicationUser ApplicationUser { get; set; }

  }
}