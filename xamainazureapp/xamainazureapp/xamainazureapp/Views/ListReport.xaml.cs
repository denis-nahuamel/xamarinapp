using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace xamainazureapp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ListReport : ContentPage
	{
		public ListReport ()
		{
			InitializeComponent ();
		}
        public ListReport(xamainazureapp.Models.Patient paciente)
        {
            InitializeComponent();
        }
    }
}