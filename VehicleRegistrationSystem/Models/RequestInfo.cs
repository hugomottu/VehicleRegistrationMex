using System;
using System.Collections.Generic;

namespace VehicleRegistrationSystem.Models
{
    // Classe que representa uma resposta da API de requisições
    public class RequestResponse
    {
        public List<RequestInfo>? result { get; set; }
        public Pagination? pagination { get; set; }
        public bool success { get; set; }
        public string? message { get; set; }
    }

    // Classe que representa informações de uma requisição
    public class RequestInfo
    {
        public string? requestNumber { get; set; }
        public string? status { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime? updatedAt { get; set; }
        public string? vehicleId { get; set; }
        public int? vehicleCode { get; set; }
        public string? requestType { get; set; }
        public string? description { get; set; }
        public RequestDetails? details { get; set; }
    }

    // Classe que representa os detalhes de uma requisição
    public class RequestDetails
    {
        public string? assignedTo { get; set; }
        public string? priority { get; set; }
        public DateTime? dueDate { get; set; }
        public List<string>? attachments { get; set; }
        public Dictionary<string, string>? additionalInfo { get; set; }
    }

    // Classe que representa informações de paginação
    public class Pagination
    {
        public int totalItems { get; set; }
        public int currentPage { get; set; }
        public int itemsPerPage { get; set; }
        public int totalPages { get; set; }
    }
}
