using System;
using xamainazureapp.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace xamainazureapp
{
	public partial class App : Application
	{
        public static string Url = "http://backendazure420180627123231.azurewebsites.net";
        public static DoctorLogin Navigator { get; internal set; }
        public App ()
		{
			InitializeComponent();
            if (!App.Current.Properties.ContainsKey("contrasenia")  && !App.Current.Properties.ContainsKey("usuario"))
                MainPage =new  DoctorProfile();
            else
			    MainPage = new DoctorLogin();
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
