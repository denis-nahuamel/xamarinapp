using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xamainazureapp.Models;

namespace xamainazureapp.Services
{
    public interface IFlowersAPI
    {
        [Get("/api/Flowers")]
        Task<List<Flower>> GetFlowers();//listado de flores
    }
}
