﻿using SQLite;

namespace MyMusic.Player.Storage.Models
{
	public sealed class Artists
	{
		[PrimaryKey]
		public int Serial { get; set; }
		public string Name { get; set; }
	}
}