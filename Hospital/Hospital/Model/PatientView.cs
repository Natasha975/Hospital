using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Model
{

	public class PatientView
	{
		public int НомерПациента { get; set; }
		public string Фамилия { get; set; }
		public string Имя { get; set; }
		public string Отчество { get; set; }
		public DateTime? ДатаРождения { get; set; }
		public string Пол { get; set; }
		public string СНИЛС { get; set; }
		public string Телефон { get; set; }
		public string ОМС { get; set; }

		// Добавляем свойство для возраста
		public int? Возраст
		{
			get
			{
				if (ДатаРождения.HasValue)
				{
					var today = DateTime.Today;
					var age = today.Year - ДатаРождения.Value.Year;
					if (ДатаРождения.Value.Date > today.AddYears(-age)) age--;
					return age;
				}
				return null;
			}
		}
	}
}
