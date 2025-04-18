using Hospital.Model;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Hospital.AdminWindow
{
	/// <summary>
	/// Логика взаимодействия для UsersPage.xaml
	/// </summary>
	public partial class UsersPage : Page
	{
		private БольницаEntities _db = new БольницаEntities();

		// Время последнего клика для обработки двойного нажатия
		private DateTime _lastClickTime;

		// Последний выбранный элемент для обработки двойного нажатия
		private object _lastClickedItem;

		public UsersPage()
		{
			InitializeComponent();
			LoadUsers();
		}

		// Загрузка списка пользователей из базы данных
		private void LoadUsers()
		{
			try
			{
				try
				{
					var users = from pol in _db.Пользователи
								join ro in _db.Роль on pol.Роль equals ro.НомерЗаписи
								select new UserView
								{
									НомерЗаписи = pol.НомерЗаписи,
									Логин = pol.Логин,
									Фамилия = pol.Фамилия,
									Имя = pol.Имя,
									Отчество = pol.Отчество,
									Роль = ro.Ниаменование,
								};

					UsersGrid.ItemsSource = users.ToList();
				}
				catch (Exception ex)
				{
					MessageBox.Show("Ошибка загрузки: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
			catch 
			{
				MessageBox.Show("Ошибка загрузки!");
			}
		}

		// Обработчик нажатия кнопки добавления пользователя
		private void AddUser_Click(object sender, RoutedEventArgs e)
		{
			// Создание и отображение окна добавления пользователя
			var addWindow = new AddUserWindow(_db)
			{
				Owner = Window.GetWindow(this)
			};

			if (addWindow.ShowDialog() == true)
			{
				LoadUsers();
			}
		}

		// Обработчик нажатия кнопки удаления пользователя
		private void DeleteUser_Click(object sender, RoutedEventArgs e)
		{
			if (UsersGrid.SelectedItem is UserView selectedUser)
			{
				var result = MessageBox.Show($"Удалить пользователя {selectedUser.Логин}?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

				if (result == MessageBoxResult.Yes)
				{
					try
					{
						// Поиск и удаление пользователя
						var userToDelete = _db.Пользователи.Find(selectedUser.НомерЗаписи);
						if (userToDelete != null)
						{
							_db.Пользователи.Remove(userToDelete);
							_db.SaveChanges();
							LoadUsers();
							MessageBox.Show("Пользователь удален", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
						}
					}
					catch (Exception ex)
					{
						MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка",
							MessageBoxButton.OK, MessageBoxImage.Error);
					}
				}
			}
			else
			{
				MessageBox.Show("Выберите пользователя для удаления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		// Обработчик нажатия клавиш в DataGrid
		private void DoctorsGrid_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Delete)
			{
				DeleteUser_Click(sender, e);
			}
		}

		// Обработчик нажатия кнопки сохранения изменений
		private void Save_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				// Завершаем редактирование перед сохранением
				UsersGrid.CommitEdit();
				// Возвращаем режим только для чтения
				UsersGrid.IsReadOnly = true;

				// Сохранение изменений для всех отредактированных пользователей
				foreach (var item in UsersGrid.Items)
				{
					if (item is UserView userView)
					{
						var user = _db.Пользователи.Find(userView.НомерЗаписи);
						if (user != null)
						{
							user.Логин = userView.Логин;
							user.Фамилия = userView.Фамилия;
							user.Имя = userView.Имя;
							user.Отчество = userView.Отчество;
						}
					}
				}

				_db.SaveChanges();
				MessageBox.Show("Изменения сохранены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		// Обработчик нажатия левой кнопки мыши в DataGrid
		private void UsersGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			// Получаем ячейку, по которой кликнули
			var cell = GetClickedCell(e.OriginalSource as DependencyObject);
			if (cell == null) return;

			// Получаем элемент данных
			var item = cell.DataContext;

			// Проверяем двойной клик (интервал менее 300 мс)
			if (item == _lastClickedItem && (DateTime.Now - _lastClickTime).TotalMilliseconds < 300)
			{
				// Двойной клик - разрешаем редактирование
				UsersGrid.IsReadOnly = false;
				UsersGrid.BeginEdit();

				// Сбрасываем таймер
				_lastClickedItem = null;
				e.Handled = true;
			}
			else
			{
				// Одинарный клик - просто выделяем
				_lastClickTime = DateTime.Now;
				_lastClickedItem = item;
			}
		}

		// Вспомогательный метод для получения ячейки DataGrid по элементу визуального дерева
		private DataGridCell GetClickedCell(DependencyObject depObj)
		{
			while (depObj != null && !(depObj is DataGridCell))
			{
				depObj = VisualTreeHelper.GetParent(depObj);
			}
			return depObj as DataGridCell;
		}
	}
}