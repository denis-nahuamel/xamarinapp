using Akavache;
using GalaSoft.MvvmLight.Command;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using xamainazureapp.Models;
using xamainazureapp.Services;


namespace xamainazureapp.ViewModels
{
    public class DoctorViewModel : ReactiveObject
    {
        #region================Atributos========================
        int idMedico = Convert.ToInt16(App.Current.Properties["idMedico"]);//el identificador del medico
        const string CacheKey = "PatientsxIdListCache";
        DateTimeOffset CacheExpiry { get { return RxApp.MainThreadScheduler.Now.Add(TimeSpan.FromDays(1)); } }

        ReactiveList<Patient> _patients;
        readonly IDoctorService _patientsService;

        public ReactiveCommand<Unit, IEnumerable<Patient>> LoadPatients { get; private set; }
        public ReactiveCommand agregaPaciente { get; private set; }

        public ReactiveList<Patient> Patients
        {
            get => _patients;
            set => this.RaiseAndSetIfChanged(ref _patients, value);
        }

        //comando agregar pacientes
        public ICommand AgregarPaciente { get { return new RelayCommand(AddPatient); } }
        private NavigationService navigate;

        public async void AddPatient()
        {
           await navigate.Navigate("AddPatient");
        }

        #endregion
        public DoctorViewModel()
        {
            try
            {
                _patientsService = new DoctorService();//inicializa el servicio con los datos
                Patients = new ReactiveList<Patient>();//creacion de la lista reactiva
                LoadPatients = ReactiveCommand.CreateFromObservable(LoadPatientsImpl);//carga los pacientes de la cache
                agregaPaciente = ReactiveCommand.Create(AddPatient);
                LoadPatients.Skip(1)
                            .Subscribe(CacheArticlesImpl);
                LoadPatients.ObserveOn(RxApp.MainThreadScheduler)
                            .Subscribe(MapArticlesImpl);
                navigate = new NavigationService();
            }
            catch {
            }
           
           // LoadPatients.ThrownExceptions.Select(exception => MessageBox.Show(exception.Message));
        }
        #region==================cargado de cache=====================
        /* IObservable<IEnumerable<Patient>> LoadPatientsFromCache()
         {
             return BlobCache
                 .LocalMachine
                 .GetOrFetchObject<IEnumerable<Patient>>
                 (CacheKey,
                  async () =>
                     await _patientsService.Get(idMedico), CacheExpiry);
         }*/
        IObservable<IEnumerable<Patient>> LoadPatientsFromCache()
        {
            try {
                return BlobCache
                .LocalMachine
                .GetAndFetchLatest<IEnumerable<Patient>>
                (CacheKey,
                 async () =>
                    await _patientsService.Get(idMedico));
            }
            catch(Exception ex)
            {
                return null;
            }
            
        }
        void CacheArticlesImpl(IEnumerable<Patient> patients)
        {
            try
            {
                BlobCache
                .LocalMachine
                .InsertObject(CacheKey, patients, CacheExpiry)
                .Wait();
            }
            catch(Exception ex) {
                
            }
            
        }

        IObservable<IEnumerable<Patient>> LoadPatientsImpl()
        {
            try {
                return !Patients.Any() ?
                    LoadPatientsFromCache() :
                    _patientsService.Get(idMedico);
            }
            catch { }
            return !Patients.Any() ?
                    LoadPatientsFromCache() :
                    _patientsService.Get(idMedico);
        }

        void MapArticlesImpl(IEnumerable<Patient> patients)
        {
            using (Patients.SuppressChangeNotifications())
            {
                Patients.Clear();
                Patients.AddRange(patients);
            }
        }
        #endregion
    }
}
