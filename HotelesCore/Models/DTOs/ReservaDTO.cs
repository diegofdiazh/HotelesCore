using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelesCore.Models.DTOs
{
    public class ReservaDTO
    {
        [Required]
        public string CodigoHotel { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }

    }
}
