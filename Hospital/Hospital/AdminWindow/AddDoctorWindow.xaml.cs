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

namespace Hospital.AdminWindow
{
	/// <summary>
	/// Логика взаимодействия для AddDoctorWindow.xaml
	/// </summary>
	public partial class AddDoctorWindow : Window
	{
		private БольницаEntities _db;

		public AddDoctorWindow(БольницаEntities db)
		{
			InitializeComponent();
			_db = db;
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(LastNameTextBox.Text) ||
				string.IsNullOrWhiteSpace(FirstNameTextBox.Text))
			{
				MessageBox.Show("Фамилия и имя обязательны для заполнения!", "Ошибка",
					MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			var newDoctor = new Врач
			{
				Фамилия = LastNameTextBox.Text,
				Имя = FirstNameTextBox.Text,
				Отчество = MiddleNameTextBox.Text,
				Специализация = SpecializationTextBox.Text,
				ВнутреннийТелефон = PhoneTextBox.Text
			};

			_db.Врач.Add(newDoctor);
			_db.SaveChanges();

			DialogResult = true;
			Close();
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
			Close();
		}
	}
}