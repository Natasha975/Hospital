using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using МедДосье.Model;

namespace МедДосье
{
	public partial class App : Application
	{
		public static UserRepository UserDatabase { get; private set; }

		public App()
		{
			InitializeComponent();

			var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "users.db");
			UserDatabase = new UserRepository(dbPath);


			MainPage = new NavigationPage(new AuthPage());
		}

		protected override void OnStart()
		{
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}
	}
}
