using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryHospital
{
	public class PatientDataService
	{
		private readonly string _connectionString;

		public PatientDataService(string connectionString)
		{
			_connectionString = connectionString;
		}

		// Метод для получения количества пациентов
		public int GetPatientCount()
		{
			int count = 0;

			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();

				string query = "SELECT COUNT(*) FROM dbo.Пациент";

				using (SqlCommand command = new SqlCommand(query, connection))
				{
					count = (int)command.ExecuteScalar();
				}
			}

			return count;
		}

		// Метод для получения количества пациентов мужского пола
		public int GetMalePatientCount()
		{
			int count = 0;

			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				string query = "SELECT COUNT(*) FROM dbo.Пациент WHERE Пол = 'М'";

				using (SqlCommand command = new SqlCommand(query, connection))
				{
					count = (int)command.ExecuteScalar();
				}
			}

			return count;
		}

		// Метод для получения количества пациентов женского пола
		public int GetFemalePatientCount()
		{
			int count = 0;

			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				string query = "SELECT COUNT(*) FROM dbo.Пациент WHERE Пол = 'Ж'";

				using (SqlCommand command = new SqlCommand(query, connection))
				{
					count = (int)command.ExecuteScalar();
				}
			}

			return count;
		}

		// Метод для получения среднего возраста пациентов
		public double GetAverageAge()
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				string query = @"SELECT AVG(DATEDIFF(YEAR, ДатаРождения, GETDATE())) 
                               FROM dbo.Пациент";

				using (SqlCommand command = new SqlCommand(query, connection))
				{
					return Convert.ToDouble(command.ExecuteScalar());
				}
			}
		}

		// Метод для получения количества пациентов по возрастным группам
		public Dictionary<string, int> GetPatientsByAgeGroups()
		{
			var ageGroups = new Dictionary<string, int>
			{
				{"0-18", 0},
				{"19-30", 0},
				{"31-45", 0},
				{"46-60", 0},
				{"60+", 0}
			};

			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				string query = @"SELECT 
                    CASE 
                        WHEN DATEDIFF(YEAR, ДатаРождения, GETDATE()) BETWEEN 0 AND 18 THEN '0-18'
                        WHEN DATEDIFF(YEAR, ДатаРождения, GETDATE()) BETWEEN 19 AND 30 THEN '19-30'
                        WHEN DATEDIFF(YEAR, ДатаРождения, GETDATE()) BETWEEN 31 AND 45 THEN '31-45'
                        WHEN DATEDIFF(YEAR, ДатаРождения, GETDATE()) BETWEEN 46 AND 60 THEN '46-60'
                        ELSE '60+'
                    END AS AgeGroup,
                    COUNT(*) AS Count
                FROM dbo.Пациент
                GROUP BY 
                    CASE 
                        WHEN DATEDIFF(YEAR, ДатаРождения, GETDATE()) BETWEEN 0 AND 18 THEN '0-18'
                        WHEN DATEDIFF(YEAR, ДатаРождения, GETDATE()) BETWEEN 19 AND 30 THEN '19-30'
                        WHEN DATEDIFF(YEAR, ДатаРождения, GETDATE()) BETWEEN 31 AND 45 THEN '31-45'
                        WHEN DATEDIFF(YEAR, ДатаРождения, GETDATE()) BETWEEN 46 AND 60 THEN '46-60'
                        ELSE '60+'
                    END";

				using (SqlCommand command = new SqlCommand(query, connection))
				using (SqlDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						ageGroups[reader["AgeGroup"].ToString()] = Convert.ToInt32(reader["Count"]);
					}
				}
			}

			return ageGroups;
		}

		// Метод для поиск пациентов, родившихся в определенный год
		public int GetPatientsBornInYear(int year)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				string query = @"SELECT COUNT(*) 
                               FROM dbo.Пациент 
                               WHERE YEAR(ДатаРождения) = @Year";

				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.AddWithValue("@Year", year);
					return (int)command.ExecuteScalar();
				}
			}
		}

		// Метод для получения списка именинников текущего месяца
		public List<string> GetBirthdayPersonsThisMonth()
		{
			var birthdayPersons = new List<string>();

			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				string query = @"SELECT Фамилия, Имя, Отчество 
                                FROM dbo.Пациент 
                                WHERE MONTH(ДатаРождения) = MONTH(GETDATE())";

				using (SqlCommand command = new SqlCommand(query, connection))
				using (SqlDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						// Отдельные компоненты ФИО
						string lastName = reader["Фамилия"]?.ToString() ?? string.Empty;
						string firstName = reader["Имя"]?.ToString() ?? string.Empty;
						string middleName = reader["Отчество"]?.ToString() ?? string.Empty;

						// Полное ФИО
						string fullName = $"{lastName} {firstName} {middleName}".Trim();

						if (!string.IsNullOrWhiteSpace(fullName))
						{
							birthdayPersons.Add(fullName);
						}
					}
				}
			}

			return birthdayPersons;
		}
	}
}
