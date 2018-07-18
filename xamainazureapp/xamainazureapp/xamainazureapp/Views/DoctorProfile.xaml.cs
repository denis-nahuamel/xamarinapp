using ReactiveUI;
using System;
using System.Reactive.Linq;
using xamainazureapp.Services;
using xamainazureapp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Reactive.Disposables;

namespace xamainazureapp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DoctorProfile : ContentPage
    { 
        #region==============Atributos==============
        public DoctorViewModel Doctor { get; set; }
        public NavigationService navigation;
        #endregion

       
        public DoctorProfile()
        {
            InitializeComponent();
            //services
            navigation = new NavigationService();
            //view models
            //call and load data
            nombre.Text = App.Current.Properties["nombres"].ToString();
            apellidos.Text = App.Current.Properties["apellidos"].ToString();

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.doctorProfile = this;
          
        }
        private async void listPatient(object sender, EventArgs e)
        {
            int idMedico =Convert.ToInt16( App.Current.Properties["idMedico"]);
          await Navigation.PushAsync(  new ListPatient(idMedico));
        }

    }
   

}