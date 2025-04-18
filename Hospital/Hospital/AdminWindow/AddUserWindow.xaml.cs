using Hospital.Model;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
namespace Hospital.AdminWindow
{
	/// <summary>
	/// Логика взаимодействия для AddUserWindow.xaml
	/// </summary>
	public partial class AddUserWindow : Window
	{
		private БольницаEntities db;
		// Флаг совпадения паролей
		private bool _passwordsMatch = false;

		public AddUserWindow(БольницаEntities _db)
		{
			InitializeComponent();
			db = _db;
			LoadRoles();
			LoadDoctors();

			IsDoctorCheckBox.Checked += IsDoctorCheckBox_Checked;
			IsDoctorCheckBox.Unchecked += IsDoctorCheckBox_Unchecked;
		}

		// Загрузка списка ролей из базы данных в ComboBox
		private void LoadRoles()
		{
			RoleComboBox.ItemsSource = db.Роль.ToList();
			RoleComboBox.DisplayMemberPath = "Ниаменование";
			RoleComboBox.SelectedValuePath = "НомерЗаписи";
		}

		// Загрузка списка врачей из базы данных в ComboBox
		private void LoadDoctors()
		{

			var doctors = db.Врач.ToList();
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

		// Обработчик изменения пароля в первом поле
		private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
		{
			CheckPasswordsMatch();
		}

		// Обработчик изменения пароля во втором поле (повтор)
		private void RepeatPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
		{
			CheckPasswordsMatch();
		}

		// Проверка совпадения паролей в обоих полях
		private void CheckPasswordsMatch()
		{
			string password = PasswordBox.Password;
			string repeatPassword = RepeatPasswordBox.Password;

			if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(repeatPassword))
			{
				PasswordMatchText.Text = "";
				_passwordsMatch = false;
			}
			else if (password == repeatPassword)
			{
				PasswordMatchText.Text = "Пароли совпадают";
				PasswordMatchText.Foreground = Brushes.Green;
				_passwordsMatch = true;
			}
			else
			{
				PasswordMatchText.Text = "Пароли не совпадают";
				PasswordMatchText.Foreground = Brushes.Red;
				_passwordsMatch = false;
			}
		}

		// Обработчик события выбора CheckBox "Это врач"
		private void IsDoctorCheckBox_Checked(object sender, RoutedEventArgs e)
		{
			DoctorLabel.Visibility = Visibility.Visible;
			DoctorComboBox.Visibility = Visibility.Visible;

			// Скрываем поля для ручного ввода ФИО
			LastNameLabel.Visibility = Visibility.Collapsed;
			LastNameTextBox.Visibility = Visibility.Collapsed;
			FirstNameLabel.Visibility = Visibility.Collapsed;
			FirstNameTextBox.Visibility = Visibility.Collapsed;
			MiddleNameLabel.Visibility = Visibility.Collapsed;
			MiddleNameTextBox.Visibility = Visibility.Collapsed;
			RoleLabel.Visibility = Visibility.Collapsed;
			RoleComboBox.Visibility = Visibility.Collapsed;

			// Автоматический выбор роли "Врач"
			var doctorRole = db.Роль.FirstOrDefault(r => r.Ниаменование == "Врач");
			if (doctorRole != null)
			{
				RoleComboBox.SelectedValue = doctorRole.НомерЗаписи;
			}
		}

		// Обработчик события снятия выбора CheckBox "Это врач"
		private void IsDoctorCheckBox_Unchecked(object sender, RoutedEventArgs e)
		{
			DoctorLabel.Visibility = Visibility.Collapsed;
			DoctorComboBox.Visibility = Visibility.Collapsed;
			DoctorComboBox.SelectedItem = null;

			// Показываем поля для ручного ввода ФИО
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

		// Обработчик изменения выбранной роли
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

		// Обработчик нажатия кнопки "Сохранить"
		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				// Проверка обязательных полей
				if (string.IsNullOrWhiteSpace(LoginTextBox.Text) || string.IsNullOrWhiteSpace(PasswordBox.Password))
				{
					MessageBox.Show("Логин и пароль обязательны для заполнения!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}

				// Проверка совпадения паролей
				if (!_passwordsMatch)
				{
					MessageBox.Show("Пароли не совпадают!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}

				// Для врача проверка, что врач выбран
				if (IsDoctorCheckBox.IsChecked == true && DoctorComboBox.SelectedItem == null)
				{
					MessageBox.Show("Необходимо выбрать врача!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}

				// Создаем нового пользователя
				var newUser = new Пользователи
				{
					Логин = LoginTextBox.Text,
					Пароль = PasswordBox.Password,
					Роль = (int)RoleComboBox.SelectedValue
				};

				// Если это врач, заполняем данные из выбранного врача
				if (IsDoctorCheckBox.IsChecked == true && DoctorComboBox.SelectedItem != null)
				{
					dynamic selectedDoctor = DoctorComboBox.SelectedItem;
					newUser.НомерВрача = selectedDoctor.НомерВрача;

					// Заполняем ФИО из данных врача
					newUser.Фамилия = selectedDoctor.Фамилия;
					newUser.Имя = selectedDoctor.Имя;
					newUser.Отчество = selectedDoctor.Отчество;
				}
				else
				{
					// Для обычного пользователя проверка заполнения ФИО
					if (string.IsNullOrWhiteSpace(LastNameTextBox.Text) || string.IsNullOrWhiteSpace(FirstNameTextBox.Text))
					{
						MessageBox.Show("Для обычного пользователя необходимо заполнить ФИО!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
						return;
					}

					newUser.Фамилия = LastNameTextBox.Text;
					newUser.Имя = FirstNameTextBox.Text;
					newUser.Отчество = MiddleNameTextBox.Text;
				}

				// Проверка на уникальность логина
				if (db.Пользователи.Any(u => u.Логин == LoginTextBox.Text))
				{
					MessageBox.Show("Пользователь с таким логином уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}

				try
				{
					db.Пользователи.Add(newUser);
					db.SaveChanges();

					DialogResult = true;
					Close();
				}
				catch (Exception ex)
				{
					MessageBox.Show($"Ошибка при сохранении пользователя: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при сохранении пользователя: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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