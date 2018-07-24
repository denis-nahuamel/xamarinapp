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
    public partial class ListPatient : ContentPage, IViewFor<DoctorViewModel>
    {
        readonly CompositeDisposable _bindingsDisposable = new CompositeDisposable();
        int idMedico;
      
        public ListPatient()
        {
            InitializeComponent();
            BindingContext = ViewModel;
        }
        public ListPatient(int id)
        {
            InitializeComponent();
            
            idMedico = id;
            Patients.ItemTapped += Patients_ItemTapped;//entra a los reportes del paciente
            
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
                App.listPatient = this;
                ViewModel = new DoctorViewModel();

                this.OneWayBind(ViewModel, vm => vm.Patients, v => v.Patients.ItemsSource).DisposeWith(_bindingsDisposable);
                this.BindCommand(ViewModel, vm => vm.LoadPatients, v => v.Patients, nameof(ListView.Refreshing)).DisposeWith(_bindingsDisposable);
                //this.BindCommand(ViewModel, vm => vm.agregaPaciente, v=>v.pa);
                BindingContext = ViewModel;
                ViewModel.LoadPatients.Subscribe(_ => Patients.EndRefresh());
                ViewModel.LoadPatients.ThrownExceptions.Select(exception => DisplayAlert("error", exception.Message, "OK"));
                //https://codereview.stackexchange.com/questions/74642/a-viewmodel-using-reactiveui-6-that-loads-and-sends-data
                this.WhenAnyValue(x => x.ViewModel.LoadPatients)
                    .SelectMany(x => x.Execute())

                    .Subscribe();
            }
            catch (Exception ex)
            {
                DisplayAlert("error", ex.Message, "OK");
            }
            
        }

        private void Holaaas_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("d", "s", "d");
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _bindingsDisposable.Clear();
        }

        #region ViewModel Setup
        public DoctorViewModel ViewModel { get; set; }
        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (DoctorViewModel)value; }
        }
        #endregion
    }
}
