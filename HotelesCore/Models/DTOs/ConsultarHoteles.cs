using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelesCore.Models.DTOs
{
    public class ConsultarHoteles
    {
        [Required]
        [DataType(DataType.Date)]
        public string FechaInicio { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public string FechaFinal { get; set; }
        [Required]
        public string Pais { get; set; }
        [Required]
        public string Ciudad { get; set; }
        [Required]
        public int CantidadHabitaciones { get; set; }
        [Required]
        public string TipoHabitacion { get; set; }
    }
     
}
