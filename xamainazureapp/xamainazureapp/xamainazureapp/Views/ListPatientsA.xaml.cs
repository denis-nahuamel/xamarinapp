using Refit;
using System.Threading.Tasks;
using xamainazureapp.Services;
using xamainazureapp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace xamainazureapp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ListPatientsA : ContentPage
	{
        private readonly PatientViewModel _viewModel;
        public const string TekConfApiUrl = "http://backendazure420180627123231.azurewebsites.net";

        public ListPatientsA()
        {
            InitializeComponent();
            holaaa();
            var apiService = new ApiService(TekConfApiUrl);
            var service = new PatientService(apiService);

            _viewModel = new PatientViewModel(service);

            this.BindingContext = _viewModel;

            _viewModel.GetConferences();
        }
        async Task holaaa()
        {
            var tekconfApi = RestService.For<IPatientApi>("http://backendazure420180627123231.azurewebsites.net");

            var helo = await tekconfApi.GetPatients();
        }
    }
}