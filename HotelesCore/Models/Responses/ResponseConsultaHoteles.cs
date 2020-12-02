using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelesCore.Models.Responses
{
    public class ResponseConsultaHoteles
    {
        public List<ResponseBaseHoteles> vuelos { get; set; }
        public ResponseConsultaHoteles()
        {
            vuelos = new List<ResponseBaseHoteles>();
        }
    }
    public partial class ResponseBaseHoteles
    {
        public string Supplier { get; set; }      
        public string City { get; set; }
        public string Description { get; set; }
        public DateTime Stardate { get; set; }
        public DateTime EndDate { get; set; }
        public string HotelImage { get; set; }
        public string HotelCode { get; set; }
        public string Price { get; set; }    

    }
    public class ResponseReservaHoteles
    {
        public List<ResponseBaseHotelesReserva> vuelos { get; set; }
        public ResponseReservaHoteles()
        {
            vuelos = new List<ResponseBaseHotelesReserva>();
        }
    }
    public partial class ResponseBaseHotelesReserva
    {
        public bool Status { get; set; }

    }
}


