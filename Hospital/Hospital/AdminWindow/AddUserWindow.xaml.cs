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
	/// Логика взаимодействия для AddUserWindow.xaml
	/// </summary>
	public partial class AddUserWindow : Window
	{
		private БольницаEntities _db;

		public AddUserWindow(БольницаEntities db)
		{
			InitializeComponent();
			_db = db;
			LoadRoles();
			LoadDoctors();

			IsDoctorCheckBox.Checked += IsDoctorCheckBox_Checked;
			IsDoctorCheckBox.Unchecked += IsDoctorCheckBox_Unchecked;
		}

		private void LoadRoles()
		{
			RoleComboBox.ItemsSource = _db.Роль.ToList();
			RoleComboBox.DisplayMemberPath = "Ниаменование";
			RoleComboBox.SelectedValuePath = "НомерЗаписи";
		}

		private void LoadDoctors()
		{

			var doctors = _db.Врач.ToList();
			DoctorComboBox.ItemsSource = doctors
				.Select(d => new
				{
					d.НомерВрача,
					d.Фамилия,
					d.Имя,
					d.Отчество,
					d.Специализация,
					FullInfo = $"{d.Фамилия} {d.Имя} {d.Отчество} ({d.Специализация})"
				})
				.ToList();

			DoctorComboBox.DisplayMemberPath = "FullInfo";
			DoctorComboBox.SelectedValuePath = "НомерВрача";
		}

		private void IsDoctorCheckBox_Checked(object sender, RoutedEventArgs e)
		{
			DoctorLabel.Visibility = Visibility.Visible;
			DoctorComboBox.Visibility = Visibility.Visible;

			// Скрытие полей ФИО и Роль
			LastNameLabel.Visibility = Visibility.Collapsed;
			LastNameTextBox.Visibility = Visibility.Collapsed;
			FirstNameLabel.Visibility = Visibility.Collapsed;
			FirstNameTextBox.Visibility = Visibility.Collapsed;
			MiddleNameLabel.Visibility = Visibility.Collapsed;
			MiddleNameTextBox.Visibility = Visibility.Collapsed;
			RoleLabel.Visibility = Visibility.Collapsed;
			RoleComboBox.Visibility = Visibility.Collapsed;

			// Автоматический выбор роли "Врач"
			var doctorRole = _db.Роль.FirstOrDefault(r => r.Ниаменование == "Врач");
			if (doctorRole != null)
			{
				RoleComboBox.SelectedValue = doctorRole.НомерЗаписи;
			}
		}

		private void IsDoctorCheckBox_Unchecked(object sender, RoutedEventArgs e)
		{
			DoctorLabel.Visibility = Visibility.Collapsed;
			DoctorComboBox.Visibility = Visibility.Collapsed;
			DoctorComboBox.SelectedItem = null;

			// Показ полей ФИО и Роль
			LastNameLabel.Visibility = Visibility.Visible;
			LastNameTextBox.Visibility = Visibility.Visible;
			FirstNameLabel.Visibility = Visibility.Visible;
			FirstNameTextBox.Visibility = Visibility.Visible;
			MiddleNameLabel.Visibility = Visibility.Visible;
			MiddleNameTextBox.Visibility = Visibility.Visible;
			RoleLabel.Visibility = Visibility.Visible;
			RoleComboBox.Visibility = Visibility.Visible;

			// Очищение выбранной роли
			RoleComboBox.SelectedItem = null;
		}

		private void RoleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (RoleComboBox.SelectedItem != null)
			{
				var selectedRole = (Роль)RoleComboBox.SelectedItem;
				IsDoctorCheckBox.IsEnabled = selectedRole.Ниаменование == "Врач";

				if (!IsDoctorCheckBox.IsEnabled)
				{
					IsDoctorCheckBox.IsChecked = false;
				}
			}
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(LoginTextBox.Text) || string.IsNullOrWhiteSpace(PasswordBox.Text))
				{
					MessageBox.Show("Логин и пароль обязательны для заполнения!", "Ошибка",
						MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}

				// Для врача проверка, что врач выбран
				if (IsDoctorCheckBox.IsChecked == true && DoctorComboBox.SelectedItem == null)
				{
					MessageBox.Show("Необходимо выбрать врача!", "Ошибка",
						MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}

				// Для обычного пользователя проверка заполнения ФИО и роли
				if (IsDoctorCheckBox.IsChecked != true && (string.IsNullOrWhiteSpace(LastNameTextBox.Text) || string.IsNullOrWhiteSpace(FirstNameTextBox.Text) ||
					RoleComboBox.SelectedItem == null))
				{
					MessageBox.Show("Для обычного пользователя необходимо заполнить ФИО и выбрать роль!", "Ошибка",
						MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}

				// Проверка на уникальность логина
				if (_db.Пользователи.Any(u => u.Логин == LoginTextBox.Text))
				{
					MessageBox.Show("Пользователь с таким логином уже существует!", "Ошибка",
						MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}

				try
				{
					var newUser = new Пользователи
					{
						Логин = LoginTextBox.Text,
						Пароль = PasswordBox.Text,
						Фамилия = LastNameTextBox.Text,
						Имя = FirstNameTextBox.Text,
						Отчество = MiddleNameTextBox.Text,
						Роль = (int)RoleComboBox.SelectedValue
					};

					// Если это врач, связка с выбранным врачом
					if (IsDoctorCheckBox.IsChecked == true && DoctorComboBox.SelectedItem != null)
					{
						dynamic selectedDoctor = DoctorComboBox.SelectedItem;
						newUser.НомерВрача = selectedDoctor.НомерВрача;
					}

					_db.Пользователи.Add(newUser);
					_db.SaveChanges();

					DialogResult = true;
					Close();
				}
				catch (Exception ex)
				{
					MessageBox.Show($"Ошибка при сохранении пользователя: {ex.Message}", "Ошибка",
						MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при сохранении пользователя: {ex.Message}", "Ошибка",
						MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
			Close();
		}
	}
}