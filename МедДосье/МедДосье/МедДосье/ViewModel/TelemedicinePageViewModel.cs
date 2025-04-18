using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using МедДосье.Model;

namespace МедДосье.ViewModel
{
	public class TelemedicinePageViewModel
	{
		public ObservableCollection<Speciality> Specialities { get; set; }

		public TelemedicinePageViewModel(ObservableCollection<Speciality> specialities)
		{
			Specialities = specialities;
		}
	}
}
