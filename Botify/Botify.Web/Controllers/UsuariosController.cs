using Botify.Data;
using Botify.Logica;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Botify.Logica;
using Botify.Data.EF;

namespace API.Spotify.Web.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly AuthService _authService;
        private readonly IUsuariosLogica _usuariosLogica;

        public UsuariosController(AuthService authService, IUsuariosLogica usuariosLogica)
        {
            _authService = authService;
            _usuariosLogica = usuariosLogica;
        }

        [HttpGet]
        public IActionResult IniciarSesion()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IniciarSesion(Usuario usuario)
        {
            var token = await _authService.AuthenticateAsync(usuario.Email, usuario.Password);

            if (token == null)
            {
                ModelState.AddModelError(string.Empty, "Credenciales inválidas o usuario no encontrado.");
                return View(usuario);
            } else
            {
                Console.WriteLine("token generado");
            // Guarda el token en TempData para enviarlo al frontend
            TempData["AuthToken"] = token;

            // Redirige a la acción Chat en el controlador Chat
            return RedirectToAction("Chat", "Chat");
            }

        }
    }
}

