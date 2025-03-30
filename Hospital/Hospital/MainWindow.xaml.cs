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
					var quer = db.Пользователи.Include(p => p.Роль1).FirstOrDefault(p => p.Логин == LoginTB.Text && p.Пароль == PasswordBx.Password);

					if (quer != null)
					{
						this.Hide();

						Window windowToOpen;

						switch(quer.Роль1.Ниаменование)
						{
							case "Администратор":
								windowToOpen = new Window();
								break;

							case "Врач":
								windowToOpen = new Window();
								break;

							case "Медесестра":
								windowToOpen = new Window();
								break;

							default:
								MessageBox.Show("Ошибка");
								this.Show();
								return;
						}

						windowToOpen.Show();

						this.Close();
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
