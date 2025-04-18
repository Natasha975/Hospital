using System.Windows;

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

		// Обработчик нажатия кнопки просмотра пациентов
		private void PatientView_Click(object sender, RoutedEventArgs e)
		{
			MainFrame.Navigate(new PatientPage());

			WelcomeText.Visibility = Visibility.Collapsed;
		}

		// Обработчик нажатия кнопки "Назад"
		private void BackButton_Click(object sender, RoutedEventArgs e)
		{
			if (MainFrame.CanGoBack)
			{
				MainFrame.GoBack();
			}
		}

		// Обработчик нажатия кнопки выхода
		private void ExitBt_Click(object sender, RoutedEventArgs e)
		{
			MainWindow mainWindow = new MainWindow();
			mainWindow.ShowDialog();
			this.Close();
        }
    }
}
