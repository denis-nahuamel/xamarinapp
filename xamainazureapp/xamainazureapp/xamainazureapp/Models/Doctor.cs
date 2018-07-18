
using System;
using System.Collections.Generic;
using System.Text;

namespace xamainazureapp.Models
{
   public class Doctor
    {
        public int DoctorID { get; set; }
        public string Nombre { get; set; }

        public string Apellidos { get; set; }
        public string Email { get; set; }

        public int Documento { get; set; }

        public string Numero { get; set; }
        public string FechaNacimiento { get; set; }
        public string Distrito { get; set; }

        public string Direccion { get; set; }

        public bool Sexo { get; set; }
        public string Codigo { get; set; }

        public string Registro { get; set; }
        public string Contrasenia { get; set; }
    }
}
