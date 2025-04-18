using System;
using System.Collections.Generic;
using System.Text;

namespace МедДосье.Model
{
	public class TimeSlot
	{
		public string Time { get; set; }
		public bool IsAvailable { get; set; } = true;
	}
}
