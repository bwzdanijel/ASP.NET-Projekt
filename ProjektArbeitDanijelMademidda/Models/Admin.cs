﻿namespace ProjektArbeitDanijelMademidda.Models
{
	public class Admin
	{
		public int Id { get; set; }
		public string? Username { get; set; }
		public string? Password { get; set; }
		public string? Salt { get; set; }

		//ip
		public string? UserIpAddress { get; set; }
	}
}
