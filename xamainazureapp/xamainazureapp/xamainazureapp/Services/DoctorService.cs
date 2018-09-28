using Newtonsoft.Json;
using Refit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reactive.Linq;
using System.Threading.Tasks;
using xamainazureapp.Models;

namespace xamainazureapp.Services
{

    public interface IDoctorService
    {
            IObservable<IEnumerable<Patient>> Get(int id);//pacientes del medico
            IObservable<IEnumerable<Report>> GetReport(int id);//pacientes del medico

    }
    public interface IComment
    {
        [Post("/api/Comentarios/PostComentario")]
        Task PostComment([Body] Comment comment);

        [Put("/api/Doctors/PutDoctor/{id}")]
        Task<dynamic> PutDoctor(int id,[Body] Doctor doctor);

        [Get("/api/Doctors/GetDoctor/{id}")]
        Task<dynamic> GetDoctor(int id);
    }

    public class DoctorService : IDoctorService
        {
            public IObservable<IEnumerable<Patient>> Get(int id)
            {
                var url = App.Url+"/api/Pacientes/GetPacientexId/"+id;
                return Observable.FromAsync(() => 
                        new HttpClient().GetAsync(url))
                                 .SelectMany(async x => {
                                     x.EnsureSuccessStatusCode();
                                     return await x.Content.ReadAsStringAsync(); })
                                 .Select(
                    content => JsonConvert.DeserializeObject<Patient[]>(content)
                    );
            }
      
        public  IObservable<IEnumerable<Report>> GetReport(int id)
        {
            var url = App.Url + "/api/Reportes/GetReportexId/"+id;
              return Observable.FromAsync(() =>
                      new HttpClient().GetAsync(url))
                               .SelectMany(async x => {
                                   x.EnsureSuccessStatusCode();
                                   return await x.Content.ReadAsStringAsync();
                               })
                               .Select(
                  content => JsonConvert.DeserializeObject<Report[]>(content)
                  );

           /* using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode) return null;
                var result = await response.Content.ReadAsStringAsync();

                //De-Serialize
                var list = JsonConvert.DeserializeObject<Report>(result);

               

                return list;*/
            
        }
    }
}
