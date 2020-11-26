using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutheticationLibrary;
using HotelesCore.Data.Entities;
using HotelesCore.Models.DTOs;
using HotelesCore.Models.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using VuelosCore.Data;

namespace HotelesCore.Controllers
{
    [Route("api/v1/Hoteles")]
    [ApiController]
    public class HotelesController : ControllerBase
    {
        private readonly ILogger<HotelesController> Logger;
        private readonly ApplicationDbContext _db;
        public HotelesController(ILogger<HotelesController> logger, ApplicationDbContext context)
        {
            Logger = logger;
            _db = context;
        }
        [HttpPost]
        [Route("ConsultarHoteles")]
        public IActionResult ConsultarHoteles([FromBody] ConsultarHoteles model)
        {
            try
            {
                DateTime dateTimeInicio;
                DateTime dateTimeFinal;
                if (!DateTime.TryParseExact(model.FechaInicio, "dd'/'MM'/'yyyy",
                                          CultureInfo.InvariantCulture,
                                          DateTimeStyles.None,
                                          out dateTimeInicio))
                {
                    return BadRequest("Formato de fecha invalido, formato permitido dd/MM/aaaa");
                }
                if (!DateTime.TryParseExact(model.FechaFinal, "dd'/'MM'/'yyyy",
                                          CultureInfo.InvariantCulture,
                                          DateTimeStyles.None,
                                          out dateTimeFinal))
                {
                    return BadRequest("Formato de fecha invalido, formato permitido dd/MM/aaaa");
                }
                ParametrosDTO parametros = new ParametrosDTO();
                Consulta2 consultaHoteles = new Consulta2
                {
                    Country = model.Pais,
                    City = model.Ciudad,
                    RoomType = model.CantidadHabitaciones.ToString(),
                    QuantityRooms = model.CantidadHabitaciones.ToString(),
                    EndDate = model.FechaFinal,
                    StartDate = model.FechaInicio
                };
                parametros.parameters.hotel.consulta = consultaHoteles;
                ResponseConsultaHoteles response = new ResponseConsultaHoteles();
                List<ResponseBase> hoteles = new List<ResponseBase>
                {
                    new ResponseBase
                    {
                        City = model.Ciudad,
                        Country = model.Pais,
                        Stardate = DateTime.Now.AddDays(1),
                        EndDate = DateTime.Now.AddDays(5),
                        HotelCode = "4456d81asd9",
                        Price = 2000000
                    },
                    new ResponseBase
                    {
                       City = model.Ciudad,
                        Country = model.Pais,
                        Stardate = DateTime.Now.AddDays(1),
                        EndDate = DateTime.Now.AddDays(5),
                        HotelCode = "89898sd81asd9",
                        Price = 3000000
                    },
                    new ResponseBase
                    {
                        City = model.Ciudad,
                        Country = model.Pais,
                        Stardate = DateTime.Now.AddDays(1),
                        EndDate = DateTime.Now.AddDays(5),
                        HotelCode = "1231d81asd9",
                        Price = 4000000
                    },
                    new ResponseBase
                    {
                       City = model.Ciudad,
                        Country = model.Pais,
                        Stardate = DateTime.Now.AddDays(1),
                        EndDate = DateTime.Now.AddDays(5),
                        HotelCode = "96454d81asd9",
                        Price = 5000000
                    }
                };
                response.hoteles = hoteles;
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError("Excepcion generada en ConsultarVuelos: " + ex.Message);
                return StatusCode(500, "Ocurrio un error");
                throw;
            }
        }
        [HttpPost]
        [Route("ReservaHotel")]
        public IActionResult ReservaHotel([FromBody] ReservaDTO model, [FromHeader] string Token)
        {
            try
            {
                Logger.LogInformation("INICIA PROCESO DE RESERVA DE HOTELES");
                JwtProvider jwt = new JwtProvider("TouresBalon.com", "UsuariosPlataforma");
                var accessToken = Request.Headers[HeaderNames.Authorization];
                var first = accessToken.FirstOrDefault();
                if (string.IsNullOrEmpty(accessToken) || !first.Contains("Bearer"))
                {
                    return BadRequest();
                }
                string token = first.Replace("Bearer", "").Trim();
                Logger.LogInformation("INICIA PROCESO DE VALIDACION DE TOKEN :" + token);
                var a = jwt.ValidateToken(token);
                if (!a)
                {
                    return Unauthorized();
                }
                ParametrosDTO parametros = new ParametrosDTO();
                Reserva2 reserva = new Reserva2
                {
                    HotelCode = model.CodigoHotel,
                    LastName = model.Apellido,
                    Name = model.Nombre
                };
                parametros.parameters.hotel.reserva = reserva;

                _db.ReservaHotels.Add(new ReservaHotel
                {
                    Nombre = model.Nombre,
                    Apellido = model.Apellido,
                    Token = token,
                    CodigoHotel = model.CodigoHotel
                });
                _db.SaveChanges();
                return Ok(new ResponseReservaVuelo
                {
                    success = true
                });
            }
            catch (Exception ex)
            {
                Logger.LogError("Excepcion generada en ReservaHotel: " + ex.Message);
                return StatusCode(500, "Ocurrio un error");
                throw;
            }
        }
        [HttpGet]
        [Route("Healty")]
        public IActionResult Healty()
        {        
            return Ok("Todo Bien");
        }
    }
}
