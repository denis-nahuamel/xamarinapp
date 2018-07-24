using Fusillade;
using ModernHttpClient;
using Refit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using xamainazureapp.Models;

namespace xamainazureapp.Services
{
    public class ApiService : IApiService
    {
        public const string ApiBaseAddress = "http://backendazure420180627123231.azurewebsites.net";

        public ApiService(string apiBaseAddress = null)
        {
            Func<HttpMessageHandler, IPatientApi> createClient = messageHandler =>
            {
                var client = new HttpClient(messageHandler)
                {
                    BaseAddress = new Uri(apiBaseAddress ?? ApiBaseAddress)
                };

                return RestService.For<IPatientApi>(client);
            };

            _background = new Lazy<IPatientApi>(() => createClient(
                new RateLimitedHttpMessageHandler(new NativeMessageHandler(), Priority.Background)));

            _userInitiated = new Lazy<IPatientApi>(() => createClient(
                new RateLimitedHttpMessageHandler(new NativeMessageHandler(), Priority.UserInitiated)));

            _speculative = new Lazy<IPatientApi>(() => createClient(
                new RateLimitedHttpMessageHandler(new NativeMessageHandler(), Priority.Speculative)));
        }

        private readonly Lazy<IPatientApi> _background;
        private readonly Lazy<IPatientApi> _userInitiated;
        private readonly Lazy<IPatientApi> _speculative;

        public IPatientApi Background
        {
            get { return _background.Value; }
        }

        public IPatientApi UserInitiated
        {
            get { return _userInitiated.Value; }
        }

        public IPatientApi Speculative
        {
            get { return _speculative.Value; }
        }
    }
    public interface IApiService
    {
        IPatientApi Speculative { get; }
        IPatientApi UserInitiated { get; }
        IPatientApi Background { get; }
    }
    [Headers("Accept: application/json")]
    public interface IPatientApi
    {
        [Get("/api/Pacientes/GetPacientes")]
        Task<List<Patient>> GetPatients();

    }
}
