using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using xamainazureapp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace xamainazureapp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ListReport : ContentPage, IViewFor<ReportViewModel>
    {
        readonly CompositeDisposable _bindingsDisposable = new CompositeDisposable();
        int IdPatient;
        public ListReport()
        {
            InitializeComponent();
        }
        public ListReport(xamainazureapp.Models.Patient patient)
        {
            InitializeComponent();
            IdPatient = patient.PacienteID;
            Reports.ItemTapped += Reports_ItemTapped;
        }

        private async void Reports_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            // xamainazureapp.Models.Patient patient;
            var content = e.Item as xamainazureapp.Models.Report;
            await Navigation.PushAsync(new ReportDetail(content));
        }

        private async void Patients_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            // xamainazureapp.Models.Patient patient;
            var content = e.Item as xamainazureapp.Models.Patient;
            await Navigation.PushAsync(new ListReport(content));
        }

        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                App.listReport = this;
                ViewModel = new ReportViewModel(IdPatient);

                this.OneWayBind(ViewModel, vm => vm.Reports, v => v.Reports.ItemsSource).DisposeWith(_bindingsDisposable);
                this.BindCommand(ViewModel, vm => vm.LoadReports, v => v.Reports, nameof(ListView.Refreshing)).DisposeWith(_bindingsDisposable);
                //this.BindCommand(ViewModel, vm => vm.agregaPaciente, v=>v.pa);
                BindingContext = ViewModel;
                ViewModel.LoadReports.Subscribe(_ => Reports.EndRefresh());
                ViewModel.LoadReports.ThrownExceptions.Select(exception => DisplayAlert("error", exception.Message, "OK"));
                //https://codereview.stackexchange.com/questions/74642/a-viewmodel-using-reactiveui-6-that-loads-and-sends-data
                this.WhenAnyValue(x => x.ViewModel.LoadReports)
                    .SelectMany(x => x.Execute())

                    .Subscribe();
            }
            catch (Exception ex)
            {
                DisplayAlert("error", ex.Message, "OK");
            }

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _bindingsDisposable.Clear();
        }

        #region ViewModel Setup
        public ReportViewModel ViewModel { get; set; }
        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (ReportViewModel)value; }
        }
        #endregion
       
    }
}