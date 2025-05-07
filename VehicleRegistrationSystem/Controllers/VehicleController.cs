using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using VehicleRegistrationSystem.Models;
using VehicleRegistrationSystem.Services;

namespace VehicleRegistrationSystem.Controllers
{
    public class VehicleController : Controller
    {
        private readonly VehicleService _vehicleService;
        private readonly SettingsService _settingsService;

        public VehicleController(VehicleService vehicleService, SettingsService settingsService)
        {
            _vehicleService = vehicleService;
            _settingsService = settingsService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SolicitacaoPlaca()
        {
            return View();
        }

        public IActionResult NovosVeiculos()
        {
            return View(new List<NewVehicle>());
        }

        [HttpPost]
        public IActionResult NovosVeiculos(IFormFile csvFile, int branchCode = 87, string color = "PRETO", int modelYear = 2025)
        {
            if (csvFile == null || csvFile.Length == 0)
            {
                ModelState.AddModelError("csvFile", "Por favor, selecione um arquivo CSV para upload.");
                return View(new List<NewVehicle>());
            }

            var csvService = new CsvService();
            var vehicles = new List<NewVehicle>();

            try
            {
                using (var stream = csvFile.OpenReadStream())
                {
                    vehicles = csvService.ProcessCsvFile(stream);
                }

                if (vehicles.Count == 0)
                {
                    ModelState.AddModelError("csvFile", "Não foi possível processar o arquivo CSV ou o arquivo está vazio.");
                }
                else
                {
                    foreach (var vehicle in vehicles)
                    {
                        vehicle.BranchCode = branchCode;
                        vehicle.Color = color;
                        vehicle.ModelYear = modelYear;
                        vehicle.ManufactureYear = modelYear;
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("csvFile", $"Erro ao processar o arquivo: {ex.Message}");
            }

            return View(vehicles);
        }

        [HttpGet]
        public IActionResult VeiculosRegistrados()
        {
            // Retorna a view sem dados, o JavaScript carregará os veículos do localStorage
            return View(new List<VehicleInfo>());
        }

        [HttpGet]
        public IActionResult NovoVeiculo()
        {
            return View(new NewVehicle
            {
                BranchCode = 87,
                Color = "PRETO",
                ModelYear = 2025,
                ManufactureYear = 2025
            });
        }

        [HttpPost]
        public async Task<IActionResult> SalvarNovoVeiculo(NewVehicle vehicle, bool callApi = false)
        {
            if (ModelState.IsValid)
            {
                if (callApi)
                {
                    // Se a opção de chamar a API estiver marcada, registra o veículo na API
                    var vehicleRegistrationService = new VehicleRegistrationService(_settingsService);
                    var (success, message, response) = await vehicleRegistrationService.RegisterVehicleAsync(vehicle);
                    
                    if (success)
                    {
                        TempData["SuccessMessage"] = "Veículo registrado com sucesso na API!";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = $"Erro ao registrar veículo na API: {message}";
                    }
                }
                else
                {
                    // Se não, apenas salva localmente (via JavaScript)
                    TempData["SuccessMessage"] = "Veículo salvo localmente com sucesso!";
                }
                
                return RedirectToAction("VeiculosRegistrados");
            }
            
            return View("NovoVeiculo", vehicle);
        }

        [HttpPost]
        public async Task<IActionResult> ProcessarVeiculos(string vehicleIdsText)
        {
            if (string.IsNullOrWhiteSpace(vehicleIdsText))
            {
                return BadRequest("A lista de IDs de veículos não pode estar vazia.");
            }

            // Processa o texto de entrada para extrair os IDs de veículos
            var vehicleIds = ProcessVehicleIdsInput(vehicleIdsText);
            
            var results = new List<VehicleProcessResult>();

            foreach (var id in vehicleIds)
            {
                var result = new VehicleProcessResult { VehicleId = id };
                
                try
                {
                    // Chama o serviço para obter as informações do veículo
                    var vehicle = await _vehicleService.GetVehicleInfoAsync(id);
                    
                    // Verificar se o veículo foi encontrado
                    if (vehicle != null)
                    {
                        // Buscar URL do anexo
                        var anexoUrl = await _vehicleService.GetAnexoUrlAsync(id);
                        
                        // Enviar para o job
                        await JobSender.SendJobAsync(vehicle, anexoUrl, _settingsService);
                        
                        result.Success = true;
                        result.Message = "Processado com sucesso";
                        result.VehicleInfo = vehicle;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = "Veículo não encontrado ou erro na API";
                    }
                }
                catch (Exception ex)
                {
                    result.Success = false;
                    result.Message = $"Erro ao processar: {ex.Message}";
                }
                
                results.Add(result);
            }

            return Json(results);
        }

        private List<string> ProcessVehicleIdsInput(string input)
        {
            // Remove caracteres desnecessários e divide a entrada em linhas
            var lines = input.Split(new[] { '\r', '\n', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            
            var vehicleIds = new List<string>();
            
            foreach (var line in lines)
            {
                // Remove espaços e aspas
                var trimmedLine = line.Trim().Trim('"', '\'');
                
                // Verifica se a linha parece ser um ID válido
                if (!string.IsNullOrWhiteSpace(trimmedLine) && trimmedLine.Length >= 32)
                {
                    vehicleIds.Add(trimmedLine);
                }
            }
            
            return vehicleIds;
        }
    }

    // Classe para representar o resultado do processamento de um veículo
    public class VehicleProcessResult
    {
        public string? VehicleId { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
        public VehicleInfo? VehicleInfo { get; set; }
    }
}
