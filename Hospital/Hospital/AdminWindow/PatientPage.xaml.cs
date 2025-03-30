using Hospital.Model;
using Hospital.RegistrarWindow;
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
	/// Логика взаимодействия для PatientPage.xaml
	/// </summary>
	public partial class PatientPage : Page
	{
		private БольницаEntities _db = new БольницаEntities();
		private List<PatientView> _allPatients = new List<PatientView>();

		public PatientPage()
		{
			InitializeComponent();
			LoadPatients();
		}

		private void LoadPatients()
		{
			try
			{
				_allPatients = _db.Пациент
					.Select(p => new PatientView
					{
						НомерПациента = p.НомерПациента,
						Фамилия = p.Фамилия,
						Имя = p.Имя,
						Отчество = p.Отчество,
						ДатаРождения = p.ДатаРождения,
						НомерДокумента = p.НомерДокумента,
						Пол = p.Пол,
						СНИЛС = p.СНИЛС,
						Телефон = p.Телефон
					})
					.ToList();

				PatientsGrid.ItemsSource = _allPatients;
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка загрузки пациентов: {ex.Message}", "Ошибка",
					MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void SearchPatients(string searchText)
		{
			if (string.IsNullOrWhiteSpace(searchText))
			{
				PatientsGrid.ItemsSource = _allPatients;
				return;
			}

			searchText = searchText.ToLower();

			var filteredPatients = _allPatients.Where(p =>
				(p.Фамилия != null && p.Фамилия.ToLower().Contains(searchText)) ||
				(p.Имя != null && p.Имя.ToLower().Contains(searchText)) ||
				(p.Отчество != null && p.Отчество.ToLower().Contains(searchText)) ||
				(p.ДатаРождения.HasValue && p.ДатаРождения.Value.Year.ToString().Contains(searchText))
			).ToList();

			PatientsGrid.ItemsSource = filteredPatients;
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
			PatientsGrid.ItemsSource = _allPatients;
		}

		private void AddUser_Click(object sender, RoutedEventArgs e)
		{
			var addWindow = new AddPatientWindow(_db)
			{
				Owner = Window.GetWindow(this)
			};

			if (addWindow.ShowDialog() == true)
			{
				// Обновление списка пациентов после добавления
				LoadPatients();
			}
		}

		private void DeleteUser_Click(object sender, RoutedEventArgs e)
		{
			var selectedPatient = PatientsGrid.SelectedItem as PatientView;
			if (selectedPatient == null)
			{
				MessageBox.Show("Выберите пациента для удаления", "Ошибка",
					MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			var result = MessageBox.Show($"Удалить пациента {selectedPatient.Фамилия} {selectedPatient.Имя}?",
				"Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

			if (result == MessageBoxResult.Yes)
			{
				try
				{
					var patientToDelete = _db.Пациент.Find(selectedPatient.НомерПациента);
					if (patientToDelete != null)
					{
						_db.Пациент.Remove(patientToDelete);
						_db.SaveChanges();
						LoadPatients();
						MessageBox.Show("Пациент удален", "Успех",
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

	public class PatientView
	{
		public int НомерПациента { get; set; }
		public string Фамилия { get; set; }
		public string Имя { get; set; }
		public string Отчество { get; set; }
		public DateTime? ДатаРождения { get; set; }
		public int? НомерДокумента { get; set; }
		public string Пол { get; set; }
		public string СНИЛС { get; set; }
		public string Телефон { get; set; }
	}
}
