using System;

namespace VehicleRegistrationSystem.Models
{
    public class AppSettings
    {
        public string VehicleApiToken { get; set; }
        public string CoreJobApiToken { get; set; }
        public string VehicleApiBaseUrl { get; set; }
        public string AnexosApiBaseUrl { get; set; }
        public string CoreJobApiUrl { get; set; }
        public string RequestApiBaseUrl { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
