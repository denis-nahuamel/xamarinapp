using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xamainazureapp.Models;

namespace xamainazureapp.Services
{
   public interface IDoctorApi
    {
        [Get("/api/Doctors/{id}")]
        Task<Doctor> GetDoctor(int id);//Listado de doctores
        [Post("/api/Doctors/Entrar")]
        //nuevo comentario
        //otro
        Task Login([Body] Login makeUp);
    }
}
