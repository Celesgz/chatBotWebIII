using Botify.Logica;
using Microsoft.AspNetCore.Mvc;

namespace Botify.Web.Controllers
{
    public class ChatController : Controller
    {
        private readonly IBotLogica _botLogica;
        public ChatController(IBotLogica botLogica)
        {
            _botLogica = botLogica;
        }
        public async Task<IActionResult> EnviarMensaje()
        {
            var artista = await _botLogica.RecomendarArtista();
            return View("Chat", artista);
        }

        public IActionResult Chat()
        {
            return View();
        }
    }
}
