using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Model
{
	/// <summary>
	/// Класс для представления данных о враче с поддержкой уведомлений об изменениях свойств
	/// </summary>
	public class DoctorView : INotifyPropertyChanged
	{
		private БольницаEntities _db;
		private Врач _doctor;

		public DoctorView(Врач doctor, БольницаEntities db)
		{
			_doctor = doctor;
			_db = db;
		}

		public int НомерВрача => _doctor.НомерВрача;

		public string Фамилия
		{
			get => _doctor.Фамилия;
			set
			{
				if (_doctor.Фамилия != value)
				{
					_doctor.Фамилия = value;
					OnPropertyChanged();
				}
			}
		}

		public string Имя
		{
			get => _doctor.Имя;
			set
			{
				if (_doctor.Имя != value)
				{
					_doctor.Имя = value;
					OnPropertyChanged();
				}
			}
		}

		public string Отчество
		{
			get => _doctor.Отчество;
			set
			{
				if (_doctor.Отчество != value)
				{
					_doctor.Отчество = value;
					OnPropertyChanged();
				}
			}
		}

		public string Специализация
		{
			get => _doctor.Специализация;
			set
			{
				if (_doctor.Специализация != value)
				{
					_doctor.Специализация = value;
					OnPropertyChanged();
				}
			}
		}

		public string ВнутреннийТелефон
		{
			get => _doctor.ВнутреннийТелефон;
			set
			{
				if (_doctor.ВнутреннийТелефон != value)
				{
					_doctor.ВнутреннийТелефон = value;
					OnPropertyChanged();
				}
			}
		}

		public bool IsUsed { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
