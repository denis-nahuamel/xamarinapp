using ReactiveUI;
using Refit;
using System;
using System.ComponentModel;
using xamainazureapp.Models;
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
        string Contrasenia = "";
        public PersonalInformation(int id)
        {
            InitializeComponent();

            
            try
            {
                patientInformation();
                
            }
            catch (Exception ex)
            { }
            btnEditar.Clicked += BtnEditar_Clicked;  
           
        }

        private async void BtnEditar_Clicked(object sender, EventArgs e)
        {
            Doctor doctor = new Doctor()
            {
                Nombre = txtNombre.Text,
                Apellidos = txtApellidos.Text,
                Email = txtEmail.Text,
                DoctorID= Convert.ToInt16(App.Current.Properties["idMedico"]),
                Documento = txtDocumento.Text,
                FechaNacimiento = txtFechaNacimiento.Text,
                Direccion = txtDireccion.Text,
                Distrito = txtDistrito.Text,
                Codigo = txtCodigo.Text,
                Numero = txtNumero.Text,
                Registro = txtRegistro.Text,
                Contrasenia=Contrasenia,
                Sexo = rbVaron.Checked,
            };
            if (string.IsNullOrEmpty(txtAContra.Text))
            {
                try
                {
                    var apiResponse = RestService.For<IComment>(App.Url);
                    dynamic respuesta = await apiResponse.PutDoctor(Convert.ToInt16(App.Current.Properties["idMedico"]), doctor);
                    if()
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", "Ocurrió un error al actualizar datos.", "Ok");
                }
            }
            else {
                if (txtAContra.Text == Contrasenia)
                {
                    if (txtNContra.Text == txtNContraR.Text)
                    {
                        doctor.Contrasenia = txtNContraR.Text;
                        try
                        {
                            var apiResponse = RestService.For<IComment>(App.Url);
                            dynamic respuesta = await apiResponse.PutDoctor(Convert.ToInt16(App.Current.Properties["idMedico"]), doctor);
                        }
                        catch (Exception ex)
                        {
                            await DisplayAlert("Error", "Ocurrió un error al actualizar datos.", "Ok");
                        }
                    }
                    else
                        await DisplayAlert("Editar Datos", "Los campos de contraseña nueva no coinciden", "Ok");
                }
                else
                {
                    await DisplayAlert("Editar Datos", "La contraseña actual no es correcta.", "Ok");
                }
            }
            

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
                    txtCodigo.Text= response.Codigo.ToString();
                    txtRegistro.Text= response.Registro.ToString();
                    txtNumero.Text = response.Numero.ToString();
                    Contrasenia = response.Contrasenia.ToString();
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