using System;
using System.Collections.Generic;
using xamainazureapp.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace xamainazureapp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ReportDetail : ContentPage
	{
        // string fecha;
        Report Report = new Report();
        Sleep Sleep = new Sleep();
        int cantidad;
        string nombre;
        public List<Food> FoodList { get; set; }
        public ReportDetail (Report report)
		{
			InitializeComponent ();
            Report = report;
            Sleep = report.Suenio[0];
            FoodList=new List<Food>();
            DarValores();
            foodList.ItemsSource = FoodList;
           // FoodList = report.Comida;
            //fecha.Text = report.Fecha.ToString();

		}
        void DarValores()
        {
            try
            {
                txtGlucosa.Text = Report.Glucosa.ToString();
                txtPeso.Text = Report.Peso.ToString();
                txtTemperatura.Text = Report.Temperatura.ToString();
                txtSPO2.Text = Report.Spo2.ToString();
                txtRitmo.Text = Report.RitmoCardiaco.ToString();
                swPastillas.IsToggled = Report.Pastillas;
                txtGlucosa.Text = Report.Glucosa.ToString();
                txtGlucosa.Text = Report.Glucosa.ToString();
                txtGlucosa.Text = Report.Glucosa.ToString();

                txtInicio.Text = Sleep.Inicio.ToString();
                txtDuracion.Text = Sleep.Duracion.ToString();
                txtInterrupciones.Text = Sleep.Interrupciones.ToString();
                txtCalidad.Text = Sleep.Calidad.ToString();
                foreach (var food in Report.Comida)
                {
                    cantidad = food.Cantidad;
                    nombre = food.Nombre;
                    FoodList.Add(new Food
                    {
                        
                        Cantidad = cantidad,
                        Nombre =nombre
                    });
                }
            }
            catch(Exception ex)
            {
                DisplayAlert("",ex.Message, "OK");
            }
            
        }
	}
}