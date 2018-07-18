using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xamainazureapp.Views;
using xamainazureapp.Views.Patient;

namespace xamainazureapp.Services
{
    public class NavigationService
    { 
        public async Task Navigate(string PageName)
        {
            switch(PageName)
            {
                case "NewFlower":
                    await App.Current.MainPage.Navigation.PushAsync(new NewFlower());
                    break;
                case "AddPatient":
                    await App.Current.MainPage.Navigation.PushAsync(new AddPatient());
                    break;
            }
               
        }
        public async Task Back()
        {
            await App.Current.MainPage.Navigation.PopAsync();
        }
    }
}
