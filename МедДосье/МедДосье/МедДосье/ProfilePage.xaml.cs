using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using МедДосье.Model;

namespace МедДосье
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfilePage : ContentPage
	{
		public ProfilePage()
		{
			InitializeComponent();
		}

		private async void Next_Clicked(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(LastNameEntry.Text) || string.IsNullOrEmpty(FirstNameEntry.Text) ||
				string.IsNullOrEmpty(EmailEntry.Text) || string.IsNullOrEmpty(PasswordEntry.Text))
			{
				await DisplayAlert("Ошибка", "Заполните обязательные поля", "OK");
				return;
			}

			// Проверяем, не зарегистрирован ли уже пользователь с таким email
			var existingUser = await App.UserDatabase.GetUserByEmailAsync(EmailEntry.Text);
			if (existingUser != null)
			{
				await DisplayAlert("Ошибка", "Пользователь с таким email уже существует", "OK");
				return;
			}

			var newUser = new User
			{
				LastName = LastNameEntry.Text,
				FirstName = FirstNameEntry.Text,
				MiddleName = MiddleNameEntry.Text,
				Email = EmailEntry.Text,
				Password = PasswordEntry.Text,
				Gender = MaleRadioButton.IsChecked ? "Мужской" : "Женский",
				BirthDate = BirthDatePicker.Date
			};

			await App.UserDatabase.SaveUserAsync(newUser);

			// Устанавливаем как текущего пользователя
			await App.UserDatabase.SetCurrentUserAsync(newUser);

			// Переходим на страницу профиля, передавая пользователя
			await Navigation.PushAsync(new TelemedicinePage(newUser));
		}
	}
}