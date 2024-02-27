using Microsoft.AspNetCore.Components;
using MyMusic.Player.Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.Player.Blazor.Components
{
  public partial class MusicComponent : ComponentBase
  {
    [Parameter]
    public MusicReference Model { get; set; }
  }
}