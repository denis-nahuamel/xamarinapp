using GalaSoft.MvvmLight.Command;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using xamainazureapp.Models;
using xamainazureapp.Services;
using xamainazureapp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
            Doctor = new DoctorViewModel();
            //call and load data
            var holi = CallFlowersApi();
            

        }
        async Task CallFlowersApi()
        {
            var apiResponse = RestService.For<IDoctorApi>("http://backendazure420180627123231.azurewebsites.net/");
            var doctorApi = await apiResponse.GetDoctor(1);
            MostrarDoctor(doctorApi);
        }

        /* private void ReloadFlowers(List<Flower> flowersapi)
         {
             flowers.Clear();
             foreach (var flower in flowersapi)
             {
                 flowers.Add(new FlowerViewModel {
                     description = flower.description,
                     idFlower=flower.idFlower,
                     price=flower.price
                 });
             }
         }*/
        private void MostrarDoctor(Doctor doctorApi)
        {
            nombreUsuario.Text = "carayy";
        }
        #region=============Commands============
        public ICommand AddFlowerCommand { get { return new RelayCommand(AddFlower); } }

        private async void AddFlower()
        {
            await navigation.Navigate("NewFlower");
        }
        #endregion
    }
   

}