using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutheticationLibrary;
using Confluent.Kafka;
using HotelesCore.Data.Entities;
using HotelesCore.Interfaces;
using HotelesCore.Models.DTOs;
using HotelesCore.Models.Responses;
using HotelesCore.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using VuelosCore.Data;
using VuelosCore.Models.Responses;

namespace HotelesCore.Controllers
{
    [Route("api/v1/Hoteles")]
    [ApiController]
    public class HotelesController : ControllerBase
    {
        private readonly ILogger<HotelesController> Logger;
        private readonly ApplicationDbContext _db;
        private readonly ProducerConfig _config;
        private readonly IAppLogger<ServidorCache> _loggercache;
        public HotelesController(ILogger<HotelesController> logger, ApplicationDbContext context, ProducerConfig config, IAppLogger<ServidorCache> loggercache)
        {
            Logger = logger;
            _db = context;
            _config = config;
            _loggercache = loggercache;
        }
        [HttpGet]
        [Route("GetCiudades")]
        [EnableCors("AllowAll")]
        public IActionResult GetCiudades()
        {
            try
            {
                Logger.LogInformation("Inicia obtencion de aeropuertos");
                var aeropuertos = _db.Aeropuertos.Where(c => !string.IsNullOrEmpty(c.Lata)).OrderBy(c => c.CiudadUbicacin).ToList();
                List<ResponseAeropuertos> responseAeropuertos = new List<ResponseAeropuertos>();
                foreach (var item in aeropuertos)
                {
                    if (responseAeropuertos.FirstOrDefault(c => c.CiudadUbicacion == item.CiudadUbicacin) == null)
                    {
                        responseAeropuertos.Add(new ResponseAeropuertos
                        {
                            CiudadUbicacion = item.CiudadUbicacin,
                            Iata = item.Lata,
                            Id = item.Id,
                            Concatenado = $"{item.CiudadUbicacin}[{item.Lata}]"
                        });
                    }
                }
                Logger.LogInformation(responseAeropuertos.ToString());
                return Ok(responseAeropuertos);
            }
            catch (Exception ex)
            {
                Logger.LogError("Excepcion generada en GetAeropuertos: " + ex.Message);
                return StatusCode(500, "Ocurrio un error");
                throw;
            }
        }
        [HttpPost]
        [Route("ConsultarHoteles")]
        [EnableCors("AllowAll")]
        public async Task<IActionResult> ConsultarHotelesAsync([FromBody] ConsultarHoteles model)
        {
            try
            {
                DateTime dateTimeInicio;
                DateTime dateTimeFinal;
                if (!DateTime.TryParseExact(model.FechaInicio, "yyyy'-'MM'-'dd",
                                          CultureInfo.InvariantCulture,
                                          DateTimeStyles.None,
                                          out dateTimeInicio))
                {
                    return BadRequest("Formato de fecha invalido, formato permitido dd/MM/aaaa");
                }
                if (!DateTime.TryParseExact(model.FechaFinal, "yyyy'-'MM'-'dd",
                                          CultureInfo.InvariantCulture,
                                          DateTimeStyles.None,
                                          out dateTimeFinal))
                {
                    return BadRequest("Formato de fecha invalido, formato permitido yyyy-MM-dd");
                }
                var destino = _db.Aeropuertos.FirstOrDefault(c => c.CiudadUbicacin == model.CiudadDestino);
                if (destino == null)
                {
                    return NotFound("No se encontro la ciudad de destino");
                }
                ParametrosDTO parametros = new ParametrosDTO();
                parametros.processType = "CATALOG";
                parametros.Uuid = model.Uuid;
                parametros.Tipo_proveedor = "HOTEL";
                parametros.Tipo_proceso = "catalogue";
                Consulta2 consultaHotel = new Consulta2
                {
                    City = destino.CiudadUbicacin,
                    Country = "Colombia",
                    QuantityRooms = "1",
                    RoomType = "Bar",
                    EndDate = model.FechaFinal,
                    StartDate = model.FechaInicio
                };
                parametros.Parametros.hotel.consulta = consultaHotel;
                string parametrosSerializados = JsonConvert.SerializeObject(parametros);
                using (var producer = new ProducerBuilder<Null, string>(_config).Build())
                {
                    await producer.ProduceAsync("topic-info-reader", new Message<Null, string>
                    {
                        Value = parametrosSerializados
                    });
                    producer.Flush(TimeSpan.FromSeconds(10));
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Excepcion generada en ConsultarVuelos: " + ex.Message);
                return StatusCode(500, "Ocurrio un error");
                throw;
            }
        }
        [HttpPost]
        [Route("ReservarHotel")]
        [EnableCors("AllowAll")]
        public async Task<IActionResult> ReservarHotelAsync([FromBody] ReservaDTO model, [FromHeader] string Token)
        {
            try
            {
                Logger.LogInformation("INICIA PROCESO DE RESERVA DE VUELO");
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
                ParametrosReservaDTO parametros = new ParametrosReservaDTO();
                parametros.Nombre_proveedor = model.NombreProveedor;
                parametros.processType = "CATALOG";
                parametros.Uuid = model.Uuid;
                parametros.Tipo_proveedor = "HOTEL";
                parametros.Tipo_proceso = "catalogue";
                Reserva2 reserva = new Reserva2
                {
                    HotelCode = model.CodigoHotel,
                    LastName = model.Apellido,
                    Name = model.Nombre
                };
                parametros.Parametros.hotel.reserva = reserva;
                string parametrosSerializados = JsonConvert.SerializeObject(parametros);
                using (var producer = new ProducerBuilder<Null, string>(_config).Build())
                {
                    await producer.ProduceAsync("topic-info-reader", new Message<Null, string>
                    {
                        Value = parametrosSerializados
                    });
                    producer.Flush(TimeSpan.FromSeconds(10));
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Excepcion generada en ReservarVuelo: " + ex.Message);
                return StatusCode(500, "Ocurrio un error");
                throw;
            }
        }

        [HttpPost]
        [Route("ConsultarHotelesUiid")]
        [EnableCors("AllowAll")]
        public IActionResult ConsultarHotelesUiid([FromBody] ReservaConsultaHotelDTO model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Uuid))
                {
                    return BadRequest();
                }
                else
                {
                    ServidorCache servidorCache = new ServidorCache(_loggercache);
                    var vuelos = servidorCache.getCache(model.Uuid + "_HOTEL" + "_CATALOG");

                    if (vuelos != null)
                    {
                        Random r = new Random();
                        string[] Descripciones = new string[10];
                        Descripciones[1] = "Hotel con vista al mar perfecto para viaje de placer";
                        Descripciones[2] = "Hotel en la montaña para alejarse de la monotonia y conectarse con la naturaleza";
                        Descripciones[3] = "Hotel perfecto para viajes de negocio";
                        Descripciones[4] = "Hotel para la familia con actividadades ludicas para cualquier edad";
                        Descripciones[5] = "Hotel todo terreno, descanso, trabajo lo que quieras ven pero ya!";                        
                        Logger.LogInformation("Se obtiene respuesta de normalizador :" + vuelos);
                        Logger.LogInformation($"Contiene {vuelos.providersResponse.Count} proveedores la respuesta");
                        if (vuelos.providersResponse.Count > 0)
                        {
                            List<ResponseBaseHoteles> responseVuelos = new List<ResponseBaseHoteles>();
                            foreach (var item in vuelos.providersResponse)
                            {
                                Logger.LogInformation($"proveedor {item}");
                                if (!string.IsNullOrEmpty(item.code) && !string.IsNullOrEmpty(item.destination) && !string.IsNullOrEmpty(item.hotelname) && !string.IsNullOrEmpty(item.price) && !string.IsNullOrEmpty(item.providerName) && !string.IsNullOrEmpty(item.roomType))
                                {
                                    Logger.LogInformation($"proveedor valido");
                                    Logger.LogInformation($"Pais origen {"Colombia"}");
                                    Logger.LogInformation($"Ciudad destino {item.destination}");
                                    var destino = _db.Aeropuertos.FirstOrDefault(c => c.Lata == item.destination);
                                    if (destino != null)
                                    {
                                        DateTime stardate = DateTime.Parse(model.FechaInicio);
                                        DateTime endDate = DateTime.Parse(model.FechaFinal);
                                        Logger.LogInformation($"origen y destino validos");
                                        responseVuelos.Add(new ResponseBaseHoteles
                                        {
                                            City = destino.CiudadUbicacin,
                                            Description = Descripciones[r.Next(1, 10)],
                                            HotelCode = item.code,
                                            HotelImage = $"hotel{r.Next(1, 5)}​​​​​",
                                            Stardate = stardate,
                                            EndDate = endDate,
                                            Price = item.price,
                                            Supplier = item.providerName,
                                            HotelName = item.hotelname
                                        });
                                        Logger.LogInformation($"proveedor agregado correctamente");
                                    }
                                    else
                                    {
                                        Logger.LogInformation($"origen invalido");
                                    }
                                }
                                else
                                {
                                    Logger.LogInformation($"proveedor invalido");
                                }
                            }
                            return Ok(responseVuelos);
                        }
                        else
                        {
                            return NotFound("No existen proveedores disponibles para esta solicitud");
                        }
                    }
                    else
                    {
                        return NotFound("No se encontro informacion con este Uuid");
                    }

                }

            }
            catch (Exception ex)
            {
                Logger.LogError("Excepcion generada en ConsutlarVuelosUuid: " + ex.Message);
                return StatusCode(500, "Ocurrio un error");
                throw;
            }
        }

        [HttpGet]
        [Route("ConsultarReservaUiid")]
        [EnableCors("AllowAll")]
        public IActionResult ConsultarReservaUiid(string uuid, string nombre, string apellido, string codigoHotel)
        {
            try
            {
                Logger.LogInformation("INICIA PROCESO DE RESERVA DE VUELO");
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
                if (string.IsNullOrEmpty(uuid))
                {
                    return BadRequest();
                }
                else
                {
                    ServidorCache servidorCache = new ServidorCache(_loggercache);
                    var vuelos = servidorCache.getCacheReserva(uuid + "_HOTEL" + "_RESERVE");

                    if (vuelos != null)
                    {
                        Logger.LogInformation("Se obtiene respuesta de normalizador :" + vuelos);
                        Logger.LogInformation($"Contiene {vuelos.providersResponse.Count} proveedores la respuesta");
                        if (vuelos.providersResponse.Count > 0)
                        {

                            Random r = new Random();
                            var x = r.Next(0, 1000000);
                            string s = x.ToString("000000");
                            List<ResponseBaseHotelesReserva> responseVuelos = new List<ResponseBaseHotelesReserva>();
                            foreach (var item in vuelos.providersResponse)
                            {
                                Logger.LogInformation($"proveedor {item}");
                                responseVuelos.Add(new ResponseBaseHotelesReserva
                                {
                                    Estado=true,
                                    CodigoReservaVuelo=s
                                });
                                Logger.LogInformation($"proveedor agregado correctamente");
                            }
                            _db.ReservaHoteles.Add(new ReservaHoteles
                            {
                                Apellido = apellido,
                                CodigoReserva = s,
                                CodigoHotel = codigoHotel,
                                Nombre = nombre,
                                Token = token
                            });
                            _db.SaveChanges();
                            return Ok(responseVuelos);
                        }
                        else
                        {
                            return NotFound("No existen proveedores disponibles para esta solicitud");
                        }
                    }
                    else
                    {
                        return NotFound("No se encontro informacion con este Uuid");
                    }


                }

            }
            catch (Exception ex)
            {
                Logger.LogError("Excepcion generada en ConsutlarVuelosUuid: " + ex.Message);
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
