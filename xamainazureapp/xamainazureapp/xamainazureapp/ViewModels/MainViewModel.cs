using GalaSoft.MvvmLight.Command;
using Refit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using xamainazureapp.Models;
using xamainazureapp.Services;

namespace xamainazureapp.ViewModels
{
    public class MainViewModel
    {
        #region==============Atributos==============
        public DoctorViewModel Doctor {get; set;}
        public string nombre { get; set; }
        public NavigationService navigation;
        #endregion
        public MainViewModel()
        {  
            //services
            navigation = new NavigationService();
            //view models
            Doctor = new DoctorViewModel();
            //call and load data
           var holi= CallFlowersApi();
           
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
                nombre = doctorApi.Nombre;
                Doctor.Apellidos = doctorApi.Apellidos;
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
