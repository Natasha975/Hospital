using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace МедДосье
{
	// Инвалидность
	[Table("Инвалидность")]
	public class Disability
	{
		[PrimaryKey, AutoIncrement]
		public int НомерЗаписи { get; set; }

		public int? НомерПациента { get; set; }
		public string Группа { get; set; }
		public string Описание { get; set; }
		public DateTime? Дата { get; set; }
	}
}
