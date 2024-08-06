using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ProyectoFE.DTOs;
using ProyectoFE.Models;
using System.Diagnostics;

namespace ProyectoFE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IOptions<ProyectoApiFE> _options;

        public HomeController(ILogger<HomeController> logger, IOptions<ProyectoApiFE> options)
        {
            _logger = logger;
            _options = options;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
