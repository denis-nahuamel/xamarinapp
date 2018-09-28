using Akavache;
using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using xamainazureapp.Models;
using xamainazureapp.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace xamainazureapp
{
	public partial class App : Application
	{
        public const string NotificationReceivedKey = "NotificationReceived";
        // public const string MobileServiceUrl = "http://xamarinpushnotifhubbackend.azurewebsites.net";
        public const string MobileServiceUrl = "http://backendazure420180627123231.azurewebsites.net";
        public static string Url = "http://backendazure420180627123231.azurewebsites.net";
        public static int idPatient;
        public static String IdNotificationPatient;
        public static DoctorLogin Navigator { get; internal set; }
        public static DoctorProfile doctorProfile { get; internal set; }
        public static ListPatient listPatient { get; internal set; }
        public static ListReport listReport { get; internal set; }
        public static int idMedico;
        public static Doctor doctorInformation { get; internal set; }

        public App (bool shallNavigate)
		{
			InitializeComponent();
            //Xamarin.Forms.Application.Current.Properties.Clear();
            if (shallNavigate == false)
            {
                try
                {
                    if (Application.Current.Properties.ContainsKey("logueado"))//si existe
                        MainPage = new NavigationPage(new DoctorProfile()) { BarTextColor = Color.FromHex("#5B4242") };//entra al perfil del medico
                    else
                        MainPage = new NavigationPage(new DoctorLogin());//lo redirije al login
                }
                catch (Exception ex)
                {
                    MainPage = new NavigationPage(new DoctorLogin());//cualquier error, lo manda al login
                }

            }
            else
            {
                Patient patient = new Patient();
                patient.PacienteID = Convert.ToInt16(IdNotificationPatient);
                MainPage = new NavigationPage(new ListReport(patient));
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
            var caches = new[]
           {
                BlobCache.LocalMachine,
                BlobCache.Secure,
                BlobCache.UserAccount,
                BlobCache.InMemory
            };

            caches.Select(x => x.Flush()).Merge().Select(_ => Unit.Default).Wait();
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
