using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Hospital.Model;

namespace Hospital.RegistrarWindow
{
	/// <summary>
	/// Логика взаимодействия для EditPatientPage.xaml
	/// </summary>
	public partial class EditPatientPage : Page
	{
		private class PatientEditData
		{
			// Основные данные пациента
			public string Фамилия { get; set; }
			public string Имя { get; set; }
			public string Отчество { get; set; }
			public DateTime? ДатаРождения { get; set; }
			public string Пол { get; set; }
			public string СНИЛС { get; set; }
			public string ИНН { get; set; }
			public string Телефон { get; set; }
			public string СемейноеПоложение { get; set; }
			public string Образование { get; set; }
			public string Занятость { get; set; }

			// Данные документа
			public string НаименованиеДокумента { get; set; }
			public string Серия { get; set; }
			public string Номер { get; set; }
			public DateTime ДатаВыдачи { get; set; }
			public string КемВыдан { get; set; }

			// Данные адреса
			public string Субъект { get; set; }
			public string Район { get; set; }
			public string Город { get; set; }
			public string НаселенныйПункт { get; set; }
			public string Улица { get; set; }
			public string Дом { get; set; }
			public string Квартира { get; set; }

			// Данные страховки
			public string СерияНомер { get; set; }
			public string НаименованиеСтраховойКомпании { get; set; }

			// Данные инвалидности
			public string Группа { get; set; }
			public string Описание { get; set; }
			public DateTime? Дата { get; set; }

			// Данные о работе
			public string МестоРаботы { get; set; }
			public string Должность { get; set; }
		}

		private readonly БольницаEntities _db;
		private readonly Пациент _patient;
		//private readonly Адрес _address;
		//private readonly Документ _document;
		//private readonly Страховка _insurance;
		//private readonly Инвалидность _disability;
		//private readonly Работа _work;

		//private class PatientEditData
		//{
		//	public string Фамилия { get; set; }
		//	public string Имя { get; set; }
		//	public string Отчество { get; set; }
		//	public DateTime? ДатаРождения { get; set; }
		//	public string СНИЛС { get; set; }
		//	public string ИНН { get; set; }
		//	public string Телефон { get; set; }
		//	public string СемейноеПоложение { get; set; }
		//	public string Образование { get; set; }
		//	public string Занятость { get; set; }
		//	public string Пол { get; set; }

		//	// Данные документа
		//	public string НаименованиеДокумента { get; set; }
		//	public string Серия { get; set; }
		//	public string Номер { get; set; }
		//	public DateTime ДатаВыдачи { get; set; }
		//	public string КемВыдан { get; set; }

		//	// Данные адреса
		//	public string Субъект { get; set; }
		//	public string Район { get; set; }
		//	public string Город { get; set; }
		//	public string НаселенныйПункт { get; set; }
		//	public string Улица { get; set; }
		//	public string Дом { get; set; }
		//	public string Квартира { get; set; }

		//	// Данные страховки
		//	public string СерияНомер { get; set; }
		//	public string НаименованиеСтраховойКомпании { get; set; }

		//	// Данные инвалидности
		//	public string Группа { get; set; }
		//	public string Описание { get; set; }
		//	public DateTime? Дата { get; set; }

		//	// Данные о работе
		//	public string МестоРаботы { get; set; }
		//	public string Должность { get; set; }
		//}

		public EditPatientPage(БольницаEntities db, Пациент patient)
		{
			InitializeComponent();
			_db = db;
			_patient = patient;

			// Явная загрузка связанных данных
			_db.Entry(_patient).Reference(p => p.Документ).Load();
			_db.Entry(_patient).Reference(p => p.Адрес1).Load();
			_db.Entry(_patient).Reference(p => p.Страховка).Load();
			_db.Entry(_patient).Collection(p => p.Инвалидность).Load();
			_db.Entry(_patient).Reference(p => p.Работа1).Load();

			// Создаем объект с данными
			var editData = new PatientEditData
			{
				// Основные данные
				Фамилия = _patient.Фамилия,
				Имя = _patient.Имя,
				Отчество = _patient.Отчество,
				ДатаРождения = _patient.ДатаРождения,
				Пол = _patient.Пол,
				СНИЛС = _patient.СНИЛС,
				ИНН = _patient.ИНН,
				Телефон = _patient.Телефон,
				СемейноеПоложение = _patient.СемейноеПоложение,
				Образование = _patient.Образование,
				Занятость = _patient.Занятость,

				// Документ
				НаименованиеДокумента = _patient.Документ?.НаименованиеДокумента ?? "Паспорт гражданина РФ",
				Серия = _patient.Документ?.Серия,
				Номер = _patient.Документ?.Номер,
				ДатаВыдачи = _patient.Документ?.ДатаВыдачи ?? DateTime.Now,
				КемВыдан = _patient.Документ?.КемВыдан,

				// Адрес
				Субъект = _patient.Адрес1?.Субъект,
				Район = _patient.Адрес1?.Район,
				Город = _patient.Адрес1?.Город,
				НаселенныйПункт = _patient.Адрес1?.НаселенныйПункт,
				Улица = _patient.Адрес1?.Улица,
				Дом = _patient.Адрес1?.Дом,
				Квартира = _patient.Адрес1?.Квартира,

				// Страховка
				СерияНомер = _patient.Страховка != null ?
					$"{_patient.Страховка.СерияПолиса} {_patient.Страховка.НомерПолиса}" : "",
				НаименованиеСтраховойКомпании = _patient.Страховка?.НаименованиеСтраховойКомпании,

				// Инвалидность (берем первую запись)
				Группа = _patient.Инвалидность.FirstOrDefault()?.Группа,
				Описание = _patient.Инвалидность.FirstOrDefault()?.Описание,
				Дата = _patient.Инвалидность.FirstOrDefault()?.Дата,

				// Работа
				МестоРаботы = _patient.Работа1?.МестоРаботы,
				Должность = _patient.Работа1?.Должность
			};

			DataContext = editData;


			//_db.Entry(_patient).Reference(p => p.Документ).Load();
			//_db.Entry(_patient).Reference(p => p.Адрес1).Load(); // Используем навигационное свойство
			//_db.Entry(_patient).Reference(p => p.Страховка).Load();
			//_db.Entry(_patient).Collection(p => p.Инвалидность).Load();
			//_db.Entry(_patient).Reference(p => p.Работа1).Load(); // Используем навигационное свойство

			//// Получаем связанные объекты
			//_document = _patient.Документ;
			//_address = _patient.Адрес1; // Теперь это объект Адрес
			//_insurance = _patient.Страховка;
			//_disability = _patient.Инвалидность.FirstOrDefault();
			//_work = _patient.Работа1; // Теперь это объект Работа


			// Получаем первую инвалидность (если есть)
			//_disability = _patient.Инвалидность.FirstOrDefault();


			// Создаем объект с данными
			//var editData = new PatientEditData
			//{
			//	Фамилия = _patient.Фамилия,
			//	Имя = _patient.Имя,
			//	Отчество = _patient.Отчество,
			//	ДатаРождения = _patient.ДатаРождения,
			//	СНИЛС = _patient.СНИЛС,
			//	ИНН = _patient.ИНН,
			//	Телефон = _patient.Телефон,
			//	СемейноеПоложение = _patient.СемейноеПоложение,
			//	Образование = _patient.Образование,
			//	Занятость = _patient.Занятость,
			//	Пол = _patient.Пол,

			//	НаименованиеДокумента = _document?.НаименованиеДокумента ?? "Паспорт гражданина РФ",
			//	Серия = _document?.Серия,
			//	Номер = _document?.Номер,
			//	ДатаВыдачи = _document.ДатаВыдачи,
			//	КемВыдан = _document?.КемВыдан,

			//	Субъект = _address?.Субъект,
			//	Район = _address?.Район,
			//	Город = _address?.Город,
			//	НаселенныйПункт = _address?.НаселенныйПункт,
			//	Улица = _address?.Улица,
			//	Дом = _address?.Дом,
			//	Квартира = _address?.Квартира,

			//	СерияНомер = _insurance != null ? $"{_insurance.СерияПолиса} {_insurance.НомерПолиса}" : "",
			//	НаименованиеСтраховойКомпании = _insurance?.НаименованиеСтраховойКомпании,

			//	Группа = _disability?.Группа,
			//	Описание = _disability?.Описание,
			//	Дата = _disability?.Дата,

			//	МестоРаботы = _work?.МестоРаботы,
			//	Должность = _work?.Должность
			//};

			//DataContext = editData;
			LoadPatientHealthData(_patient.НомерПациента);
		}

		private void LoadPatientHealthData(int patientId)
		{
			var healthRecord = _db.КартаПациента.FirstOrDefault(k => k.НомерПациента == patientId);
			if (healthRecord != null)
			{
				// Здесь можно загрузить данные о здоровье, если нужно
			}
		}

		private void Save_Click(object sender, RoutedEventArgs e)
		{
				try
				{
					// 1. Основные данные пациента
					_patient.Фамилия = LastName.Text;
					_patient.Имя = FirstName.Text;
					_patient.Отчество = Patronymic.Text;
					_patient.Пол = Gender.Text;

					if (DateOfBirth.SelectedDate.HasValue)
					{
						_patient.ДатаРождения = DateOfBirth.SelectedDate.Value;
					}
					else
					{
						MessageBox.Show("Пожалуйста, укажите дату рождения", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
						return;
					}

					_patient.СНИЛС = SNILSTB.Text;
					_patient.ИНН = INNTB.Text;
					_patient.Телефон = PhoneTB.Text;
					_patient.СемейноеПоложение = MaritalStatusTB.Text;
					_patient.Образование = EducationTB.Text;
					_patient.Занятость = BusynessTB.Text;

					// 2. Документы
					if (_patient.Документ == null)
					{
						_patient.Документ = new Документ();
						_db.Документ.Add(_patient.Документ);
					}
					_patient.Документ.Серия = DocumentSeriesTextBox.Text;
					_patient.Документ.Номер = DocumentNumberTextBox.Text;
					_patient.Документ.КемВыдан = DocumentIssuedByTextBox.Text;
					_patient.Документ.ДатаВыдачи = DocumentIssueDatePicker.SelectedDate ?? DateTime.Now;

					// 3. Адрес
					if (_patient.Адрес1 == null)
					{
						_patient.Адрес1 = new Адрес();
						_db.Адрес.Add(_patient.Адрес1);
					}
					_patient.Адрес1.Субъект = SubjectTB.Text;
					_patient.Адрес1.Район = DistristTB.Text;
					_patient.Адрес1.Город = TownTB.Text;
					_patient.Адрес1.Улица = StreetTB.Text;
					_patient.Адрес1.Дом = HouseTB.Text;
					_patient.Адрес1.Квартира = ApartmentTB.Text;

					// 4. Страховка
					if (_patient.Страховка == null)
					{
						_patient.Страховка = new Страховка();
						_db.Страховка.Add(_patient.Страховка);
					}
					var policyParts = InsurancePolicyTextBox.Text?.Split(' ') ?? new string[0];
					_patient.Страховка.СерияПолиса = policyParts.Length > 0 ? policyParts[0] : "";
					_patient.Страховка.НомерПолиса = policyParts.Length > 1 ? policyParts[1] : "";
					_patient.Страховка.НаименованиеСтраховойКомпании = InsuranceCompanyTextBox.Text;

					// 5. Инвалидность
					var disability = _patient.Инвалидность.FirstOrDefault();
					if (!string.IsNullOrEmpty(DisabilityGroupTB.Text) || !string.IsNullOrEmpty(DisabilityOfDescriptionTB.Text))
					{
						if (disability == null)
						{
							disability = new Инвалидность
							{
								НомерПациента = _patient.НомерПациента,
								Группа = DisabilityGroupTB.Text,
								Описание = DisabilityOfDescriptionTB.Text,
								Дата = DisabilityDateTB.SelectedDate
							};
							_patient.Инвалидность.Add(disability);
						}
						else
						{
							disability.Группа = DisabilityGroupTB.Text;
							disability.Описание = DisabilityOfDescriptionTB.Text;
							disability.Дата = DisabilityDateTB.SelectedDate;
						}
					}
					else if (disability != null)
					{
						_patient.Инвалидность.Remove(disability);
						_db.Инвалидность.Remove(disability);
					}

					// 6. Работа
					if (!string.IsNullOrEmpty(PlaceOfWorkTB.Text) || !string.IsNullOrEmpty(PostTB.Text))
					{
						if (_patient.Работа1 == null)
						{
							_patient.Работа1 = new Работа
							{
								МестоРаботы = PlaceOfWorkTB.Text,
								Должность = PostTB.Text
							};
							_db.Работа.Add(_patient.Работа1);
						}
						else
						{
							_patient.Работа1.МестоРаботы = PlaceOfWorkTB.Text;
							_patient.Работа1.Должность = PostTB.Text;
						}
					}
					else if (_patient.Работа1 != null)
					{
						_db.Работа.Remove(_patient.Работа1);
						_patient.Работа1 = null;
					}

					_db.SaveChanges();
					MessageBox.Show("Все данные пациента сохранены", "Успех",
								  MessageBoxButton.OK, MessageBoxImage.Information);
					NavigationService?.GoBack();
				}
				catch (Exception ex)
				{
					MessageBox.Show($"Ошибка при сохранении: {ex.Message}\n\n{ex.InnerException?.Message}", "Ошибка",
								  MessageBoxButton.OK, MessageBoxImage.Error);
				}
		}

		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			// Отменяем изменения
			var changedEntries = _db.ChangeTracker.Entries()
				.Where(x => x.State != EntityState.Unchanged).ToList();

			foreach (var entry in changedEntries)
			{
				entry.State = EntityState.Unchanged;
			}

			NavigationService?.GoBack();
		}

		private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			if (!char.IsDigit(e.Text, 0))
				e.Handled = true;
		}
	}
}
