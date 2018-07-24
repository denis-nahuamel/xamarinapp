using Akavache;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using xamainazureapp.Models;
using xamainazureapp.Services;
namespace xamainazureapp.ViewModels
{
    public class ReportViewModel: ReactiveObject
    {

        #region================Atributos========================
        int IdReport;
        const string CacheKey = "ReportxIdListCacheee";
        DateTimeOffset CacheExpiry { get { return RxApp.MainThreadScheduler.Now.Add(TimeSpan.FromDays(1)); } }

        ReactiveList<Report> _reports;
        readonly IDoctorService _reportService;

        public ReactiveCommand<Unit, IEnumerable<Report>> LoadReports { get; private set; }
       // public ReactiveCommand agregaPaciente { get; private set; }

        public ReactiveList<Report> Reports
        {
            get => _reports;
            set => this.RaiseAndSetIfChanged(ref _reports, value);
        }

        //comando agregar pacientes
       // public ICommand AgregarPaciente { get { return new RelayCommand(AddPatient); } }
        private NavigationService navigate;

        public async void AddPatient()
        {
            await navigate.Navigate("AddPatient");
        }
        public ReportViewModel()
        {
            _reportService = new DoctorService();//inicializa el servicio con los datos
            Reports = new ReactiveList<Report>();//creacion de la lista reactiva
            LoadReports = ReactiveCommand.CreateFromObservable(LoadReportsImpl);//carga los pacientes de la cache
            //agregaPaciente = ReactiveCommand.Create(AddPatient);
            LoadReports.Skip(1)
                        .Subscribe(CacheReportsImpl);
            LoadReports.ObserveOn(RxApp.MainThreadScheduler)
                        .Subscribe(MapReportsImpl);
            navigate = new NavigationService();
        }
        #endregion
        public ReportViewModel(int idReport)
        {
            IdReport = idReport;
            _reportService = new DoctorService();//inicializa el servicio con los datos
            Reports = new ReactiveList<Report>();//creacion de la lista reactiva
            LoadReports = ReactiveCommand.CreateFromObservable(LoadReportsImpl);//carga los pacientes de la cache
            //agregaPaciente = ReactiveCommand.Create(AddPatient);
            LoadReports.Skip(1)
                        .Subscribe(CacheReportsImpl);
            LoadReports.ObserveOn(RxApp.MainThreadScheduler)
                        .Subscribe(MapReportsImpl);
            navigate = new NavigationService();
            // LoadPatients.ThrownExceptions.Select(exception => MessageBox.Show(exception.Message));
        }
        #region==================cargado de cache=====================
        IObservable<IEnumerable<Report>> LoadReportsFromCache()
        {
            return BlobCache
                .LocalMachine
                .GetOrFetchObject<IEnumerable<Report>>
                (CacheKey+IdReport.ToString(),
                 async () =>
                    await _reportService.GetReport(IdReport), CacheExpiry);
        }

        void CacheReportsImpl(IEnumerable<Report> reports)
        {
            BlobCache
                .LocalMachine
                .InsertObject(CacheKey+IdReport.ToString(), reports, CacheExpiry)
                .Wait();
        }

        IObservable<IEnumerable<Report>> LoadReportsImpl()
        {
            return !Reports.Any() ?
                LoadReportsFromCache() :
                _reportService.GetReport(IdReport);
        }

        void MapReportsImpl(IEnumerable<Report> reports)
        {
            using (Reports.SuppressChangeNotifications())
            {
                Reports.Clear();
                Reports.AddRange(reports);
            }
        }
        #endregion
    }
}
