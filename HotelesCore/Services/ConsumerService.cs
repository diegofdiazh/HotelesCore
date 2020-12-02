using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotelesCore.Data;
using HotelesCore.Interfaces;
using HotelesCore.Models.DTOs;


namespace HotelesCore.Services
{
    public class ConsumerService : BackgroundService
    {
        private readonly ConsumerConfig _config;
        private readonly IConsumer<Null, string> _kafkaConsumer;
        private readonly ILogger<ConsumerService> _logger;
        private readonly IAppLogger<ServidorCache> _loggercache;
        public ConsumerService(ConsumerConfig config, ILogger<ConsumerService> logger, IAppLogger<ServidorCache> loggerCache)
        {
            _config = config;
            _kafkaConsumer = new ConsumerBuilder<Null, string>(_config).Build();
            _logger = logger;
            _loggercache = loggerCache;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            new Thread(() => StartConsumerLoop(stoppingToken)).Start();

            return Task.CompletedTask;
        }

        private void StartConsumerLoop(CancellationToken cancellationToken)
        {
            _kafkaConsumer.Subscribe("topic-3-normalizer");

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var cr = this._kafkaConsumer.Consume(cancellationToken);
                    _logger.LogInformation("Se obtiene respuesta de normalizador :" + cr.Message.Value);
                    JObject jResults = JObject.Parse(cr.Message.Value);
                    if (jResults.Count == 4)
                    {
                        var jitem = jResults.Children<JProperty>().FirstOrDefault(x => x.Name == "providerType");
                        if (jitem.Value.ToString() == "HOTEL")
                        {
                            var jitemType = jResults.Children<JProperty>().FirstOrDefault(x => x.Name == "processType");
                            if (jitemType.Value.ToString() == "CATALOG")
                            {
                                var vuelos = JsonConvert.DeserializeObject<Root>(cr.Message.Value);
                                ServidorCache servidorCache = new ServidorCache(_loggercache);
                                servidorCache.setCache(cr.Message.Value, vuelos.uuid + "_" + vuelos.providerType + "_" + vuelos.processType);
                            }
                            else if (jitemType.Value.ToString() == "RESERVE")
                            {
                                var vuelos = JsonConvert.DeserializeObject<Root1>(cr.Message.Value);
                                ServidorCache servidorCache = new ServidorCache(_loggercache);
                                servidorCache.setCache(cr.Message.Value, vuelos.uuid + "_" + vuelos.providerType + "_" + vuelos.processType);
                            }

                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (ConsumeException e)
                {
                    // Consumer errors should generally be ignored (or logged) unless fatal.
                    _logger.LogWarning("Consume error: {0}", e.Error.Reason);
                    if (e.Error.IsFatal)
                    {
                        break;
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError("Consume error: {0}", e);
                    break;
                }
            }
        }
    }
}
