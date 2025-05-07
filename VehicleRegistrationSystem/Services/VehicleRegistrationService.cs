using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VehicleRegistrationSystem.Constants;
using VehicleRegistrationSystem.Models;
using VehicleRegistrationSystem.Services;

namespace VehicleRegistrationSystem.Services
{
    public class VehicleRegistrationService
    {
        private readonly HttpClient _httpClient;
        private readonly SettingsService _settingsService;

        public VehicleRegistrationService(SettingsService settingsService)
        {
            _httpClient = new HttpClient();
            _settingsService = settingsService;
        }

        public async Task<(bool Success, string Message, object Response)> RegisterVehicleAsync(NewVehicle vehicle)
        {
            try
            {
                // Obter as configurações atualizadas
                var settings = await _settingsService.GetSettingsAsync();
                
                // Configurar o token de autorização para cada requisição
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", settings.VehicleApiToken);
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                // Criar o payload para a API
                var payload = new VehicleApiPayload
                {
                    Chassis = vehicle.Chassis,
                    VehicleModelCode = vehicle.VehicleModelCode,
                    BranchCode = vehicle.BranchCode,
                    Color = vehicle.Color,
                    ManufactureYear = vehicle.ManufactureYear,
                    ModelYear = vehicle.ModelYear,
                    EngineNumber = vehicle.Engine,
                    Country = vehicle.Country,
                    Metadata = new VehicleMetadata()
                };

                // Serializar o payload para JSON
                var jsonContent = JsonConvert.SerializeObject(payload);
                Console.WriteLine($"Enviando payload para API: {jsonContent}");

                // Criar o conteúdo da requisição
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Enviar a requisição para a API
                var response = await _httpClient.PostAsync(settings.VehicleApiBaseUrl, content);
                
                // Ler a resposta
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Resposta da API: Status={response.StatusCode}, Conteúdo={responseContent}");

                // Verificar se a requisição foi bem-sucedida
                if (response.IsSuccessStatusCode)
                {
                    var responseObject = JsonConvert.DeserializeObject(responseContent);
                    return (true, "Veículo registrado com sucesso!", responseObject);
                }
                else
                {
                    return (false, $"Erro ao registrar veículo: {response.StatusCode} - {responseContent}", null);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exceção ao registrar veículo: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return (false, $"Erro ao registrar veículo: {ex.Message}", null);
            }
        }
    }
}
