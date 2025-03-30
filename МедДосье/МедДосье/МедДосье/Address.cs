using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace МедДосье
{
	// Адрес
	[Table("Адрес")]
	public class Address
	{
		[PrimaryKey, AutoIncrement]
		public int Номер { get; set; }

		public string Субъект { get; set; }
		public string Район { get; set; }
		public string Город { get; set; }
		public string НаселенныйПункт { get; set; }
		public string Улица { get; set; }
		public string Дом { get; set; }
		public string Квартира { get; set; }
		public string Местность { get; set; }
	}
}
