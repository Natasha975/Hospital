using Hospital.Model;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Hospital.AdminWindow
{
	/// <summary>
	/// Логика взаимодействия для DoctorPage.xaml
	/// </summary>
	public partial class DoctorPage : Page
	{
		private БольницаEntities db = new БольницаEntities();

		public DoctorPage()
		{
			InitializeComponent();
			LoadData();
		}

		// Загрузка данных о врачах из базы данных
		public void LoadData()
		{
			var doctors = db.Врач.ToList();
			var usedDoctorsIds = db.Пользователи.Select(u => u.НомерВрача).ToList();

			var doctorViews = doctors.Select(d => new DoctorView(d, db)
			{
				IsUsed = usedDoctorsIds.Contains(d.НомерВрача)
			}).ToList();

			DoctorsGrid.ItemsSource = doctorViews;
		}

		// Обработчик нажатия кнопки добавления врача
		private void AddDoctor_Click(object sender, RoutedEventArgs e)
		{
			// Создаем и показываем окно добавления врача
			var addWindow = new AddDoctorWindow(db)
			{
				Owner = Window.GetWindow(this)
			};

			// Если окно закрыто с результатом true, обновляем данные
			if (addWindow.ShowDialog() == true)
			{
				LoadData();
			}
		}

		// Обработчик нажатия кнопки удаления врача
		private void DeleteDoctor_Click(object sender, RoutedEventArgs e)
		{
			var selectedDoctor = DoctorsGrid.SelectedItem as DoctorView;

			if (selectedDoctor == null)
			{
				MessageBox.Show("Выберите врача для удаления", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
				return;
			}

			// Проверка, связан ли врач с пользователем
			bool isDoctorInUse = db.Пользователи.Any(u => u.НомерВрача == selectedDoctor.НомерВрача);

			if (isDoctorInUse)
			{
				MessageBox.Show("Нельзя удалить врача, так как он связан с учетной записью пользователя.\nСначала удалите соответствующего пользователя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			// Запрос подтверждения удаления
			var result = MessageBox.Show($"Вы уверены, что хотите удалить врача {selectedDoctor.Фамилия} {selectedDoctor.Имя}?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

			if (result == MessageBoxResult.Yes)
			{
				try
				{
					// Поиск врача в базе данных
					var doctorToDelete = db.Врач.Find(selectedDoctor.НомерВрача);

					if (doctorToDelete != null)
					{
						// Удаление врача
						db.Врач.Remove(doctorToDelete);
						db.SaveChanges();

						// Обновление списка врачей
						LoadData();

						MessageBox.Show("Врач успешно удален", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show($"Ошибка при удалении врача: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
		}

		// Обработчик изменения выбранного элемента в DataGrid
		private void DoctorsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (DoctorsGrid.SelectedItem != null)
			{
				var selectedDoctor = DoctorsGrid.SelectedItem as DoctorView;
				DeleteDoctor.IsEnabled = !selectedDoctor.IsUsed;
			}
		}

		// Обработчик нажатия клавиш в DataGrid
		private void DoctorsGrid_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Delete)
			{
				DeleteDoctor_Click(sender, e);
			}
		}

		// Обработчик нажатия кнопки сохранения изменений
		private async void Save_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				await db.SaveChangesAsync();
				MessageBox.Show("Данные сохранены успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}