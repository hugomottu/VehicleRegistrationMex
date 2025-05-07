using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleRegistrationSystem.Models;
using VehicleRegistrationSystem.Services;

namespace VehicleRegistrationSystem.Modules
{
    /// <summary>
    /// Módulo responsável pela listagem e gerenciamento de requisições
    /// </summary>
    public class RequestListingModule
    {
        private readonly RequestService _requestService;
        private readonly SettingsService _settingsService;

        public RequestListingModule(SettingsService settingsService)
        {
            _settingsService = settingsService;
            _requestService = new RequestService(settingsService);
        }

        /// <summary>
        /// Lista as requisições com opções de paginação e filtro por status
        /// </summary>
        /// <param name="page">Número da página atual</param>
        /// <param name="pageSize">Quantidade de itens por página</param>
        /// <param name="status">Status das requisições para filtrar (opcional)</param>
        /// <returns>Tarefa assíncrona que retorna a lista de requisições</returns>
        public async Task<List<RequestInfo>> ListRequestsAsync(int page = 1, int pageSize = 10, string? status = null)
        {
            try
            {
                var response = await _requestService.GetRequestsAsync(page, pageSize, status);
                
                if (response == null || !response.success)
                {
                    Console.WriteLine($"Erro ao listar requisições: {response?.message ?? "Resposta nula da API"}");
                    return new List<RequestInfo>();
                }

                // Exibe informações de paginação
                if (response.pagination != null)
                {
                    Console.WriteLine($"Página {response.pagination.currentPage} de {response.pagination.totalPages}");
                    Console.WriteLine($"Total de itens: {response.pagination.totalItems}");
                }

                return response.result ?? new List<RequestInfo>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exceção ao listar requisições: {ex.Message}");
                return new List<RequestInfo>();
            }
        }

        /// <summary>
        /// Exibe os detalhes de uma requisição específica pelo seu número
        /// </summary>
        /// <param name="requestNumber">Número da requisição</param>
        /// <returns>Tarefa assíncrona que retorna os detalhes da requisição</returns>
        public async Task<RequestInfo?> GetRequestDetailsAsync(string requestNumber)
        {
            try
            {
                var request = await _requestService.GetRequestDetailsAsync(requestNumber);
                
                if (request == null)
                {
                    Console.WriteLine($"Requisição {requestNumber} não encontrada ou erro na API.");
                    return null;
                }

                return request;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exceção ao obter detalhes da requisição: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Exibe os detalhes de uma requisição formatados para console
        /// </summary>
        /// <param name="request">Objeto com informações da requisição</param>
        public void DisplayRequestDetails(RequestInfo? request)
        {
            if (request == null)
            {
                Console.WriteLine("Nenhuma informação de requisição disponível.");
                return;
            }

            Console.WriteLine("\n===== DETALHES DA REQUISIÇÃO =====");
            Console.WriteLine($"Número: {request.requestNumber}");
            Console.WriteLine($"Status: {request.status}");
            Console.WriteLine($"Tipo: {request.requestType}");
            Console.WriteLine($"Descrição: {request.description}");
            Console.WriteLine($"Criado em: {request.createdAt}");
            Console.WriteLine($"Atualizado em: {request.updatedAt}");
            Console.WriteLine($"ID do Veículo: {request.vehicleId}");
            Console.WriteLine($"Código do Veículo: {request.vehicleCode}");

            if (request.details != null)
            {
                Console.WriteLine("\n--- Detalhes Adicionais ---");
                Console.WriteLine($"Atribuído para: {request.details.assignedTo}");
                Console.WriteLine($"Prioridade: {request.details.priority}");
                Console.WriteLine($"Data de vencimento: {request.details.dueDate}");
                
                if (request.details.attachments?.Count > 0)
                {
                    Console.WriteLine("\nAnexos:");
                    foreach (var attachment in request.details.attachments)
                    {
                        Console.WriteLine($"- {attachment}");
                    }
                }

                if (request.details.additionalInfo?.Count > 0)
                {
                    Console.WriteLine("\nInformações Adicionais:");
                    foreach (var info in request.details.additionalInfo)
                    {
                        Console.WriteLine($"- {info.Key}: {info.Value}");
                    }
                }
            }
            
            Console.WriteLine("===================================\n");
        }

        /// <summary>
        /// Exibe uma lista de requisições formatada para console
        /// </summary>
        /// <param name="requests">Lista de requisições</param>
        public void DisplayRequestList(List<RequestInfo>? requests)
        {
            if (requests == null || requests.Count == 0)
            {
                Console.WriteLine("Nenhuma requisição encontrada.");
                return;
            }

            Console.WriteLine("\n======== LISTA DE REQUISIÇÕES ========");
            Console.WriteLine(string.Format("{0,-15} {1,-15} {2,-15} {3,-20} {4,-30}",
                "Número", "Status", "Tipo", "Criado em", "Descrição"));
            Console.WriteLine(new string('-', 95));

            foreach (var request in requests)
            {
                string description = request.description?.Length > 25 
                    ? request.description.Substring(0, 22) + "..." 
                    : request.description ?? "";
                
                Console.WriteLine(string.Format("{0,-15} {1,-15} {2,-15} {3,-20} {4,-30}",
                    request.requestNumber,
                    request.status,
                    request.requestType,
                    request.createdAt.ToString("dd/MM/yyyy HH:mm"),
                    description));
            }
            
            Console.WriteLine("=====================================\n");
        }
    }
}
