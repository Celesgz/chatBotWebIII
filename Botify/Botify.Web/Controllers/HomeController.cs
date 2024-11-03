using Botify.Entidades;
using Botify.Logica;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Botify.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IBotLogica _botLogica;

        public HomeController(ILogger<HomeController> logger, IBotLogica botLogica)
        {
            _logger = logger;
            _botLogica = botLogica;
        }

        public IActionResult Index()
        {
            return View();
        }

    }
}
