using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelesCore.Models.DTOs
{

    public class ParametrosReservaDTO
    {
        public string Tipo_proveedor { get; set; }
        public string Nombre_proveedor { get; set; }
        public string Tipo_proceso { get; set; }
        public Parameters Parametros { get; set; }
        public string processType { get; set; }
        public string Uuid { get; set; }
        public ParametrosReservaDTO()
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
