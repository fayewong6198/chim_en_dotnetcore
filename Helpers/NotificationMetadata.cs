using Microsoft.Extensions.DependencyInjection;

namespace Chim_En_DOTNET.Helpers
{
  public class NotificationMetadata
  {
    public string Sender { get; set; }
    public string Reciever { get; set; }
    public string SmtpServer { get; set; }
    public int Port { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }

  }
}