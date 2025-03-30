using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace МедДосье
{
	public partial class App : Application
	{
		public const string DATABASE_NAME = "patients.db";
		private static PatientRepository database;

		public static PatientRepository Database
		{
			get
			{
				if (database == null)
				{
					database = new PatientRepository(
						Path.Combine(
							Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
							DATABASE_NAME));
				}
				return database;
			}
		}

		public App()
		{
			InitializeComponent();

			MainPage = new NavigationPage(new LoginPage());
		}

		protected override async void OnStart()
		{
			// Создаем таблицу при старте приложения
			await Database.CreateTableAsync();

			// Список тестовых пациентов
			var testPatients = new List<Patient>
				{
					new Patient
					{
						LastName = "Иванов",
						FirstName = "Иван",
						MiddleName = "Иванович",
						Login = "iv",  // Логин 1
						Password = "12",  // Пароль 1
						DocumentName ="Паспорт",
						Series = "1234",
						Number = "123456",
						IssuedBy = "SDFSG",
						OfTheIssueDate = new DateTime(2010, 5, 15),
						Gender = "M",
						SNILS = "123-456-789 00",
						INN = "123456789012",
						Phone = "+7 (123) 456-78-90",
						MaritalStatus = "Женат",
						Education = "Высшее",
						Employment = "Работает",
					},
					new Patient
					{
						LastName = "Петрова",
						FirstName = "Мария",
						MiddleName = "Сергеевна",
						Login = "petrova",  // Логин 2
						Password = "34",  // Пароль 2
						DocumentName ="Паспорт",
						Series = "1234",
						Number = "123456",
						IssuedBy = "SDFSG",
						OfTheIssueDate = new DateTime(2010, 5, 15),
						Gender = "F",
						SNILS = "987-654-321 00",
						INN = "987654321098",
						Phone = "+7 (987) 654-32-10",
						MaritalStatus = "Замужем",
						Education = "Среднее специальное",
						Employment = "В декрете",
					},
					new Patient
					{
						LastName = "Сидоров",
						FirstName = "Алексей",
						MiddleName = "Николаевич",
						Login = "sidor",  // Логин 3
						Password = "56",  // Пароль 3
						DocumentName ="Паспорт",
						Series = "1234",
						Number = "123456",
						IssuedBy = "SDFSG",
						OfTheIssueDate = new DateTime(2010, 5, 15),
						Gender = "M",
						SNILS = "456-123-789 00",
						INN = "456123789045",
						Phone = "+7 (456) 123-78-90",
						MaritalStatus = "Холост",
						Education = "Среднее",
						Employment = "Безработный",
					}
				};

			// Добавляем всех тестовых пациентов, если их еще нет в базе
			foreach (var patient in testPatients)
			{
				var existingPatient = await Database.GetPatientByCredentialsAsync(patient.Login, patient.Password);
				if (existingPatient == null)
				{
					await Database.SavePatientAsync(patient);
				}
			}
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}
	}
}