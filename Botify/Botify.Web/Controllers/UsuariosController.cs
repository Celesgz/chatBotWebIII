using Botify.Data;
using Botify.Logica;
using API.Spotify.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Botify.Logica;
using Botify.Data.EF;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;

namespace Botify.Web.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly AuthService _authService;
        private readonly IUsuariosLogica _usuariosLogica;
        private readonly PasswordHasher<Usuario> _passwordHasher = new PasswordHasher<Usuario>();

        public UsuariosController(AuthService authService, IUsuariosLogica usuariosLogica)
        {
            _authService = authService;
            _usuariosLogica = usuariosLogica;
        }

        [HttpPost]
        public async Task<IActionResult> IniciarSesion(Usuario usuario)
        {
            var usuarioDb = await _usuariosLogica.BuscarUsuarioPorEmail(usuario.Email);

            if (usuarioDb != null)
            {
                var resultado = _passwordHasher.VerifyHashedPassword(usuarioDb, usuarioDb.Password, usuario.Password);

                if (resultado == PasswordVerificationResult.Success)
                {
                    var token = _authService.GenerarToken(usuario.Email);

                    if (token == null)
                    {
                        ModelState.AddModelError(string.Empty, "Error al generar el token. Inténtelo nuevamente.");
                        return View(usuario);
                    }

                    Response.Cookies.Append("AuthToken", token, new CookieOptions
                    {
                        HttpOnly = true,             
                        Secure = true,               
                        Expires = DateTimeOffset.Now.AddMinutes(60) 
                    });

                    return RedirectToAction("Chat", "Chat");
                }
            }

            ModelState.AddModelError(string.Empty, "Credenciales inválidas o usuario no encontrado.");
            return View(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Registrarse(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                Usuario? encontrado = await _usuariosLogica.BuscarUsuarioPorEmail(usuario.Email);

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

        public IActionResult Logout()
        {
            Response.Cookies.Delete("AuthToken");

            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult IniciarSesion()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Registrarse()
        {
            return View();
        }
    }
}



