using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using VehicleRegistrationSystem.Constants;
using VehicleRegistrationSystem.Models;
using VehicleRegistrationSystem.Services;

namespace VehicleRegistrationSystem.Utils
{
    public static class JobSender
    {
        // Método assíncrono para enviar os dados do veículo para a API
        public static async Task SendJobAsync(VehicleInfo vehicleInfo, string? anexoUrl, SettingsService settingsService)
        {
            // Obter as configurações atualizadas
            var settings = await settingsService.GetSettingsAsync();
            
            var details = new VehicleDetails();

            var jsonToSend = new
            {
                xamlKey = details.xamlKey,
                InArguments = new
                {
                    email = details.Email,
                    niv = vehicleInfo.Chassis,
                    claveVehicular = details.ClaveVehicular,
                    anioModelo = details.AnioModelo,
                    rfc = details.Rfc,
                    primerApellido = details.PrimerApellido,
                    segundoApellido = details.SegundoApellido,
                    nombre = details.Nombre,
                    codigoPostal = details.CodigoPostal,
                    localidad = details.Localidad,
                    calle = details.Calle,
                    numeroExterior = details.NumeroExterior,
                    telefonoMovil = details.TelefonoMovil,
                    numeroMotor = vehicleInfo.Engine,
                    clase = details.Clase,
                    tipo = details.Tipo,
                    color = details.Color,
                    transmision = details.Transmision,
                    combustible = details.Combustible,
                    cilindros = details.Cilindros,
                    pasajeros = details.Pasajeros,
                    cc = details.Cc,
                    factura = "N/A",
                    fechaFactura = DateTime.Now.ToString("dd/MM/yyyy"),
                    importeDelVehiculoSinDescuentoMasIva = "0",
                    folioFiscalUuid1 = "",
                    folioFiscalUuid2 = "",
                    folioFiscalUuid3 = "",
                    folioFiscalUuid4 = "",
                    folioFiscalUuid5 = "",
                    rfcEmisor = details.RfcEmisor,
                    rfcReceptor = details.RfcReceptor,
                    totalDeLaFactura = "0",
                    urlIneAnverso = details.urlIneAnverso,
                    urlIneReverso = details.urlIneReverso,
                    urlContratoArrendamiento = details.urlContratoArrendamiento,
                    urlPoderNotarial1 = details.urlPoderNotarial1,
                    urlPoderNotarial2 = details.urlPoderNotarial2,
                    urlFactura = anexoUrl
                },
                action = details.action,
                referenceId = vehicleInfo.Chassis
            };

            // Criação de um cliente HTTP para enviar a requisição
            using var client = new HttpClient();
            // Adiciona o token de autorização no cabeçalho da requisição
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {settings.CoreJobApiToken}");
            // Serializa o objeto JSON para enviar como corpo da requisição
            var content = new StringContent(JsonConvert.SerializeObject(jsonToSend), Encoding.UTF8, "application/json");

            var response = await client.PostAsync(settings.CoreJobApiUrl, content);
            // Lê o resultado da resposta da API
            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine(response.IsSuccessStatusCode ? "Requisição enviada com sucesso!" : $"Erro: {response.StatusCode}{result}");
            Console.WriteLine(vehicleInfo.Chassis);
        }
    }
}