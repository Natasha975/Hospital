using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace МедДосье
{
	// Документ
	[Table("Документ")]
	public class Document
	{
		[PrimaryKey, AutoIncrement]
		public int НомерЗаписи { get; set; }

		public string НаименованиеДокумента { get; set; }
		public string Серия { get; set; }
		public string Номер { get; set; }
		public string КемВыдан { get; set; }
		public DateTime ДатаВыдачи { get; set; }
	}
}
