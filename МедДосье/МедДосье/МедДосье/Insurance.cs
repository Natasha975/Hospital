using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace МедДосье
{
	// Страховка
	[Table("Страховка")]
	public class Insurance
	{
		[PrimaryKey, AutoIncrement]
		public int НомерЗаписи { get; set; }

		public string СерияПолиса { get; set; }
		public string НомерПолиса { get; set; }
		public string НаименованиеСтраховойКомпании { get; set; }
		public DateTime? СрокДействия { get; set; }
	}
}
