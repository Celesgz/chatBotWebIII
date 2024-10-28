using Botify.Entidades;
using Botify.Logica;
using Botify.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Botify.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ITokenLogica _tokenLogica;

        public HomeController(ILogger<HomeController> logger, ITokenLogica tokenLogica)
        {
            _logger = logger;
            _tokenLogica = tokenLogica;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> ObtenerToken()
        {
            var token = await _tokenLogica.ObtenerToken();

            return View("MostrarToken", token);
        }


        public async Task<IActionResult> Conectar()
        {
            //string artistId = "1Xyo4u8uXC1ZmMpatF05PJ"; //The Weekend
            string artistId = "4dpARuHxo51G3z768sgnrY";

            var artistInfoJson = await _tokenLogica.ObtenerInformacionDelArtista(artistId);

            Console.WriteLine("JSON recibido:");
            Console.WriteLine(artistInfoJson);

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            var artista = JsonSerializer.Deserialize<Artista>(artistInfoJson, options);

            return View("MostrarArtista", artista);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
