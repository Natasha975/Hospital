using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace МедДосье.Model
{
	public class Doctor : BindableObject
	{
		public string Name { get; set; }
		public string Image { get; set; }
		public string Price { get; set; }
		public string Description { get; set; }

		private string _todayText;
		public string TodayText
		{
			get => _todayText;
			set { _todayText = value; OnPropertyChanged(); }
		}

		private string _tomorrowText;
		public string TomorrowText
		{
			get => _tomorrowText;
			set { _tomorrowText = value; OnPropertyChanged(); }
		}

		private string _dayAfterTomorrowText;
		public string DayAfterTomorrowText
		{
			get => _dayAfterTomorrowText;
			set { _dayAfterTomorrowText = value; OnPropertyChanged(); }
		}

		private string _inThreeDaysText;
		public string InThreeDaysText
		{
			get => _inThreeDaysText;
			set { _inThreeDaysText = value; OnPropertyChanged(); }
		}

		private DateTime? _selectedDate;
		public DateTime? SelectedDate
		{
			get => _selectedDate;
			set
			{
				_selectedDate = value;
				OnPropertyChanged();
				UpdateTimeSlots();
			}
		}

		private ObservableCollection<TimeSlot> _timeSlots;
		public ObservableCollection<TimeSlot> TimeSlots
		{
			get => _timeSlots;
			set
			{
				_timeSlots = value;
				OnPropertyChanged();
			}
		}

		public void UpdateDateButtons()
		{
			var today = DateTime.Today;

			TodayText = "Сегодня\n" + today.ToString("dd.MM");
			TomorrowText = "Завтра\n" + today.AddDays(1).ToString("dd.MM");
			DayAfterTomorrowText = "Послезавтра\n" + today.AddDays(2).ToString("dd.MM");
			InThreeDaysText = today.AddDays(3).ToString("dd.MM");
		}

		private void UpdateTimeSlots()
		{
			if (!SelectedDate.HasValue)
			{
				TimeSlots = null;
				return;
			}

			// Генерируем временные слоты (например, с 9:00 до 18:00 с интервалом в 30 минут)
			var slots = new ObservableCollection<TimeSlot>();
			var startTime = new TimeSpan(9, 0, 0);
			var endTime = new TimeSpan(18, 0, 0);
			var interval = TimeSpan.FromMinutes(30);

			for (var time = startTime; time <= endTime; time = time.Add(interval))
			{
				slots.Add(new TimeSlot { Time = time.ToString(@"hh\:mm") });
			}

			TimeSlots = slots;
		}
	}
}