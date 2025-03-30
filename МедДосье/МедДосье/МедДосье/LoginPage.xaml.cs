using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace МедДосье
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
		public LoginPage()
		{
			InitializeComponent();
		}

		private async void OnLoginClicked(object sender, EventArgs e)
		{
			var login = LoginEntry.Text;
			var password = PasswordEntry.Text;

			if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
			{
				ErrorLabel.Text = "Логин и пароль обязательны";
				ErrorLabel.IsVisible = true;
				return;
			}

			var patient = await App.Database.GetPatientByCredentialsAsync(login, password);

			if (patient != null)
			{
				// Успешная авторизация
				await Navigation.PushAsync(new MainPage(patient));
			}
			else
			{
				ErrorLabel.Text = "Неверный логин или пароль";
				ErrorLabel.IsVisible = true;
			}
		}
	}
}