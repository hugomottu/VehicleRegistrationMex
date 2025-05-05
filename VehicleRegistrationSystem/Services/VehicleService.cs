using Newtonsoft.Json;
using System.Net.Http.Headers;
using VehicleRegistrationSystem.Constants;
using VehicleRegistrationSystem.Models;

namespace VehicleRegistrationSystem.Services
{
    public class VehicleService // Classe responsável pelos serviços de consulta de veículos
    {
        private readonly HttpClient _httpClient; // Cliente HTTP usado para fazer requisições para a API

        // Construtor da classe, inicializando o HttpClient e configurando o cabeçalho de autorização
        public VehicleService() 
        {
            _httpClient = new HttpClient();
            // Adiciona o token de autorização no cabeçalho da requisição
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {AppConstants.VehicleApiToken}");
        }

        // Método assíncrono para buscar as informações do veículo com base no ID do veículo
        public async Task<VehicleInfo?> GetVehicleInfoAsync(string vehicleId)
        {
            try
            {
                Console.WriteLine($"Buscando informações do veículo com ID: {vehicleId}");
                // Faz a requisição GET para buscar as informações do veículo usando o ID
                var response = await _httpClient.GetAsync(AppConstants.VehicleApiBaseUrl + vehicleId);
                
                // Registra o status da resposta
                Console.WriteLine($"Status da resposta: {(int)response.StatusCode} - {response.StatusCode}");
                
                // Verifica se a resposta da API foi bem-sucedida
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Erro na API: {errorContent}");
                    return null;
                }
                
                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Resposta da API recebida com sucesso");
                
                // Deserializa o JSON para o modelo ApiVehicleResponse
                var apiResponse = JsonConvert.DeserializeObject<ApiVehicleResponse>(json);
                
                // Criar um novo objeto VehicleInfo com os dados da resposta da API
                var vehicleInfo = new VehicleInfo
                {
                    Chassis = apiResponse?.result?.vehicleId,
                    Engine = "N/A", // Não temos essa informação na resposta da API
                    ContainerNumber = "N/A", // Não temos essa informação na resposta da API
                    BranchCode = "87",
                    Color = "PRETO",
                    ModelYear = apiResponse?.result?.manufactureYear,
                    DateRegistered = DateTime.Now.ToString("dd/MM/yyyy"),
                    Status = "Pendente",
                    PlateStatus = "Novo"
                };
                
                return vehicleInfo;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exceção ao buscar informações do veículo: {ex.Message}");
                return null;
            }
        }

        // Método assíncrono para buscar a URL do anexo de um veículo com base no código do veículo
        public async Task<string?> GetAnexoUrlAsync(string vehicleId)
        {
            try
            {
                Console.WriteLine($"Buscando anexos para o veículo com ID: {vehicleId}");
                // Faz a requisição GET para buscar os anexos do veículo usando o ID do veículo
                var response = await _httpClient.GetAsync(AppConstants.AnexosApiBaseUrl + $"{vehicleId}/14");
                
                // Registra o status da resposta
                Console.WriteLine($"Status da resposta de anexos: {(int)response.StatusCode} - {response.StatusCode}");
                
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Erro na API de anexos: {errorContent}");
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Resposta da API de anexos recebida com sucesso");
                
                dynamic data = JsonConvert.DeserializeObject(json);
                var url = data?.dataResult?.Count > 0 ? data.dataResult[0].url.ToString() : null;
                
                if (string.IsNullOrEmpty(url))
                {
                    Console.WriteLine("Nenhum anexo encontrado para o veículo");
                }
                else
                {
                    Console.WriteLine($"URL do anexo encontrada: {url}");
                }
                
                return url;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exceção ao buscar anexos do veículo: {ex.Message}");
                return null;
            }
        }
    }
}