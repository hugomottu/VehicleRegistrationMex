using Newtonsoft.Json;
using VehicleRegistrationSystem.Constants;
using VehicleRegistrationSystem.Models;

namespace VehicleRegistrationSystem.Services
{
    public class RequestService
    {
        private readonly HttpClient _httpClient;

        public RequestService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {AppConstants.VehicleApiToken}");
        }

        /// <summary>
        /// Obtém a lista de requisições com paginação
        /// </summary>
        /// <param name="page">Número da página atual</param>
        /// <param name="pageSize">Quantidade de itens por página</param>
        /// <param name="status">Status da requisição (opcional)</param>
        /// <returns>Resposta contendo a lista de requisições e informações de paginação</returns>
        public async Task<RequestResponse?> GetRequestsAsync(int page = 1, int pageSize = 10, string? status = null)
        {
            string url = $"{AppConstants.RequestApiBaseUrl}?page={page}&pageSize={pageSize}";
            
            if (!string.IsNullOrEmpty(status))
            {
                url += $"&status={status}";
            }

            var response = await _httpClient.GetAsync(url);
            
            if (!response.IsSuccessStatusCode) return null;
            
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<RequestResponse>(json);
        }

        /// <summary>
        /// Obtém os detalhes de uma requisição específica pelo número
        /// </summary>
        /// <param name="requestNumber">Número da requisição</param>
        /// <returns>Informações detalhadas da requisição</returns>
        public async Task<RequestInfo?> GetRequestDetailsAsync(string requestNumber)
        {
            var response = await _httpClient.GetAsync($"{AppConstants.RequestApiBaseUrl}/{requestNumber}");
            
            if (!response.IsSuccessStatusCode) return null;
            
            var json = await response.Content.ReadAsStringAsync();
            var requestResponse = JsonConvert.DeserializeObject<RequestResponse>(json);
            
            return requestResponse?.result?.FirstOrDefault();
        }
    }
}
