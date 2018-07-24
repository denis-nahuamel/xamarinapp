using System;
using System.Collections.Generic;
using System.Text;

namespace xamainazureapp.Models
{
   public class Report
    {
        public int Peso { get; set; }
        public int Glucosa { get; set; }
        public string Temperatura { get; set; }
        public string Spo2 { get; set; }
        public string RitmoCardiaco { get; set; }
        public List<Sleep> Suenio { get; set; }
        public bool Pastillas { get; set; }
        public List<Food> Comida { get; set; }
        public string Fecha { get; set; }
    }
}
