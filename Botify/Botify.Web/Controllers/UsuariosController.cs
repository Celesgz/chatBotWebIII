using Botify.Data;
using Botify.Logica;
using API.Spotify.Web.Models;
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
            UsuarioModelView model = new UsuarioModelView
            {
                Email = usuario.Email,
                Password = usuario.Password
            };

            // Llamar al servicio de autenticación para validar credenciales y generar token
            var token = await _authService.AuthenticateAsync(model.Email, model.Password);

            if (token == null)
            {
                ModelState.AddModelError(string.Empty, "Credenciales inválidas o usuario no encontrado.");
                return View(usuario); // Puedes redirigir a una vista de error o mostrar el error en la vista actual
            }

            // Configurar la cookie con el token
            Response.Cookies.Append("AuthToken", token, new CookieOptions
            {
                HttpOnly = true,
                //Secure = true,
                //SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.Now.AddMinutes(60)
            });

            return RedirectToAction("IngresoOk");
        }
        [HttpGet]
        //[Authorize(AuthenticationSchemes = "CookieAuth")]
        public IActionResult IngresoOk()
        {
            return View();
        }
    }
}

