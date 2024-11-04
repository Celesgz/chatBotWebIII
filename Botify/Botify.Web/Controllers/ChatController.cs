using Botify.Logica;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Botify.Web.Controllers;

[Authorize]
public class ChatController : Controller
{
    private readonly IBotLogica _botLogica;

    public ChatController(IBotLogica botLogica)
    {
        _botLogica = botLogica;
    }

    [Authorize]
    [HttpGet]
    public IActionResult Chat()
    {
        return View();
    }
}
