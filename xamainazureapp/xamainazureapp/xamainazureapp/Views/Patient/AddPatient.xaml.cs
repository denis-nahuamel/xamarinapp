using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace xamainazureapp.Views.Patient
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddPatient : ContentPage
	{
		public AddPatient ()
		{
			InitializeComponent ();
            ansPicker.ItemsSource = new[]
          {
               "varon",
               "mujer",
           };
            ansPicker.Spacing = 0;
            ansPicker.Items[0].WidthRequest = 80;
            ansPicker.Items[1].WidthRequest = 100;
            //ansPicker.CheckedChanged += ansPicker_CheckedChanged;
            ansPicker.Items[0].Checked = true;
		}
       
    }
}