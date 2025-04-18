using System;
using System.Collections.ObjectModel;
using System.Text;
using static МедДосье.DoctorListPage;
using Xamarin.Forms;
using МедДосье.Model;
using System.Windows.Input;

namespace МедДосье.ViewModel
{
	public class DoctorListViewModel : BindableObject
	{
		public ICommand SelectDateCommand { get; }
		public ContentPage Page { get; }
		public ICommand SelectTimeCommand { get; }
		private Doctor SelectedDoctor { get; set; }

		public DoctorListViewModel(Speciality speciality, ContentPage page)
		{
			Page = page;
			SpecialityName = speciality.Name;
			SelectDateCommand = new Command<Doctor>(OnDateSelected);
			SelectTimeCommand = new Command<TimeSlot>(OnTimeSelected);

			// Инициализация списка врачей
			if (speciality.Name == "Терапевт")
			{
				Doctors = new ObservableCollection<Doctor>
				{
					new Doctor { Name = "Андреева Карина Владимировна", Image = "noPreview.jpg",  Price="2299", Description = "Медицинский работник" },
					new Doctor { Name = "Мулюков Тахир Рахматуллаевич", Image = "doctor.jpg",  Price="2299",  Description = "Медицинский работник" }
				};
			}
			else if (speciality.Name == "Невролог")
			{
				Doctors = new ObservableCollection<Doctor>
				{
					new Doctor { Name = "Козлов Артем Сергеевич", Image = "noPreview.jpg", Price="2799", Description = "Врач-невролог высшей категории" },
					new Doctor { Name = "Белова Ольга Игоревна", Image = "doctor1.jpg", Price="3200", Description = "Детский невролог, кандидат медицинских наук" }
				};
			}
			else if (speciality.Name == "Хирург")
			{
				Doctors = new ObservableCollection<Doctor>
				{
					new Doctor { Name = "Петров Дмитрий Александрович", Image = "noPreview.jpg", Price="4500", Description = "Хирург общей практики" },
					new Doctor { Name = "Смирнова Елена Викторовна", Image = "noPreview.jpg", Price="5200", Description = "Кардиохирург, доктор медицинских наук" }
				};
			}
			else if (speciality.Name == "Педиатр")
			{
				Doctors = new ObservableCollection<Doctor>
				{
					new Doctor { Name = "Иванова Галина Михайловна", Image = "noPreview.jpg", Price="2500", Description = "Педиатр с 20-летним стажем" },
					new Doctor { Name = "Фролов Сергей Павлович", Image = "noPreview.jpg", Price="2300", Description = "Детский врач первой категории" }
				};
			}
			else if (speciality.Name == "Кардиолог")
			{
				Doctors = new ObservableCollection<Doctor>
				{
					new Doctor { Name = "Волков Андрей Николаевич", Image = "noPreview.jpg", Price="3800", Description = "Кардиолог-аритмолог" },
					new Doctor { Name = "Громова Ирина Олеговна", Image = "noPreview.jpg", Price="4200", Description = "Врач функциональной диагностики" }
				};
			}
			else if (speciality.Name == "Гастроэнтеролог")
			{
				Doctors = new ObservableCollection<Doctor>
				{
					new Doctor { Name = "Соколова Марина Евгеньевна", Image = "noPreview.jpg", Price="3500", Description = "Специалист по заболеваниям ЖКТ" },
					new Doctor { Name = "Никитин Алексей Дмитриевич", Image = "noPreview.jpg", Price="3100", Description = "Эндоскопист-гастроэнтеролог" }
				};
			}
			else
			{
				Doctors = new ObservableCollection<Doctor>();
			}

			// Обновляем тексты кнопок для каждого врача
			foreach (var doctor in Doctors)
			{
				doctor.UpdateDateButtons();
			}
		}

		private void OnDateSelected(Doctor doctor)
		{
			SelectedDoctor = doctor;

			// Если уже выбрана эта же дата - сбрасываем выбор
			if (doctor.SelectedDate.HasValue &&
				((doctor.TodayText.Contains("Сегодня") && doctor.SelectedDate.Value.Date == DateTime.Today) ||
				(doctor.TomorrowText.Contains("Завтра") && doctor.SelectedDate.Value.Date == DateTime.Today.AddDays(1))))
			{
				doctor.SelectedDate = null;
				return;
			}

			// Устанавливаем выбранную дату для врача
			var today = DateTime.Today;
			if (doctor.TodayText.Contains("Сегодня"))
				doctor.SelectedDate = today;
			else if (doctor.TomorrowText.Contains("Завтра"))
				doctor.SelectedDate = today.AddDays(1);
			else if (doctor.DayAfterTomorrowText.Contains("Послезавтра"))
				doctor.SelectedDate = today.AddDays(2);
			else
				doctor.SelectedDate = today.AddDays(3);
		}

		private async void OnTimeSelected(TimeSlot timeSlot)
		{
			if (SelectedDoctor == null || SelectedDoctor.SelectedDate == null || timeSlot == null)
				return;

			var appointmentDate = SelectedDoctor.SelectedDate.Value.Date + TimeSpan.Parse(timeSlot.Time);
			await Page.DisplayAlert("Запись",
				$"Вы записаны к {SelectedDoctor.Name} на {appointmentDate:dd.MM.yyyy} в {timeSlot.Time}", "OK");
		}

		public string SpecialityName { get; set; }
		public ObservableCollection<Doctor> Doctors { get; set; }
	}
}