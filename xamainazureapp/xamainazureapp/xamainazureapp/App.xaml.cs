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
        public static DoctorProfile doctorProfile { get; internal set; }
        public static ListPatient listPatient { get; internal set; }
        public static ListReport listReport { get; internal set; }
        public static int idMedico;
        
        public App ()
		{
			InitializeComponent();
            //Xamarin.Forms.Application.Current.Properties.Clear();
            try {
                if (Application.Current.Properties.ContainsKey("logueado"))//si existe
                    MainPage =new NavigationPage( new DoctorProfile());//entra al perfil del medico
                else
                    MainPage = new NavigationPage(new DoctorLogin());//lo redirije al login
            } catch (Exception ex)
            {
                MainPage = new NavigationPage(new DoctorLogin());//cualquier error, lo manda al login
            }
            var baseStyle = new Style(typeof(Frame))
            {
                Setters = {
                new Setter { Property = Frame.BackgroundColorProperty, Value =new Color(238, 220, 220 , 0.5)    }

            }
            };


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
        public async static void SetProperties(string property, object value)
        {
            var app = (App)Application.Current;
            app.Properties[property] = value;
            await app.SavePropertiesAsync();
        }
       
    }
}
