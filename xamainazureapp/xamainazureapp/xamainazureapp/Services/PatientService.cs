using Akavache;
using Fusillade;
using Plugin.Connectivity;
using Polly;
using Refit;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reactive.Linq;
using System.Threading.Tasks;
using xamainazureapp.Models;

namespace xamainazureapp.Services
{
    public class PatientService: IPatientService
    {
        private readonly IApiService _apiService;

        public PatientService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<List<Patient>> GetPatients(Priority priority)
        {
            var cache = BlobCache.LocalMachine;
            var cachedConferences = cache.GetAndFetchLatest("patientssss", () => GetRemotePatientAsync(priority),
                offset =>
                {
                    TimeSpan elapsed = DateTimeOffset.Now - offset;
                    return elapsed > new TimeSpan(hours: 24, minutes: 0, seconds: 0);
                });

            var conferences = await cachedConferences.FirstOrDefaultAsync();
            return conferences;
        }

      

        private async Task<List<Patient>> GetRemotePatientAsync(Priority priority)
        {
            List<Patient> conferences = null;
            Task<List<Patient>> getConferencesTask;
            switch (priority)
            {
                case Priority.Background:
                    //getConferencesTask = _apiService.Background.GetPatients();
                    var tekconfApi = RestService.For<IPatientApi>("http://backendazure420180627123231.azurewebsites.net");

                    getConferencesTask =  tekconfApi.GetPatients();
                    break;
                case Priority.UserInitiated:
                    getConferencesTask = _apiService.UserInitiated.GetPatients();
                    break;
                case Priority.Speculative:
                    getConferencesTask = _apiService.Speculative.GetPatients();
                    break;
                default:
                    getConferencesTask = _apiService.UserInitiated.GetPatients();
                    break;
            }

            if (CrossConnectivity.Current.IsConnected)
            {
                conferences = await Policy
                      .Handle<WebException>()
                      .WaitAndRetry
                      (
                        retryCount: 5,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                      )
                      .ExecuteAsync(async () => await getConferencesTask);
            }
            return conferences;
        }

       

    }
    public interface IPatientService
    {
        Task<List<Patient>> GetPatients(Priority priority);
   
    }
}
