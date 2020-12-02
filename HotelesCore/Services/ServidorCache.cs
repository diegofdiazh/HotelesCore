using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelesCore.Controllers;
using HotelesCore.Interfaces;
using HotelesCore.Models.DTOs;
using HotelesCore.Models.Responses;


namespace HotelesCore.Services
{
    public class ServidorCache
    {
        private readonly IAppLogger<ServidorCache> _logger;
        public ServidorCache(IAppLogger<ServidorCache> logger)
        {
            _logger = logger;
        }

        public Root getCache(string llave)
        {
            try
            {
                _logger.LogInformation($"SOLICITUD OBTENCIO CACHE CON LA SIGUIENTE {llave}");
                var client = new RestClient("http://host.docker.internal:32793/get/" + llave);
                Root responseVuelos = null;
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                IRestResponse response = client.Execute(request);
                if (response.IsSuccessful)
                {
                    string test = response.Content;
                    string test2 = test.Substring(1, test.Length - 1)
                        .Replace("\\n", "").Replace("\\", "");
                    string test3 = test2.Remove(test2.Length - 1);
                    string test4 = test3.Remove(test3.Length - 1);
                    string test5 = test4.Remove(0, 1);
                    _logger.LogInformation($"VALOR OBTENIDO {test5}");
                    responseVuelos = JsonConvert.DeserializeObject<Root>(test5);
                }
                _logger.LogInformation($"NO SE OBTUVO NADA");
                return responseVuelos;
            }
            catch (Exception ex)
            {
                _logger.LogError("SE REVENTO LA JODA MI PAPA EN GET:" + ex.Message);
                throw;
            }
           
        }
        public Root1 getCacheReserva(string llave)
        {
            try
            {
                _logger.LogInformation($"SOLICITUD OBTENCIO CACHE CON LA SIGUIENTE {llave}");
                var client = new RestClient("http://host.docker.internal:32793/get/" + llave);
                Root1 responseVuelos = null;
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                IRestResponse response = client.Execute(request);
                if (response.IsSuccessful)
                {
                    string test = response.Content;
                    string test2 = test.Substring(1, test.Length - 1)
                        .Replace("\\n", "").Replace("\\", "");
                    string test3 = test2.Remove(test2.Length - 1);
                    string test4 = test3.Remove(test3.Length - 1);
                    string test5 = test4.Remove(0, 1);
                    _logger.LogInformation($"VALOR OBTENIDO {test5}");
                    responseVuelos = JsonConvert.DeserializeObject<Root1>(test5);
                }
                _logger.LogInformation($"NO SE OBTUVO NADA");
                return responseVuelos;
            }
            catch (Exception ex)
            {
                _logger.LogError("SE REVENTO LA JODA MI PAPA EN GET RESERVA:" + ex.Message);
                throw;
            }

        }
        public bool setCache(string msgnormalizado, string llave)
        {
            try
            {
                _logger.LogInformation($"SOLICITUD NORMALIZADA {msgnormalizado}");
                var client = new RestClient("http://host.docker.internal:32793/set/" + llave);
                client.Timeout = -1;

                RequestCache requestCache = new RequestCache
                {
                    request = msgnormalizado
                };
                var body2 = JsonConvert.SerializeObject(requestCache);
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", body2, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (response.IsSuccessful)
                {
                    _logger.LogInformation($"SOLICITUD CACHEADA {response.Content}");
                    return true;
                }
                _logger.LogInformation($"SOLICITUD NO CACHEADA");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError("SE REVENTO LA JODA MI PAPA EN SET:" + ex.Message);
                return false;
                throw;
            }
        }
    }
}
