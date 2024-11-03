using Botify.Logica;
using Microsoft.AspNetCore.Mvc;
namespace Botify.Web.Controllers;
public class ChatController : Controller
{
    private readonly IBotLogica _botLogica;

    public ChatController(IBotLogica botLogica)
    {
        _botLogica = botLogica;
    }

    [HttpGet]
    public IActionResult Chat()
    {
        return View();
    }
}
