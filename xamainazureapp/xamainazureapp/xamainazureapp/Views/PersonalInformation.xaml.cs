using ReactiveUI;
using Refit;
using System;
using System.ComponentModel;
using xamainazureapp.Services;
using xamainazureapp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace xamainazureapp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PersonalInformation : ContentPage
    {
        bool Varon=false,Mujer=false;
        
        public PersonalInformation(int id)
        {
            InitializeComponent();

            
            try
            {
                patientInformation();
                
            }
            catch (Exception ex)
            { }
                
           
        }
        public async void patientInformation()
        {
            try
            {
                var apiResponse = RestService.For<IComment>(App.Url);
                var response = await apiResponse.GetDoctor(Convert.ToInt16(App.Current.Properties["idMedico"]));
                if (response != null)
                {
                    txtNombre.Text = response.Nombre.ToString();
                    txtApellidos.Text = response.Apellidos.ToString();
                    txtEmail.Text = response.Email.ToString();
                    txtDocumento.Text = response.Documento.ToString();
                    txtFechaNacimiento.Text = response.FechaNacimiento.ToString();
                    txtDireccion.Text = response.Direccion.ToString();
                    txtDistrito.Text = response.Distrito.ToString();

                    if (Convert.ToBoolean(response.Sexo) == true)
                        Varon = true;
                    else
                        Mujer = true;
                    BindingContext = new Personal()
                    {
                        varon = Varon,
                        _varon = Varon,
                        mujer = Mujer,
                        _mujer = Mujer
                    };

                }
                else
                {
                    //cambiarEstado(true);
                    await DisplayAlert("Error", "Error de respuesta del servicio, Contáctese con el administrador", "Ok");
                    // return null;
                }
            }
            catch (Exception ex)
            {
                //cambiarEstado(true);
                await DisplayAlert("Error", "Error de conexión, inténtelo nuevamente", "Ok");
                //return null;
            }

        }
    }
       
}