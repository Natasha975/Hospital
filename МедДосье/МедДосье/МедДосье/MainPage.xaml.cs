using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace МедДосье
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : ContentPage
	{
		private Patient currentPatient;
		public MainPage(Patient patient)
		{
			InitializeComponent();
			currentPatient = patient;
			BindingContext = currentPatient;
		}

		private async void OnSaveClicked(object sender, EventArgs e)
		{
			// Сохраняем изменения
			await App.Database.SavePatientAsync(currentPatient);
			await DisplayAlert("Успешно", "Данные сохранены", "OK");
		}
	}
}
