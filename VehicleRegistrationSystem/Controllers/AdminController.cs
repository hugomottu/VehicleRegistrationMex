using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VehicleRegistrationSystem.Models;
using VehicleRegistrationSystem.Services;

namespace VehicleRegistrationSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly SettingsService _settingsService;

        public AdminController(SettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public async Task<IActionResult> Index()
        {
            var settings = await _settingsService.GetSettingsAsync();
            return View(settings);
        }

        [HttpPost]
        public async Task<IActionResult> SaveSettings(AppSettings settings)
        {
            if (ModelState.IsValid)
            {
                await _settingsService.SaveSettingsAsync(settings);
                TempData["SuccessMessage"] = "Configurações salvas com sucesso!";
                return RedirectToAction("Index");
            }

            return View("Index", settings);
        }
    }
}
