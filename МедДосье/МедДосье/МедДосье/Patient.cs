using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace МедДосье
{
	[Table("Patient")]
	public class Patient
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		[NotNull]
		public string LastName { get; set; } // Фамилия

		[NotNull]
		public string FirstName { get; set; } // Имя

		public string MiddleName { get; set; } // Отчество
		[NotNull]
		public string Login { get; set; } // Логин
		[NotNull]
		public string Password { get; set; } // Пароль
		[NotNull]
		public string DocumentName { get; set; } // НаименованиеДокумента
		[NotNull]
		public string Series { get; set; }
		[NotNull]
		public string Number { get; set; }
		[NotNull]
		public string IssuedBy { get; set; }
		public DateTime OfTheIssueDate  { get; set; }
		public string Gender { get; set; } // Пол
		public string SNILS { get; set; } // СНИЛС
		public string INN { get; set; } // ИНН
		public int? AddressId { get; set; } // Адрес
		public string Phone { get; set; } // Телефон
		public int? InsuranceId { get; set; } // НомерСтраховки
		public string MaritalStatus { get; set; } // СемейноеПоложение
		public string Education { get; set; } // Образование
		public string Employment { get; set; } // Занятость
		public int? WorkId { get; set; } // Работа
	}
}
