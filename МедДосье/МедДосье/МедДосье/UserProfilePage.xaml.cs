using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using МедДосье.Model;
using МедДосье.ViewModel;

namespace МедДосье
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UserProfilePage : ContentPage
	{
		public UserProfilePage (User user)
		{
			InitializeComponent ();

			// Пользователь не null
			if (user == null)
			{
				DisplayAlert("Ошибка", "Данные пользователя не загружены", "OK");
				Navigation.PopAsync();
				return;
			}

			BindingContext = new UserProfileViewModel(user);

			// Заголовок с именем пользователя
			Title = $"Профиль {user.FullName}";
		}

		private void OnEditProfileClicked(object sender, EventArgs e)
		{
			DisplayAlert("Предупреждение", "Кнопка для редактирования пользователя", "OK");
		}

		private void OnConsultationHistoryClicked(object sender, EventArgs e)
		{
			DisplayAlert("Предупреждение", "Кнопка для просмотра консультации", "OK");
		}

		private async void OnLogoutClicked(object sender, EventArgs e)
		{
			bool answer = await DisplayAlert("Подтверждение", "Вы действительно хотите выйти из аккаунта?", "Да", "Нет");
			if (answer)
			{
				await Navigation.PopToRootAsync();
			}
		}
	}
}