using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reactive.Linq;
using xamainazureapp.Models;

namespace xamainazureapp.Services
{

    public interface IDoctorService
        {
            IObservable<IEnumerable<Patient>> Get(int id);
        }

        public class DoctorService : IDoctorService
        {
            public IObservable<IEnumerable<Patient>> Get(int id)
            {
                //var url = $"{Configuration.ApiBaseUrl}Articles.json";
                var url = "http://backendazure420180627123231.azurewebsites.net/api/Pacientes/GetPacientexId/"+id;
                return Observable.FromAsync(() => 
                        new HttpClient().GetAsync(url))
                                 .SelectMany(async x => {
                                     x.EnsureSuccessStatusCode();
                                     return await x.Content.ReadAsStringAsync(); })
                                 .Select(
                    content => JsonConvert.DeserializeObject<Patient[]>(content)
                    );
            }
        }
}
