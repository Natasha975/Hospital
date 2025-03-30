using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace МедДосье
{
	// Работа
	[Table("Работа")]
	public class Job
	{
		[PrimaryKey, AutoIncrement]
		public int НомерЗаписи { get; set; }

		public string МестоРаботы { get; set; }
		public string Должность { get; set; }
	}
}
