using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace VehicleRegistrationSystem.Models
{
    public class ApiVehicleResponse
    {
        public ApiVehicleResult? result { get; set; }
        public bool success { get; set; }
        public string? message { get; set; }
    }

    public class ApiVehicleResult
    {
        public int code { get; set; }
        public string? vehicleId { get; set; }
        public string? chassis { get; set; }
        public string? engineNumber { get; set; }
        public string? manufactureYear { get; set; }
        public ApiVehicleMetadata? metadata { get; set; }
    }

    public class ApiVehicleMetadata
    {
        public string? Invoice { get; set; }
        public string? Invoice_UUID { get; set; }
        public string? TotalAmount { get; set; }
        public DateTime? ManufactureDate { get; set; }
    }
}
