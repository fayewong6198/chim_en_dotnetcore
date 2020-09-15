namespace Chim_En_DOTNET.Models
{
  public class UserPermision
  {
    public int Id { get; set; }
    public string ApplicationUserId { get; set; }

    public string Permision { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
  }
}