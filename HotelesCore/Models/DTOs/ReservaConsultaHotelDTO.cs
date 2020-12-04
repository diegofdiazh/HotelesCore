using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelesCore.Models.DTOs
{
    public class ReservaConsultaHotelDTO
    {
        [Required]
        public string Uuid { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public string FechaInicio { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public string FechaFinal { get; set; }
        [Required]
        public string CiudadDestino { get; set; }

    }
}
