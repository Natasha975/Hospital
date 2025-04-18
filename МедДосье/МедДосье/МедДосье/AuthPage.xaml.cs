using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace МедДосье
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AuthPage : ContentPage
	{
		public AuthPage ()
		{
			InitializeComponent ();
		}

		private async void OnLoginClicked(object sender, EventArgs e)
		{
			// Логика входа
			var email = EmailEntry.Text;
			var password = PasswordEntry.Text;

			var user = await App.UserDatabase.GetUserByEmailAsync(email);

			if (user != null && user.Password == password)
			{
				await Navigation.PushAsync(new TelemedicinePage(user));
			}
			else
			{
				await DisplayAlert("Ошибка", "Неверный email или пароль", "OK");
			}
		}

		private async void OnRegisterClicked(object sender, EventArgs e)
		{
			// Переход на страницу регистрации
			await Navigation.PushAsync(new ProfilePage());
		}

		private async void OnForgotPasswordTapped(object sender, EventArgs e)
		{
			// Восстановление пароля
			await DisplayAlert("Восстановление", "Функция восстановления пароля", "OK");
		}

		private async void OnAgreementTapped(object sender, EventArgs e)
		{
			// Показать соглашение
			await DisplayAlert("Соглашение", "Текст пользовательского соглашения...", "Понятно");
		}

		private async void OnClearDatabaseClicked(object sender, EventArgs e)
		{
			bool answer = await DisplayAlert("Подтверждение",
										   "Вы действительно хотите очистить базу данных?",
										   "Да", "Нет");
			if (answer)
			{
				await App.UserDatabase.ClearAllUsersAsync();
				await DisplayAlert("Успех", "База данных очищена", "OK");
			}
		}
	}
}