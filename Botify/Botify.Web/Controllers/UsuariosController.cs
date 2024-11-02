using Botify.Data;
using Botify.Logica;
using API.Spotify.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Botify.Logica;
using Botify.Data.EF;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

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

            // Llamar al servicio de autenticación para validar credenciales y generar token
            var token = await _authService.AuthenticateAsync(usuario.Email, usuario.Password);

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

            return RedirectToAction("Chat", "Chat");
        }

        [HttpGet]
        public IActionResult Registrarse()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrarse(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                Usuario? encontrado = await _usuariosLogica.BuscarUsuario(usuario);
                if (encontrado == null)
                {
                    try
                    {
                        await _usuariosLogica.AgregarUsuario(usuario);
                        ViewBag.Message = "Usuario registrado. Ahora podés iniciar sesión";
                    }
                    catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2627)
                    {
                        if (sqlEx.Message.Contains("UQ_Usuario_Nombre"))
                        {
                            ModelState.AddModelError("Nombre", "El nombre de usuario ya existe. Por favor, elegí otro.");
                        }
                        else if (sqlEx.Message.Contains("UQ_Usuario_Email"))
                        {
                            ViewBag.Message = "El e-mail ya está registrado. ¿Querés iniciar sesión?";
                            ModelState.AddModelError("Email", "El e-mail ya está registrado");
                        }
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", "Ocurrió un error inesperado. Intenta de nuevo.");
                    }
                }
                else
                {
                    ViewBag.Message = "El usuario ya existe, ¿querés iniciar sesión?";
                }
            }

            return View(usuario);
        }

    }
}

