using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelesCore.Models.Responses
{
    public class ResponseConsultaHoteles
    {
        public List<ResponseBase> hoteles { get; set; }
        public ResponseConsultaHoteles()
        {
            hoteles = new List<ResponseBase>();
        }
    }
    public partial class ResponseBase
    {
        public string Country { get; set; }
        public string City { get; set; }
        public DateTime Stardate { get; set; }
        public DateTime EndDate { get; set; }
        public long Price { get; set; }
        public string HotelCode { get; set; }
    }
}


