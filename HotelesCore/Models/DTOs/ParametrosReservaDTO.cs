using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelesCore.Models.DTOs
{

    public class Consulta
    {
        [JsonProperty("@Origin")]
        public string Origin { get; set; }
        [JsonProperty("@Destination")]
        public string Destination { get; set; }
        [JsonProperty("@StartDate")]
        public string StartDate { get; set; }
        [JsonProperty("@EndDate")]
        public string EndDate { get; set; }
        [JsonProperty("@QuantityTravellers")]
        public string QuantityTravellers { get; set; }
        [JsonProperty("@Class")]
        public string Class { get; set; }
    }

    public class Reserva
    {
        [JsonProperty("@Name")]
        public string Name { get; set; }
        [JsonProperty("@LastName")]
        public string LastName { get; set; }
        [JsonProperty("@FlightCode")]
        public string FlightCode { get; set; }
    }

    public class Vuelos
    {
        public Consulta consulta { get; set; }
        public Reserva reserva { get; set; }
    }

    public class Consulta2
    {
        [JsonProperty("@Country")]
        public string Country { get; set; }
        [JsonProperty("@City")]
        public string City { get; set; }
        [JsonProperty("@StartDate")]
        public string StartDate { get; set; }
        [JsonProperty("@EndDate")]
        public string EndDate { get; set; }
        [JsonProperty("@QuantityRooms")]
        public string QuantityRooms { get; set; }
        [JsonProperty("@RoomType")]
        public string RoomType { get; set; }
    }

    public class Reserva2
    {
        [JsonProperty("@HotelCode")]
        public string HotelCode { get; set; }
        [JsonProperty("@Name")]
        public string Name { get; set; }
        [JsonProperty("@LastName")]
        public string LastName { get; set; }
    }

    public class Hotel
    {
        public Consulta2 consulta { get; set; }
        public Reserva2 reserva { get; set; }
    }

    public class Consulta3
    {
        [JsonProperty("@Origin")]
        public string Origin { get; set; }
        [JsonProperty("@Destination")]
        public string Destination { get; set; }
        [JsonProperty("@StartDate")]
        public string StartDate { get; set; }
        [JsonProperty("@EndDate")]
        public string EndDate { get; set; }
        [JsonProperty("@QuantityTravellers")]
        public string QuantityTravellers { get; set; }
        [JsonProperty("@Class")]
        public string Class { get; set; }
    }

    public class Reserva3
    {
        [JsonProperty("@TicketCode")]
        public string TicketCode { get; set; }
        [JsonProperty("@Name")]
        public string Name { get; set; }
        [JsonProperty("@LastName")]
        public string LastName { get; set; }
    }

    public class TransporteTerrestre
    {
        public Consulta3 consulta { get; set; }
        public Reserva3 reserva { get; set; }
    }

    public class Consulta4
    {
        [JsonProperty("@Category")]
        public string Category { get; set; }
        [JsonProperty("@City")]
        public string City { get; set; }
        [JsonProperty("@Date")]
        public string Date { get; set; }
        public Consulta4()
        {

        }
    }

    public class Reserva4
    {
        [JsonProperty("@TicketCode")]
        public string TicketCode { get; set; }
        [JsonProperty("@Location")]
        public string Location { get; set; }
    }

    public class Eventos
    {
        public Consulta4 consulta { get; set; }
        public Reserva4 reserva { get; set; }
    }

    public class Parameters
    {
        public Vuelos vuelos { get; set; }
        public Hotel hotel { get; set; }
        public TransporteTerrestre transporteTerrestre { get; set; }
        public Eventos eventos { get; set; }
    }

    public class ParametrosDTO
    {
        public string Tipo_proveedor { get; set; }
        public string Tipo_proceso { get; set; }
        public Parameters Parametros { get; set; }
        public string processType { get; set; }
        public string Uuid { get; set; }
        public ParametrosDTO()
        {
            Parametros = new Parameters();
            Parametros.eventos = new Eventos();
            Parametros.hotel = new Hotel();
            Parametros.transporteTerrestre = new TransporteTerrestre();
            Parametros.vuelos = new Vuelos();
            Parametros.eventos = new Eventos
            {
                consulta = null,
                reserva = null
            };
            Parametros.hotel = new Hotel
            {
                consulta = null,
                reserva = null
            };
            Parametros.transporteTerrestre = new TransporteTerrestre
            {
                consulta = null,
                reserva = null
            };
            Parametros.vuelos = new Vuelos
            {
                consulta = null,
                reserva = null
            };

            Consulta4 consulta4 = new Consulta4
            {
                Category = "",
                City = "",
                Date = ""
            };
            Reserva4 reserva4 = new Reserva4
            {
                Location = "",
                TicketCode = ""
            };
            Consulta3 consulta3 = new Consulta3
            {
                Class = "",
                Destination = "",
                EndDate = "",
                Origin = "",
                QuantityTravellers = "",
                StartDate = ""
            };
            Reserva3 reserva3 = new Reserva3
            {
                LastName = "",
                Name = "",
                TicketCode = ""
            };
            Consulta2 consulta2 = new Consulta2
            {
                City = "",
                Country = "",
                EndDate = "",
                QuantityRooms = "",
                RoomType = "",
                StartDate = ""
            };
            Reserva2 reserva2 = new Reserva2
            {
                HotelCode = "",
                LastName = "",
                Name = ""
            };
            Consulta consulta = new Consulta
            {
                Class = "",
                Destination = "",
                EndDate = "",
                Origin = "",
                QuantityTravellers = "",
                StartDate = ""
            };
            Reserva reserva = new Reserva
            {
                FlightCode = "",
                LastName = "",
                Name = ""
            };
            Parametros.hotel.consulta = consulta2;
            Parametros.hotel.reserva = reserva2;
            Parametros.transporteTerrestre.consulta = consulta3;
            Parametros.transporteTerrestre.reserva = reserva3;
            Parametros.vuelos.consulta = consulta;
            Parametros.vuelos.reserva = reserva;
            Parametros.eventos.consulta = consulta4;
            Parametros.eventos.reserva = reserva4;
        }

    }
}
