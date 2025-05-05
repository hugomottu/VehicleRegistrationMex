using System.Text.Json.Serialization;

namespace VehicleRegistrationSystem.Models
{
    // Classe principal que representa a resposta da API de veículos
    public class VehicleInfo
    {
        // Contém os dados do veículo retornado pela API
        public string? ContainerNumber { get; set; }
        public string? Chassis { get; set; }
        public string? Engine { get; set; }
        public string? BranchCode { get; set; }
        public string? Color { get; set; }
        public string? ModelYear { get; set; }
        public string? DateRegistered { get; set; }
        public string? Status { get; set; } = "Pendente";
        public string? PlateStatus { get; set; } = "Novo";
        
        [JsonIgnore]
        public bool IsSuccess { get; set; }
        
        [JsonIgnore]
        public string? Message { get; set; }
        
        // Propriedades aninhadas para detalhes do veículo
        public VehicleDetailInfo? Details { get; set; }
    }

    // Classe que representa as informações detalhadas de um veículo
    public class VehicleDetailInfo
    {
        public string? VehicleId { get; set; }
        public string? VehicleModelCode { get; set; }
        public string? ManufactureYear { get; set; }
        public string? Country { get; set; }
        public string? PlateNumber { get; set; }
    }
}