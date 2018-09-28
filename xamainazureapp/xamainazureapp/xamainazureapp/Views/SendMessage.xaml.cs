using Newtonsoft.Json.Linq;
using Refit;
using System;
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
	public partial class SendMessage : ContentPage
	{
        HttpClient _client = new HttpClient();
        int IdReport;
        public SendMessage (bool commentExist, int idReport,Comment comment)
		{
			InitializeComponent ();
            if(commentExist==true)
            {
                btnEnviar.IsEnabled = false;
                lblMensaje.Text = comment.Contenido;
            }
            IdReport = idReport;
		}
        protected override void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Subscribe<object, string>(this, App.NotificationReceivedKey, OnMessageReceived);
            btnEnviar.Clicked += OnBtnSendClicked;
        }

        void OnMessageReceived(object sender, string msg)
        {
            Device.BeginInvokeOnMainThread(() => {
                // lblMsg.Text = msg;
                DisplayAlert("",msg, "Ok");
            });
        }

        async void OnBtnSendClicked(object sender, EventArgs e)
        {
            //enviar notificacion push
            btnEnviar.IsEnabled = false;
            var url= App.Url + "/api/Doctors/SendNotificationAsync";
            var content = new StringContent("\"" + edMensaje.Text + "\"", Encoding.UTF8, "application/json");
            var result = await _client.PostAsync(url, content);
            //insertar comentario en la bd
            var client = new HttpClient();
            string time = DateTime.Now.ToString("dd/MM/yyyy");
            Comment comment = new Comment()
            {
                Contenido = edMensaje.Text,
                ReporteID = IdReport,
                Fecha = time
            };
            await postmessage(comment);//envia comentario sobre el reporte
            await DisplayAlert("Mensaje","El comentario fué enviado.","Ok");
        }
        async Task postmessage(Comment comment)//envia el comentario a la bd
        {
            try
            {
                var apiResponse = RestService.For<IComment>(App.Url);
                await apiResponse.PostComment(comment);
                lblMensaje.Text = edMensaje.Text;
                btnEnviar.IsEnabled = false;
            }
            catch(Exception ex) {
                await DisplayAlert("Error", "Ocurrió un error al enviar el mensaje.", "Ok");
            }
            
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<object>(this, App.NotificationReceivedKey);
        }
    }
    
}