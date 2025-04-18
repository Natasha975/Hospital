using Hospital.Model;
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

namespace Hospital.AdminWindow
{
	/// <summary>
	/// Логика взаимодействия для AdminWindow.xaml
	/// </summary>
	public partial class AdminWindow : Window
	{
		private БольницаEntities _db = new БольницаEntities();

		public AdminWindow()
		{
			InitializeComponent();
		}

		// Обработчик события закрытия окна
		protected override void OnClosed(EventArgs e)
		{
			_db.Dispose();
			base.OnClosed(e);
		}

		// Обработчик нажатия кнопки просмотра пользователей
		private void UsersView_Click(object sender, RoutedEventArgs e)
		{
			MainFrame.Navigate(new UsersPage());

			WelcomeText.Visibility = Visibility.Collapsed;
		}

		// Обработчик нажатия кнопки выхода
		private void ExitBt_Click(object sender, RoutedEventArgs e)
		{
			MainWindow mainWindow = new MainWindow();
			mainWindow.ShowDialog();
			this.Hide();
		}

		// Обработчик нажатия кнопки просмотра врачей
		private void DoctorsView_Click(object sender, RoutedEventArgs e)
		{
			MainFrame.Navigate(new DoctorPage());

			WelcomeText.Visibility = Visibility.Collapsed;
		}
	}
}