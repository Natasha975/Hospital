using System;
using System.Linq;
using LibraryHospital;

namespace ConsoleApp
{
	internal class Program
	{
		static void Main(string[] args)
		{
			// Строка подключения
			string connectionString = "data source=DESKTOP-SDP8VA5;initial catalog=Больница;" +
									  "integrated security=True;trustservercertificate=True;";

			var patientService = new PatientDataService(connectionString);

			try
			{
				// Общее количество пациентов
				int totalCount = patientService.GetPatientCount();
				Console.WriteLine($"Общее количество пациентов: {totalCount}");

				// Количество пациентов мужского пола
				int maleCount = patientService.GetMalePatientCount();
				Console.WriteLine($"\nКоличество пациентов мужского пола: {maleCount}");

				// Количество пациентов женского пола
				int femaleCount = patientService.GetFemalePatientCount();
				Console.WriteLine($"Количество пациентов женского пола: {femaleCount}");

				//Соотношение пациентов муж пола и пациентов жен пола
				Console.WriteLine($"Соотношение М/Ж: {maleCount}:{femaleCount}");

				//Средний возраст пациентов
				Console.WriteLine($"\nСредний возраст пациентов: {patientService.GetAverageAge():F1} лет");

				// Возрастные группы
				Console.WriteLine("\nРаспределение по возрастным группам:");
				var ageGroups = patientService.GetPatientsByAgeGroups();
				foreach (var group in ageGroups)
				{
					Console.WriteLine($"{group.Key} лет: {group.Value} пациента(ов)");
				}

				// Пациенты, родившиеся в 2008 году
				int year = 2008;
				Console.WriteLine($"\nПациентов, родившихся в {year} году: {patientService.GetPatientsBornInYear(year)}");

				// Именинники месяца
				Console.WriteLine("\nИменинники этого месяца:");
				var birthdayPersons = patientService.GetBirthdayPersonsThisMonth();
				if (birthdayPersons.Any())
				{
					foreach (var person in birthdayPersons)
					{
						Console.WriteLine(person);
					}
				}
				else
				{
					Console.WriteLine("В этом месяце именинников нет");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Произошла ошибка: {ex.Message}");
			}

			Console.ReadLine();
		}
	}
}
