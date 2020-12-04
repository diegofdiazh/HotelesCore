using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelesCore.Data.Entities
{
    public class ReservaHoteles
    {
        public int Id { get; set; }
        public string CodigoHotel { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Token { get; set; }
        public string CodigoReserva { get; set; }
    }
}
