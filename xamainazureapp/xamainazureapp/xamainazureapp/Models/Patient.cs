using System;
using System.Collections.Generic;
using System.Text;

namespace xamainazureapp.Models
{
   public class Patient
    {
        public int PacienteID { get; set; }
        public string Nombre { get; set; }


        public string Apellidos { get; set; }


        public string Email { get; set; }

        public string Documento { get; set; }


        public string Telefono { get; set; }


        public string FechaNacimiento { get; set; }


        public string Distrito { get; set; }


        public string Direccion { get; set; }
        public bool Sexo { get; set; }


        public string Alergias { get; set; }
        public bool Diabetes { get; set; }


        public string TipoSangre { get; set; }

        public string Seguro { get; set; }

        public string FactorRiesgo { get; set; }

        public string Patologia { get; set; }

        public string FechaInicio { get; set; }
    }
}
