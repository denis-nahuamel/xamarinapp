using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Refit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using xamainazureapp.Models;
using xamainazureapp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace xamainazureapp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DoctorLogin : ContentPage
	{
        public NavigationService navigation;
        public DoctorLogin ()
		{
			InitializeComponent ();
            entrar.Clicked += Entrar_Clicked;
            App.Navigator = this;
            NavigationPage.SetHasNavigationBar(this, false);

        }

        private async void Entrar_Clicked(object sender, EventArgs e)
        {
            cambiarEstado(false);
            try
            {

                if (string.IsNullOrEmpty(user.Text) || string.IsNullOrEmpty(password.Text))
                {
                    await DisplayAlert("Error", "Debe de llenar todos los espacios.", "Ok"); return;
                }

                HttpClient client = new HttpClient();
                JObject oJsonObject = new JObject();
                string sContentType = "application/json";
                oJsonObject.Add("documento", user.Text.ToString());
                oJsonObject.Add("contrasenia", password.Text.ToString());
                HttpResponseMessage response = await client.PostAsync(App.Url + "/api/Doctors/Entrar", new StringContent(oJsonObject.ToString(), Encoding.UTF8, sContentType));

                dynamic respuesta1 = await response.Content.ReadAsStringAsync();
                var respuesta =JsonConvert.DeserializeObject(respuesta1);

                if (respuesta != null)
                {
                    if (response.StatusCode.ToString()!="OK") //si está vacío
                    {
                        cambiarEstado(true);
                        await DisplayAlert("Error", "¡Usuario o contraseña incorrecta!","Ok");
                    }
                    else
                    {

                        //Doctor medico = new Doctor();
                      
                        string nombre = respuesta["Nombre"];
                        string apellidos = respuesta["Apellidos"];
                        int idMedico = respuesta["DoctorID"];
                        App.SetProperties("logueado", "si");
                        App.SetProperties("nombres", nombre);
                        App.SetProperties("apellidos",apellidos);
                        App.SetProperties("idMedico", idMedico);

                       /* App.SetProperties("email", respuesta.Email);
                        App.SetProperties("documento", respuesta.Documento);
                        App.SetProperties("numero",respuesta.Numero);
                        App.SetProperties("fechaNacimiento", respuesta.FechaNacimiento);
                        App.SetProperties("distrito", respuesta.Distrito);
                        App.SetProperties("direccion", respuesta.Direccion);
                        App.SetProperties("sexo",respuesta.Sexo);
                        App.SetProperties("codigo", respuesta.Codigo);
                        App.SetProperties("registro",respuesta.Registro);*/


                        // App.Current.MainPage = new DoctorProfile(); // Navegacion a la pagina usuario
                        await Navigation.PushAsync(new DoctorProfile());

                    }
                }
                else
                {
                    cambiarEstado(true);
                    await DisplayAlert("Iniciar Sesión", "Error de respuesta del servicio, Contáctese con el administrador","Ok");
                }
            }
            catch (Exception ex)
            {
                cambiarEstado(true);
                await DisplayAlert("Iniciar Sesión", "Error de conexión, inténtelo nuevamente","Ok");
            }
            finally
            {
                cambiarEstado(true);
                entrar.IsEnabled = true;
            }
           
        }
        public void cambiarEstado(bool estado)
        {
            user.IsEnabled = estado;
            password.IsEnabled = estado;
            entrar.IsEnabled = estado;
            if (estado == true)
            {
                IsRunning = false;
            }
            else
            {
                IsRunning = true;
            }
        }
        public bool IsRunning
        {
            set
            {
                if (isRunning != value)
                {
                    isRunning = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRunning"));
                }
            }
            get
            {
                return isRunning;
            }
        }
        private bool isRunning;
        new public event PropertyChangedEventHandler PropertyChanged;
    }
}