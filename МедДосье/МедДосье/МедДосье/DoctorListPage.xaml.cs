using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using МедДосье.Model;
using МедДосье.ViewModel;

namespace МедДосье
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DoctorListPage : ContentPage
	{
		public DoctorListPage(Speciality selectedSpeciality)
		{
			InitializeComponent();
			BindingContext = new DoctorListViewModel(selectedSpeciality, this);
		}		
	}
}