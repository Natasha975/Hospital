using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace МедДосье
{
	public class PatientRepository
	{
		private readonly SQLiteAsyncConnection database;

		public PatientRepository(string databasePath)
		{
			database = new SQLiteAsyncConnection(databasePath);
		}

		public async Task CreateTableAsync()
		{
			await database.CreateTableAsync<Patient>();
		}

		public async Task<List<Patient>> GetPatientsAsync()
		{
			return await database.Table<Patient>().ToListAsync();
		}

		public async Task<Patient> GetPatientAsync(int id)
		{
			return await database.GetAsync<Patient>(id);
		}

		public async Task<Patient> GetPatientByCredentialsAsync(string login, string password)
		{
			return await database.Table<Patient>()
				.Where(p => p.Login == login && p.Password == password)
				.FirstOrDefaultAsync();
		}

		public async Task<int> SavePatientAsync(Patient patient)
		{
			if (patient.Id != 0)
			{
				await database.UpdateAsync(patient);
				return patient.Id;
			}
			else
			{
				return await database.InsertAsync(patient);
			}
		}

		public async Task<int> DeletePatientAsync(Patient patient)
		{
			return await database.DeleteAsync(patient);
		}
	}
}
