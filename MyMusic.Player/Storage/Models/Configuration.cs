﻿using SQLite;

namespace MyMusic.Player.Storage.Models
{
  public sealed class Configuration
  {
    [PrimaryKey]
    public int Serial { get; set; }
		public string Name { get; set; }
    public string CloudUrl { get; set; }
    public string CloudPassword { get; set; }
		public bool IsSelected { get; set; }
  }
}