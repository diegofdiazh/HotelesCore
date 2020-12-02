using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelesCore.Data.Entities
{
    public class Aeropuertos
    {
        public int Id { get; set; }
        public string NombreAeropuerto { get; set; }
        public string Lata { get; set; }
        public string Oaci { get; set; }
        public string CiudadUbicacin { get; set; }
        public string Departamento { get; set; }
    }
}
