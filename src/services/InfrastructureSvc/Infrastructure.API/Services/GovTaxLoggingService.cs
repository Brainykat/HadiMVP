using Infrastructure.API.Configurations;
using Infrastructure.API.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Dtos;
using System.Net.Http.Headers;

namespace Infrastructure.API.Services
{
  public class GovTaxLoggingService : IGovTaxLoggingService
  {
    private readonly ILogger<GovTaxLoggingService> logger;
    private readonly HttpClient httpClient;
    private readonly GovernmentAPIConfigs configs;
    public GovTaxLoggingService(ILogger<GovTaxLoggingService> logger, HttpClient httpClient,
      IOptions<GovernmentAPIConfigs> optionsMonitor)
    {
      configs = optionsMonitor.Value;
      this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
      this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
      httpClient.BaseAddress = new Uri($"{configs.Url}");
      httpClient.DefaultRequestHeaders.Accept.Clear();
      httpClient.DefaultRequestHeaders.Accept.Add(
          new MediaTypeWithQualityHeaderValue("application/json"));
    }
    public async Task<bool> LogTaxData(NewOrderEBDto dto)
    {
      try
      {
        var content = new StringContent(JsonConvert.SerializeObject(dto), System.Text.Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync($"...", content);
        if (response.IsSuccessStatusCode)
        {
          //TODO: update order Status in database....
        }
        return response.IsSuccessStatusCode;
      }
      catch (Exception ex)
      {
        logger.LogError($"{GetType().FullName}  ==> Error {ex.Message} inner {ex.InnerException?.InnerException}");
        return false;
      }
    }
  }
}
