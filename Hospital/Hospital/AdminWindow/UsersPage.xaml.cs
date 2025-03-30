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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hospital.AdminWindow
{
	/// <summary>
	/// Логика взаимодействия для UsersPage.xaml
	/// </summary>
	public partial class UsersPage : Page
	{
		private БольницаEntities _db = new БольницаEntities();

		public UsersPage()
		{
			InitializeComponent();
			LoadUsers();
		}

		private void LoadUsers()
		{
			try
			{
				try
				{
					var users = _db.Пользователи
						.Select(u => new UserView
						{
							НомерЗаписи = u.НомерЗаписи,
							Логин = u.Логин,
							Фамилия = u.Фамилия,
							Имя = u.Имя,
							Отчество = u.Отчество,
							Роль = u.Роль1.Ниаменование
						})
						.ToList();

					UsersGrid.ItemsSource = users;
				}
				catch (Exception ex)
				{
					MessageBox.Show("Ошибка загрузки: " + ex.Message, "Ошибка",
						MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Ошибка загрузки: " + ex.Message);
			}
		}

		private void AddUser_Click(object sender, RoutedEventArgs e)
		{
			var addWindow = new AddUserWindow(_db)
			{
				Owner = Window.GetWindow(this)
			};

			if (addWindow.ShowDialog() == true)
			{
				// Обновление данных после успешного добавления
				LoadUsers();
			}
		}

		private void DeleteUser_Click(object sender, RoutedEventArgs e)
		{

			var selectedUser = UsersGrid.SelectedItem as UserView;
			if (selectedUser == null)
			{
				MessageBox.Show("Выберите пользователя для удаления", "Ошибка",
					MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			var result = MessageBox.Show($"Удалить пользователя {selectedUser.Логин}?",
				"Подтверждение удаления",
				MessageBoxButton.YesNo,
				MessageBoxImage.Question);

			if (result == MessageBoxResult.Yes)
			{
				try
				{
					var userToDelete = _db.Пользователи.Find(selectedUser.НомерЗаписи);
					if (userToDelete != null)
					{
						_db.Пользователи.Remove(userToDelete);
						_db.SaveChanges();
						LoadUsers();
						MessageBox.Show("Пользователь удален", "Успех",
							MessageBoxButton.OK, MessageBoxImage.Information);
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка",
						MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
		}

		private void DoctorsGrid_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Delete)
			{
				DeleteUser_Click(sender, e);
			}
		}

		private void Save_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				_db.SaveChanges();
				MessageBox.Show("Изменения сохранены", "Успех",
					MessageBoxButton.OK, MessageBoxImage.Information);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка",
					MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}