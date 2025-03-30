using Hospital.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Hospital.RegistrarWindow
{
	/// <summary>
	/// Логика взаимодействия для AddPatientWindow.xaml
	/// </summary>
	public partial class AddPatientWindow : Window
	{
		private БольницаEntities _db;

		public AddPatientWindow(БольницаEntities db)
		{
			InitializeComponent();
			_db = db;

			// Дата по умолчанию
			BirthDatePicker.SelectedDate = DateTime.Now.AddYears(-30);
			GenderComboBox.SelectedIndex = 0;
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			// Валидация данных
			if (string.IsNullOrWhiteSpace(LastNameTextBox.Text) ||
				string.IsNullOrWhiteSpace(FirstNameTextBox.Text) ||
				BirthDatePicker.SelectedDate == null)
			{
				MessageBox.Show("Фамилия, имя и дата рождения обязательны для заполнения!",
					"Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			try
			{
				// Создание нового пациента
				var newPatient = new Пациент
				{
					Фамилия = LastNameTextBox.Text,
					Имя = FirstNameTextBox.Text,
					Отчество = MiddleNameTextBox.Text,
					ДатаРождения = BirthDatePicker.SelectedDate,
					Пол = ((ComboBoxItem)GenderComboBox.SelectedItem).Content.ToString(),
					Телефон = PhoneTextBox.Text,
					СНИЛС = SnilsTextBox.Text
				};

				// Добавление в базу данных
				_db.Пациент.Add(newPatient);
				_db.SaveChanges();

				// Закрытие окна с положительным результатом
				DialogResult = true;
				Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при сохранении пациента: {ex.Message}",
					"Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
			Close();
		}
	}
}
