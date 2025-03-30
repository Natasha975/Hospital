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

		public void LoadData()
		{
			var quer = db.Врач.ToList();

			DoctorsGrid.ItemsSource = quer;
		}

		private void AddDoctor_Click(object sender, RoutedEventArgs e)
		{
			var addWindow = new AddDoctorWindow(db)
			{
				Owner = Window.GetWindow(this)
			};

			if (addWindow.ShowDialog() == true)
			{
				LoadData();
			}
		}

		private void DeleteDoctor_Click(object sender, RoutedEventArgs e)
		{
			// Получение выделенного врача
			var selectedDoctor = DoctorsGrid.SelectedItem as Врач;

			// Проверка, что врач выбран
			if (selectedDoctor == null)
			{
				MessageBox.Show("Выберите врача для удаления", "Информация",
							  MessageBoxButton.OK, MessageBoxImage.Information);
				return;
			}

			// Запрос подтверждения удаления
			var result = MessageBox.Show($"Вы уверены, что хотите удалить врача {selectedDoctor.Фамилия} {selectedDoctor.Имя}?",
									   "Подтверждение удаления",
									   MessageBoxButton.YesNo,
									   MessageBoxImage.Question);

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

						MessageBox.Show("Врач успешно удален", "Успех",
									   MessageBoxButton.OK, MessageBoxImage.Information);
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show($"Ошибка при удалении врача: {ex.Message}", "Ошибка",
								  MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
		}

		private void DoctorsGrid_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Delete)
			{
				DeleteDoctor_Click(sender, e);
			}
		}

		private void Save_Click(object sender, RoutedEventArgs e)
		{
			db.SaveChanges();
		}
	}
}