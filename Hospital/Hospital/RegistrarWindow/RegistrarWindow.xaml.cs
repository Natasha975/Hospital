using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Hospital.AdminWindow;

namespace Hospital.RegistrarWindow
{
	/// <summary>
	/// Логика взаимодействия для RegistrarWindow.xaml
	/// </summary>
	public partial class RegistrarWindow : Window
	{
		public RegistrarWindow()
		{
			InitializeComponent();
		}

		private void PatientView_Click(object sender, RoutedEventArgs e)
		{
			MainFrame.Navigate(new PatientPage());

			WelcomeText.Visibility = Visibility.Collapsed;
		}
	}
}
