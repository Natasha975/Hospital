using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using МедДосье.Model;
using МедДосье.ViewModel;

namespace МедДосье
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TelemedicinePage : ContentPage
	{
		private User _currentUser;

		public TelemedicinePage(User user)
		{
			InitializeComponent();
			LoadDataAsync();

			_currentUser = user;
		}

		private async void LoadDataAsync()
		{
			try
			{
				var specialities = new ObservableCollection<Speciality>
				{
					new Speciality { Name = "Терапевт", Price = "от 1 299 ₽" },
					new Speciality { Name = "Невролог", Price = "от 1 299 ₽" },
					new Speciality { Name = "Хирург", Price = "от 1 299 ₽" },
					new Speciality { Name = "Педиатр", Price = "от 1 299 ₽" },
					new Speciality { Name = "Кардиолог", Price = "от 1 299 ₽" },
					new Speciality { Name = "Гастроэнтеролог", Price = "от 1 299 ₽" },
				};

				BindingContext = new TelemedicinePageViewModel(specialities);
			}
			catch (Exception ex)
			{
				await DisplayAlert("Ошибка", "Не удалось загрузить данные", "OK");
				Console.WriteLine(ex);
			}
		}

		private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.CurrentSelection.Count != 0)
			{
				Speciality selectedSpeciality = (Speciality)e.CurrentSelection[0];
				await Navigation.PushAsync(new DoctorListPage(selectedSpeciality));
				((CollectionView)sender).SelectedItem = null;
			}
		}

		private async void OnProfileClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new UserProfilePage(_currentUser));
		}
	}	
}