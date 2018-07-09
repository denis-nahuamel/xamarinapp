using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xamainazureapp.Views;

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
                case "DoctorProfile":
                    await App.Current.MainPage.Navigation.PushAsync(new DoctorProfile());
                    break;
            }
               
        }
        public async Task Back()
        {
            await App.Current.MainPage.Navigation.PopAsync();
        }
    }
}
