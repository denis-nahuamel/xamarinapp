using System.Collections.Generic;
using System.Threading.Tasks;
using Fusillade;
using xamainazureapp.Models;
using xamainazureapp.Services;

namespace xamainazureapp.ViewModels
{
    //[ImplementPropertyChanged]
    public class PatientViewModel
    {
        private readonly IPatientService _conferencesService;

        public PatientViewModel(IPatientService conferencesService)
        {
            _conferencesService = conferencesService;
        }

        public List<Patient> Conferences { get; set; }
        public bool IsLoading { get; set; }

        public async Task GetConferences()
        {
            this.IsLoading = true;

            var conferences = await _conferencesService
                                            .GetPatients(Priority.Background)
                                            .ConfigureAwait(false);

            // CacheConferences(conferences);

            this.IsLoading = false;

            this.Conferences = conferences;
        }

        /*  private void CacheConferences(List<ConferenceDto> conferences)
          {
              foreach (var slug in conferences.Select(x => x.Slug))
              {
                  _conferencesService.GetConference(Priority.Speculative, slug);
              }
          }*/
    }
}
