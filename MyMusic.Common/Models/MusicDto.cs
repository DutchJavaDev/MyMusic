using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.Common.Models
{
  public class MusicDto
  {
    public int Serial { get; set; }
    public string Name { get; set; }
    public Guid TrackingId { get; set; }
  }
}
