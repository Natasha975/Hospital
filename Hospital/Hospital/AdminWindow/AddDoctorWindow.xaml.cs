using Hospital.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Hospital.AdminWindow
{
	/// <summary>
	/// Логика взаимодействия для AddDoctorWindow.xaml
	/// </summary>
	public partial class AddDoctorWindow : Window
	{
		private БольницаEntities db;
		// Список всех возможных специализаций врачей
		private List<string> specializations = new List<string>
		{
			"Аллерголог",
			"Аллерголог-иммунолог",
			"Ангионевролог",
			"Ангиохирург",
			"Андролог",
			"Андролог-эндокринолог",
			"Анестезиолог",
			"Апитерапевт",
			"Аритмолог",
			"Ароматерапевт",
			"Артролог",
			"Бактериолог",
			"Бальнеолог",
			"Валеолог",
			"Венеролог",
			"Вертебролог",
			"Вирусолог",
			"Врач по спортивной медицине",
			"Врач скорой помощи",
			"Врач ультразвуковой диагностики",
			"Врач функциональной диагностики",
			"Врач ЭКО",
			"Врач-эпилептолог",
			"Гастроэнтеролог",
			"Гематолог",
			"Генетик",
			"Гепатолог",
			"Гериатр (геронтолог)",
			"Гинеколог",
			"Гинеколог-онколог",
			"Гинеколог-перинатолог",
			"Гинеколог-эндокринолог",
			"Гирудотерапевт",
			"Гистолог",
			"Гомеопат",
			"Дерматовенеролог",
			"Дерматолог",
			"Детский гинеколог",
			"Детский невропатолог",
			"Детский хирург",
			"Диабетолог",
			"Диетолог",
			"Иглорефлексотерапевт",
			"Иммунолог",
			"Имплантолог",
			"Инфекционист",
			"Кардиолог",
			"Кардиохирург",
			"Кинезиолог",
			"КЛД (лаборант)",
			"Комбустиолог",
			"Косметолог-дерматолог",
			"Курортолог",
			"Логопед",
			"ЛФК-врач",
			"Маммолог",
			"Миколог",
			"Микрохирург",
			"Нарколог",
			"Невролог",
			"Натуротерапевт",
			"Невропатолог",
			"Нейрохирург",
			"Неонатолог",
			"Нефролог",
			"Окулист",
			"Онколог",
			"Онкоуролог",
			"Ортопед",
			"Остеопат",
			"Оториноларинголог",
			"Офтальмолог",
			"Паразитолог",
			"Пародонтолог",
			"Педиатр",
			"Пластический хирург",
			"Подолог",
			"Проктолог (колопроктолог)",
			"Профпатолог",
			"Психиатр",
			"Психиатр-нарколог",
			"Психоаналитик",
			"Психолог",
			"Психоневролог",
			"Психотерапевт",
			"Пульмонолог",
			"Радиолог",
			"Реабилитолог",
			"Реаниматолог",
			"Ревматолог",
			"Рентгенолог",
			"Репродуктолог",
			"Рефлексотерапевт",
			"Сексолог",
			"Сексопатолог",
			"Семейный врач",
			"Сомнолог",
			"Стоматолог",
			"Стоматолог-имплантолог",
			"Стоматолог-ортодонт",
			"Стоматолог-ортопед",
			"Стоматолог-терапевт",
			"Стоматолог-хирург",
			"Суггестолог",
			"Судебно-медицинский эксперт",
			"Сурдолог",
			"Терапевт",
			"Терапевт женской консультации",
			"Токсиколог",
			"Торакальный хирург",
			"Травматолог",
			"Трансплантолог",
			"Трансфузиолог",
			"Трихолог",
			"Уролог",
			"Фармаколог клинический",
			"Физиотерапевт",
			"Фитотерапевт",
			"Флеболог",
			"Фониатр",
			"Фтизиатр",
			"Химиотерапевт",
			"Хирург",
			"Челюстно-лицевой хирург",
			"Эмбриолог",
			"Эметолог",
			"Эндокринолог",
			"Эндоскопист",
			"Эпилептолог"
		};

		public AddDoctorWindow(БольницаEntities _db)
		{
			InitializeComponent();
			db = _db;
			SpecializationComboBox.ItemsSource = specializations;
		}


		// Обработчик ввода текста для поля телефона
		private void PhoneTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			if (!char.IsDigit(e.Text, e.Text.Length - 1))
			{
				e.Handled = true;
			}
		}

		// Обработчик вставки текста в поле телефона
		private void PhoneTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
		{
			if (e.DataObject.GetDataPresent(typeof(string)))
			{
				string text = (string)e.DataObject.GetData(typeof(string));
				if (!text.All(char.IsDigit))
				{
					// Отменяем вставку, если есть не-цифры
					e.CancelCommand();
				}
			}
			else
			{
				e.CancelCommand();
			}
		}

		// Обработчик нажатия кнопки "Сохранить"
		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			// Проверка обязательных полей
			if (string.IsNullOrWhiteSpace(LastNameTextBox.Text) 
				|| string.IsNullOrWhiteSpace(FirstNameTextBox.Text))
			{
				MessageBox.Show("Фамилия и имя обязательны для заполнения!", 
								"Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (string.IsNullOrWhiteSpace(SpecializationComboBox.Text))
			{
				MessageBox.Show("Выберите специализацию!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			// Создание нового объекта врача
			var newDoctor = new Врач
			{
				Фамилия = LastNameTextBox.Text,
				Имя = FirstNameTextBox.Text,
				Отчество = MiddleNameTextBox.Text,
				Специализация = SpecializationComboBox.Text,
				ВнутреннийТелефон = PhoneTextBox.Text
			};

			try
			{
				// Сохранение в базу данных
				db.Врач.Add(newDoctor);
				db.SaveChanges();
				DialogResult = true;
				Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		// Обработчик нажатия кнопки "Отмена"
		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
			Close();
		}
	}
}