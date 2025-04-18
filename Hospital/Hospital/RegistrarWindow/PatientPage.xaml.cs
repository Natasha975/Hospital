using Hospital.AdminWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Hospital.Model;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hospital.RegistrarWindow
{
	/// <summary>
	/// Логика взаимодействия для PatientPage.xaml
	/// </summary>
	public partial class PatientPage : Page
	{
		private БольницаEntities db = new БольницаEntities();

		public PatientPage()
		{
			InitializeComponent();

			LoadPatients();
		}

		private void LoadPatients()
		{
			try
			{
				var _allPatients = from pa in db.Пациент
								   join oms in db.Страховка on pa.НомерСтраховки equals oms.НомерЗаписи
								   select new PatientView
								   {
									   НомерПациента = pa.НомерПациента,
									   Фамилия = pa.Фамилия,
									   Имя = pa.Имя,
									   Отчество = pa.Отчество,
									   ДатаРождения = pa.ДатаРождения,
									   Пол = pa.Пол,
									   СНИЛС = pa.СНИЛС,
									   Телефон = pa.Телефон,
									   ОМС = oms.СерияПолиса + oms.НомерПолиса
									   // Возраст вычисляется автоматически в свойстве
								   };

				PatientsGrid.ItemsSource = _allPatients.ToList();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка загрузки пациентов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void SearchPatients(string searchText)
		{
			try
			{
				// Сначала получаем всех пациентов как список
				var allPatients = (from pa in db.Пациент
								   join oms in db.Страховка on pa.НомерСтраховки equals oms.НомерЗаписи
								   select new PatientView
								   {
									   НомерПациента = pa.НомерПациента,
									   Фамилия = pa.Фамилия,
									   Имя = pa.Имя,
									   Отчество = pa.Отчество,
									   ДатаРождения = pa.ДатаРождения,
									   Пол = pa.Пол,
									   СНИЛС = pa.СНИЛС,
									   Телефон = pa.Телефон,
									   ОМС = oms.СерияПолиса + oms.НомерПолиса
								   }).ToList(); // Важно: материализуем запрос перед фильтрацией

				// Если строка поиска пустая - показываем всех пациентов
				if (string.IsNullOrWhiteSpace(searchText))
				{
					PatientsGrid.ItemsSource = allPatients;
					return;
				}

				searchText = searchText.ToLower();

				// Фильтруем уже в памяти
				var filteredPatients = allPatients.Where(p =>
					(p.Фамилия != null && p.Фамилия.ToLower().Contains(searchText)) ||
					(p.Имя != null && p.Имя.ToLower().Contains(searchText)) ||
					(p.Отчество != null && p.Отчество.ToLower().Contains(searchText)) ||
					(p.ОМС != null && p.ОМС.ToLower().Contains(searchText)) ||
					(p.Телефон != null && p.Телефон.ToLower().Contains(searchText)) ||
					(p.СНИЛС != null && p.СНИЛС.ToLower().Contains(searchText)) ||
					(p.ДатаРождения.HasValue && p.ДатаРождения.Value.ToString("dd.MM.yyyy").Contains(searchText)) ||
					(p.Возраст.HasValue && p.Возраст.Value.ToString().Contains(searchText))

				).ToList();

				PatientsGrid.ItemsSource = filteredPatients;
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при поиске пациентов: {ex.Message}", "Ошибка",
					MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		// Обработчики событий для поиска
		private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			SearchPatients(SearchTextBox.Text);
		}

		private void SearchButton_Click(object sender, RoutedEventArgs e)
		{
			SearchPatients(SearchTextBox.Text);
		}

		private void ResetSearch_Click(object sender, RoutedEventArgs e)
		{
			SearchTextBox.Text = "";
		}

		private void PatientsGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			// Проверяем, что клик был по строке, а не по заголовку или пустому месту
			if (e.OriginalSource is Visual visual)
			{
				var row = FindParent<DataGridRow>(visual);
				if (row != null && row.Item is PatientView selectedPatient)
				{
					OpenEditPatientWindow(selectedPatient);
				}
			}
		}

		private static T FindParent<T>(DependencyObject child) where T : DependencyObject
		{
			while (child != null && !(child is T))
			{
				child = VisualTreeHelper.GetParent(child);
			}
			return child as T;
		}

		private void OpenEditPatientWindow(PatientView patient)
		{
			var patientFromDb = db.Пациент.Find(patient.НомерПациента);
			if (patientFromDb == null) return;

			if (Window.GetWindow(this) is RegistrarWindow registrarWindow)
			{
				// Подписываемся на событие возвращения
				registrarWindow.MainFrame.Navigated += (sender, args) =>
				{
					if (args.Content is PatientPage) // Если вернулись на эту страницу
					{
						LoadPatients(); // Обновляем данные
					}
				};

				registrarWindow.MainFrame.Navigate(new EditPatientPage(db, patientFromDb));
			}
		}

		private void AddUser_Click(object sender, RoutedEventArgs e)
		{
			var addWindow = new AddPatientWindow(db)
			{
				Owner = Window.GetWindow(this)
			};

			if (addWindow.ShowDialog() == true)
			{
				LoadPatients();
			}
		}

		private void Save_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				db.SaveChanges();
				MessageBox.Show("Изменения сохранены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}