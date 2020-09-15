
using System;
using System.Collections.Generic;

namespace Chim_En_DOTNET.Models
{
  public class City
  {
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<District> Districts { get; set; }
  }
}