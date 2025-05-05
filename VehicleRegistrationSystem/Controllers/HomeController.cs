using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VehicleRegistrationSystem.Models;

namespace VehicleRegistrationSystem.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manutencao()
        {
            return View();
        }

        public IActionResult Vistorias()
        {
            return View();
        }

        public IActionResult PatioInterno()
        {
            return View();
        }

        public IActionResult Performance()
        {
            return View();
        }

        public IActionResult Estoque()
        {
            return View();
        }

        public IActionResult Planos()
        {
            return View();
        }

        public IActionResult Ideias()
        {
            return View();
        }

        public IActionResult Bino()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
