using System;
using System.Collections.Generic;
using CsvHelper.Configuration.Attributes;

namespace VehicleRegistrationSystem.Models
{
    public class NewVehicle
    {
        [Name("numero do conteiner")]
        public string ContainerNumber { get; set; }

        [Name("chassi")]
        public string Chassis { get; set; }

        [Name("motor")]
        public string Engine { get; set; }

        // Propriedades adicionais para o payload da API
        public int VehicleModelCode { get; set; } = 27;
        public int BranchCode { get; set; } = 87;
        public string Color { get; set; } = "PRETO";
        public int ManufactureYear { get; set; } = 2025;
        public int ModelYear { get; set; } = 2025;
        public int Country { get; set; } = 144;
    }

    // Classe para representar o payload completo da API de veículos
    public class VehicleApiPayload
    {
        public string Chassis { get; set; }
        public int VehicleModelCode { get; set; }
        public int BranchCode { get; set; }
        public string Color { get; set; }
        public int ManufactureYear { get; set; }
        public int ModelYear { get; set; }
        public string EngineNumber { get; set; }
        public object OwnerRegistrationId { get; set; } = null;
        public int Country { get; set; }
        public VehicleMetadata Metadata { get; set; } = new VehicleMetadata();
    }

    public class VehicleMetadata
    {
        public object AdditionalProp1 { get; set; } = null;
        public object AdditionalProp2 { get; set; } = null;
        public object AdditionalProp3 { get; set; } = null;
    }
}
