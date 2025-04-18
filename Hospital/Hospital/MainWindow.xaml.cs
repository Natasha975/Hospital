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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Entity;
using Hospital.AdminWindow;
using Hospital.RegistrarWindow;
using System.Windows.Media.Animation;

namespace Hospital
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void Vxod_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				using (var db = new БольницаEntities())
				{
					var quer = db.Пользователи.FirstOrDefault(p => p.Логин == LoginTB.Text && p.Пароль == PasswordBx.Password);

					if (quer != null)
					{
						if (quer.Роль1.Ниаменование == "Администратор")
						{
							AdminWindow.AdminWindow adminWindow = new AdminWindow.AdminWindow();
							adminWindow.Show();
							this.Close();
						}
						else if (quer.Роль1.Ниаменование == "Регистратор")
						{
							RegistrarWindow.RegistrarWindow regWindow = new RegistrarWindow.RegistrarWindow();
							regWindow.Show();
							this.Close();
						}
						else
						{
							MessageBox.Show("Ошибка");
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
	}
}