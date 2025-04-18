using Hospital.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
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
		private БольницаEntities db;

		public AddPatientWindow(БольницаEntities dbs)
		{
			InitializeComponent();

			db = dbs;
			LoadData();

			// Инициализация по умолчанию
			IdentityDocumentComboBox.SelectedIndex = 0;

			BirthDatePicker.SelectedDate = DateTime.Today;
			BirthDatePicker_SelectedDateChanged(null, null);
		}

		public void LoadData()
		{
			// Заполнение ComboBox субъектами РФ
			RegionComboBox.ItemsSource = new List<Region>
			{
				new Region { Id = 1, Name = "Республика Адыгея" },
				new Region { Id = 2, Name = "Республика Алтай" },
				new Region { Id = 3, Name = "Республика Башкортостан" },
				new Region { Id = 4, Name = "Республика Бурятия" },
				new Region { Id = 5, Name = "Республика Дагестан" },
				new Region { Id = 6, Name = "Республика Ингушетия" },
				new Region { Id = 7, Name = "Кабардино-Балкарская Республика" },
				new Region { Id = 8, Name = "Республика Калмыкия" },
				new Region { Id = 9, Name = "Карачаево-Черкесская Республика" },
				new Region { Id = 10, Name = "Республика Карелия" },
				new Region { Id = 11, Name = "Республика Коми" },
				new Region { Id = 12, Name = "Республика Крым" },
				new Region { Id = 13, Name = "Республика Марий Эл" },
				new Region { Id = 14, Name = "Республика Мордовия" },
				new Region { Id = 15, Name = "Республика Саха (Якутия)" },
				new Region { Id = 16, Name = "Республика Северная Осетия - Алания" },
				new Region { Id = 17, Name = "Республика Татарстан" },
				new Region { Id = 18, Name = "Республика Тыва" },
				new Region { Id = 19, Name = "Удмуртская Республика" },
				new Region { Id = 20, Name = "Республика Хакасия" },
				new Region { Id = 21, Name = "Чеченская Республика" },
				new Region { Id = 22, Name = "Чувашская Республика" },
				new Region { Id = 23, Name = "Алтайский край" },
				new Region { Id = 24, Name = "Забайкальский край" },
				new Region { Id = 25, Name = "Камчатский край" },
				new Region { Id = 26, Name = "Краснодарский край" },
				new Region { Id = 27, Name = "Красноярский край" },
				new Region { Id = 28, Name = "Пермский край" },
				new Region { Id = 29, Name = "Приморский край" },
				new Region { Id = 30, Name = "Ставропольский край" },
				new Region { Id = 31, Name = "Хабаровский край" },
				new Region { Id = 32, Name = "Амурская область" },
				new Region { Id = 33, Name = "Архангельская область" },
				new Region { Id = 34, Name = "Астраханская область" },
				new Region { Id = 35, Name = "Белгородская область" },
				new Region { Id = 36, Name = "Брянская область" },
				new Region { Id = 37, Name = "Владимирская область" },
				new Region { Id = 38, Name = "Волгоградская область" },
				new Region { Id = 39, Name = "Вологодская область" },
				new Region { Id = 40, Name = "Воронежская область" },
				new Region { Id = 41, Name = "Ивановская область" },
				new Region { Id = 42, Name = "Иркутская область" },
				new Region { Id = 43, Name = "Калининградская область" },
				new Region { Id = 44, Name = "Калужская область" },
				new Region { Id = 45, Name = "Кемеровская область" },
				new Region { Id = 46, Name = "Кировская область" },
				new Region { Id = 47, Name = "Костромская область" },
				new Region { Id = 48, Name = "Курганская область" },
				new Region { Id = 49, Name = "Курская область" },
				new Region { Id = 50, Name = "Ленинградская область" },
				new Region { Id = 51, Name = "Липецкая область" },
				new Region { Id = 52, Name = "Магаданская область" },
				new Region { Id = 53, Name = "Московская область" },
				new Region { Id = 54, Name = "Мурманская область" },
				new Region { Id = 55, Name = "Нижегородская область" },
				new Region { Id = 56, Name = "Новгородская область" },
				new Region { Id = 57, Name = "Новосибирская область" },
				new Region { Id = 58, Name = "Омская область" },
				new Region { Id = 59, Name = "Оренбургская область" },
				new Region { Id = 60, Name = "Орловская область" },
				new Region { Id = 61, Name = "Пензенская область" },
				new Region { Id = 62, Name = "Псковская область" },
				new Region { Id = 63, Name = "Ростовская область" },
				new Region { Id = 64, Name = "Рязанская область" },
				new Region { Id = 65, Name = "Самарская область" },
				new Region { Id = 66, Name = "Саратовская область" },
				new Region { Id = 67, Name = "Сахалинская область" },
				new Region { Id = 68, Name = "Свердловская область" },
				new Region { Id = 69, Name = "Смоленская область" },
				new Region { Id = 70, Name = "Тамбовская область" },
				new Region { Id = 71, Name = "Тверская область" },
				new Region { Id = 72, Name = "Томская область" },
				new Region { Id = 73, Name = "Тульская область" },
				new Region { Id = 74, Name = "Тюменская область" },
				new Region { Id = 75, Name = "Ульяновская область" },
				new Region { Id = 76, Name = "Челябинская область" },
				new Region { Id = 77, Name = "Ярославская область" },
				new Region { Id = 78, Name = "Москва" },
				new Region { Id = 79, Name = "Санкт-Петербург" },
				new Region { Id = 80, Name = "Севастополь" },
				new Region { Id = 81, Name = "Еврейская автономная область" },
				new Region { Id = 82, Name = "Ненецкий автономный округ" },
				new Region { Id = 83, Name = "Ханты-Мансийский автономный округ - Югра" },
				new Region { Id = 84, Name = "Чукотский автономный округ" },
				new Region { Id = 85, Name = "Ямало-Ненецкий автономный округ" },
				new Region { Id = 86, Name = "Донецкая Народная Республика" },
				new Region { Id = 87, Name = "Запорожская область" },
				new Region { Id = 88, Name = "Луганская Народная Республика" },                
				new Region { Id = 89, Name = "Херсонская область" }
			};
		}

		private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			PersonalDataPanel.Visibility = Visibility.Collapsed;
			DocumentsPanel.Visibility = Visibility.Collapsed;
			HealthPanel.Visibility = Visibility.Collapsed;
			ContactInfoPanel.Visibility = Visibility.Collapsed;
			AddressPanel.Visibility = Visibility.Collapsed;
			InsurancePanel.Visibility = Visibility.Collapsed;
			SocialInfoPanel.Visibility = Visibility.Collapsed;
			DisabilityPanel.Visibility = Visibility.Collapsed;
			RepresentativePanel.Visibility = Visibility.Collapsed;
			HasRepresentativeCheckBox.Visibility = Visibility.Collapsed;

			// Показать выбранную панель
			var selectedItem = (TreeViewItem)e.NewValue;
			switch (selectedItem.Tag.ToString())
			{
				case "PersonalData":
					PersonalDataPanel.Visibility = Visibility.Visible;
					HasRepresentativeCheckBox.Visibility = Visibility.Visible;
					// При возврате на личные данные проверяем нужно ли показывать представителя
					UpdateRepresentativePanelVisibility();
					break;
				case "Documents":
					DocumentsPanel.Visibility = Visibility.Visible;
					break;
				case "Health":
					HealthPanel.Visibility = Visibility.Visible;
					break;
				case "ContactInfo":
					ContactInfoPanel.Visibility = Visibility.Visible;
					break;
				case "Address":
					AddressPanel.Visibility = Visibility.Visible;
					break;
				case "Insurance":
					InsurancePanel.Visibility = Visibility.Visible;
					break;
				case "SocialInfo":
					SocialInfoPanel.Visibility = Visibility.Visible;
					break;
				case "Disability":
					DisabilityPanel.Visibility = Visibility.Visible;
					break;
			}
		}

		private void UpdateRepresentativePanelVisibility()
		{
			if (HasRepresentativeCheckBox.IsChecked == true)
			{
				RepresentativePanel.Visibility = Visibility.Visible;
			}
			else if (BirthDatePicker.SelectedDate.HasValue)
			{
				var age = CalculateAge(BirthDatePicker.SelectedDate.Value);
				if (age < 18)
				{
					RepresentativePanel.Visibility = Visibility.Visible;
				}
			}
		}

		private string FormatSnils(string snils)
		{
			if (string.IsNullOrWhiteSpace(snils)) return snils;

			// Удаляем все нецифровые символы
			string digitsOnly = new string(snils.Where(char.IsDigit).ToArray());

			if (digitsOnly.Length != 11) return snils;

			// Форматируем по шаблону: 000-000-000 00
			return $"{digitsOnly.Substring(0, 3)}-{digitsOnly.Substring(3, 3)}-{digitsOnly.Substring(6, 3)} {digitsOnly.Substring(9, 2)}";
		}

		private async void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				// Валидация обязательных полей
				if (string.IsNullOrWhiteSpace(LastNameTextBox.Text) ||
					string.IsNullOrWhiteSpace(FirstNameTextBox.Text) ||
					BirthDatePicker.SelectedDate == null ||
					GenderComboBox.SelectedItem == null ||
					string.IsNullOrWhiteSpace(PhoneTextBox.Text))
				{
					MessageBox.Show("Пожалуйста, заполните все обязательные поля (*)");
					return;
				}

				// Проверка СНИЛС (11 цифр)
				if (!string.IsNullOrWhiteSpace(SnilsTextBox.Text) &&
					(SnilsTextBox.Text.Length != 11 || !long.TryParse(SnilsTextBox.Text, out _)))
				{
					MessageBox.Show("СНИЛС должен содержать ровно 11 цифр");
					return;
				}

				
				// Создаем новый адрес
				Адрес address = new Адрес
				{
					Субъект = (RegionComboBox.SelectedItem as Region)?.Name ?? RegionComboBox.Text,
					Район = DistrictTextBox.Text,
					Город = CityTextBox.Text,
					НаселенныйПункт = LocalityTextBox.Text,
					Улица = StreetTextBox.Text,
					Дом = HouseTextBox.Text,
					Квартира = ApartmentTextBox.Text
				};
				db.Адрес.Add(address);

				// Создаем документ пациента
				Документ document = new Документ
				{
					НаименованиеДокумента = (IdentityDocumentComboBox.SelectedItem as ComboBoxItem)?.Content.ToString(),
					Серия = DocumentSeriesTextBox.Text,
					Номер = DocumentNumberTextBox.Text,
					ДатаВыдачи = PatientDocIssueDatePicker.SelectedDate ?? DateTime.Now,
					КемВыдан = PatientDocIssuedByWhom.Text
				};
				db.Документ.Add(document);

				await db.SaveChangesAsync();

				// Создаем страховку (если данные заполнены)
				Страховка insurance = null;
				if (!string.IsNullOrWhiteSpace(InsurancePolicyLabel.Text))
				{
					insurance = new Страховка
					{
						СерияПолиса = InsurancePolicyLabel.Text,
						НаименованиеСтраховойКомпании = InsuranceCompanyLabel.Text,
						СрокДействия = DateTime.Now.AddYears(40)
					};
					db.Страховка.Add(insurance);
				}

				// Сохраняем промежуточные данные
				await db.SaveChangesAsync();

				// Создаем пациента
				Пациент patient = new Пациент
				{
					Фамилия = LastNameTextBox.Text,
					Имя = FirstNameTextBox.Text,
					Отчество = MiddleNameTextBox.Text,
					ДатаРождения = BirthDatePicker.SelectedDate.Value,
					Пол = (GenderComboBox.SelectedItem as ComboBoxItem)?.Content.ToString(),
					СНИЛС = FormatSnils(SnilsTextBox.Text),
					ИНН = InnTextBox.Text,
					Телефон = PhoneTextBox.Text,
					СемейноеПоложение = (MaritalStatusComboBox.SelectedItem as ComboBoxItem)?.Content.ToString(),
					Образование = (EducationComboBox.SelectedItem as ComboBoxItem)?.Content.ToString(),
					Занятость = (EmploymentComboBox.SelectedItem as ComboBoxItem)?.Content.ToString(),
					НомерДокумента = document.НомерЗаписи,
					Адрес = address.Номер,
					НомерСтраховки = insurance?.НомерЗаписи
				};
				db.Пациент.Add(patient);

				// Сохраняем пациента
				await db.SaveChangesAsync();

				// Добавляем инвалидность (если указана)
				if (HasDisabilityComboBox.SelectedIndex == 1 && DisabilityGroupComboBox.SelectedItem != null)
				{
					Инвалидность disability = new Инвалидность
					{
						НомерПациента = patient.НомерПациента,
						Группа = (DisabilityGroupComboBox.SelectedItem as ComboBoxItem)?.Content.ToString(),
						Описание = DisabilityDescriptionTextBox.Text,
						Дата = DisabilityDatePicker.SelectedDate
					};
					db.Инвалидность.Add(disability);
				}

				// Добавляем представителя (если указан)
				if (RepresentativePanel.Visibility == Visibility.Visible &&
					!string.IsNullOrWhiteSpace(RepresentativeLastNameTextBox.Text) &&
					!string.IsNullOrWhiteSpace(RepresentativeFirstNameTextBox.Text)&&
					!string.IsNullOrWhiteSpace(RepresentativeDocIssuedByWhom.Text))
				{
					// Создаем документ представителя
					Документ repDocument = new Документ
					{
						НаименованиеДокумента = (RepresentativeDocTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString(),
						Серия = RepresentativeDocSeriesTextBox.Text,
						Номер = RepresentativeDocNumberTextBox.Text,
						ДатаВыдачи = RepresentativeDocIssueDatePicker.SelectedDate ?? DateTime.Now,
						КемВыдан = RepresentativeDocIssuedByWhom.Text
					};
					db.Документ.Add(repDocument);

					// Сохраняем документ
					await db.SaveChangesAsync();

					ПредставительПациента representative = new ПредставительПациента
					{
						НаименованиеПредставителя = (RepresentativeTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString(),
						Фамилия = RepresentativeLastNameTextBox.Text,
						Имя = RepresentativeFirstNameTextBox.Text,
						Отчетсво = RepresentativeMiddleNameTextBox.Text,
						Телефон = RepresentativePhoneTextBox.Text,
						НомерПациента = patient.НомерПациента,
						НомерДокумента = repDocument.НомерЗаписи
					};
					db.ПредставительПациента.Add(representative);
				}

				// Финальное сохранение
				await db.SaveChangesAsync();

				MessageBox.Show("Пациент успешно зарегистрирован!");
				this.DialogResult = true;
				this.Close();
			}
			catch (DbEntityValidationException ex)
			{
				var errorMessages = ex.EntityValidationErrors
					.SelectMany(x => x.ValidationErrors)
					.Select(x => x.ErrorMessage);

				string fullErrorMessage = string.Join("\n", errorMessages);
				MessageBox.Show($"Ошибки валидации:\n{fullErrorMessage}", "Ошибка сохранения");
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при сохранении: {ex.InnerException?.Message ?? ex.Message}");
			}
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
			Close();
		}

		// Валидация числовых полей
		private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{			
			if (!char.IsDigit(e.Text, e.Text.Length - 1))
			{
				e.Handled = true;
			}
		}

		private void HasDisabilityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			DisabilityPanel1.Visibility = HasDisabilityComboBox.SelectedIndex == 1
				? Visibility.Visible
				: Visibility.Collapsed;
		}

		// Добавим проверку возраста при изменении даты рождения
		private void BirthDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
		{
			if (BirthDatePicker.SelectedDate.HasValue)
			{
				var age = CalculateAge(BirthDatePicker.SelectedDate.Value);
				bool isMinor = age < 18;

				if (isMinor)
				{
					HasRepresentativeCheckBox.IsChecked = true;
					HasRepresentativeCheckBox.IsEnabled = false;
					// Не меняем Visibility здесь, это сделает UpdateRepresentativePanelVisibility
					IdentityDocumentComboBox.SelectedIndex = 2;
				}
				else
				{
					HasRepresentativeCheckBox.IsEnabled = true;
					IdentityDocumentComboBox.SelectedIndex = 0;
				}
			}
			UpdateRepresentativePanelVisibility();
		}

		private void HasRepresentativeCheckBox_Checked(object sender, RoutedEventArgs e)
		{
			UpdateRepresentativePanelVisibility();
			//RepresentativePanel.Visibility = Visibility.Visible;
		}

		private void HasRepresentativeCheckBox_Unchecked(object sender, RoutedEventArgs e)
		{
			if (BirthDatePicker.SelectedDate.HasValue && CalculateAge(BirthDatePicker.SelectedDate.Value) >= 18)
			{
				UpdateRepresentativePanelVisibility();
			}
			else
			{
				HasRepresentativeCheckBox.IsChecked = true;
			}
		}

		// Метод для вычисления возраста
		private int CalculateAge(DateTime birthDate)
		{
			var today = DateTime.Today;
			var age = today.Year - birthDate.Year;

			// Проверяем, был ли уже день рождения в этом году
			if (birthDate.Date > today.AddYears(-age))
			{
				age--;
			}

			return age;
		}
	}
}
