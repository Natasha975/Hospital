using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace МедДосье.Model
{
	public class UserRepository
	{
		private readonly SQLiteAsyncConnection _database;

		public UserRepository(string dbPath)
		{
			_database = new SQLiteAsyncConnection(dbPath);
			InitializeAsync();
		}

		private async void InitializeAsync()
		{
			await _database.CreateTableAsync<User>();
		}

		public Task<List<User>> GetUsersAsync()
		{
			return _database.Table<User>().ToListAsync();
		}

		public Task<int> SaveUserAsync(User user)
		{
			return _database.InsertAsync(user);
		}

		public async Task<User> GetUserByEmailAsync(string email)
		{
			try
			{
				return await _database.Table<User>()
									 .Where(u => u.Email ==  email)
									 .FirstOrDefaultAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Database error: {ex}");
				return null;
			}
		}

		public async Task ClearAllUsersAsync()
		{
			await _database.DeleteAllAsync<User>();
		}

		public async Task SetCurrentUserAsync(User user)
		{
			// Сбрасываем текущий статус у всех пользователей
			await _database.ExecuteAsync("UPDATE User SET IsCurrent = 0");

			// Устанавливаем текущего пользователя
			user.IsCurrent = true;
			await _database.UpdateAsync(user);
		}

		public async Task<User> GetCurrentUserAsync()
		{
			return await _database.Table<User>()
								 .Where(u => u.IsCurrent)
								 .FirstOrDefaultAsync();
		}
	}
}