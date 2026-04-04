using Calipso.Security;
using Epsilon.Renders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Negocio.Persistencia.Modelos;
using Negocio.Servicios;
using System;
using Google.Apis.Auth;

namespace Epsilon.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class LoginController : AbstractSecurityController
    {
        private IGestionUsuarios _gestionUsuarios;
        private readonly IRazorRenderService _razorRenderService;


        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Constructor d
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="seguridad"></param>
        /// <param name="gestionUsuarios"></param>
        public LoginController(ILogger<LoginController> logger, ISeguridad seguridad, IGestionUsuarios gestionUsuarios, IRazorRenderService renderService) : base(logger, seguridad)
        {
            _gestionUsuarios = gestionUsuarios;
            _razorRenderService = renderService;
        }

        [HttpPost]
        public JsonResult LoginUser(string nombre, string password)
        {
            var user = _gestionUsuarios.Context.Usuarios
                .FirstOrDefault(u => u.Nombre == nombre && u.Password == password);

            if (user != null)
            {
                if (user.FotoPerfil != null && user.FotoPerfil.Length > 0)
                {
                    var fotoBase64 = Convert.ToBase64String(user.FotoPerfil);
                    HttpContext.Session.SetString("FotoPerfil", fotoBase64);
                }
                else
                {
                    HttpContext.Session.SetString("FotoPerfil", "");
                }

                return Json(new { success = true, redirectUrl = Url.Action("Index", "Home") });
            }

            return Json(new { success = false, message = "Usuario o contraseña incorrectos" });
        }


        /// <summary>
        ///  Se ha metido libreria google auth con dotnet add package Google.Apis.Auth
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> LoginGoogle(string token)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(token);

                var email = payload.Email;
                var nombre = payload.Name;

                var user = _gestionUsuarios.Context.Usuarios
                    .FirstOrDefault(u => u.Email == email);

                if (user == null)
                {
                    // Crear usuario automáticamente
                    user = new Usuario
                    {
                        Nombre = nombre,
                        Email = email,
                        Activo = true,
                        FechaAlta = DateTime.Now
                    };

                    _gestionUsuarios.Context.Usuarios.Add(user);
                    _gestionUsuarios.Context.SaveChanges();
                }

                // Aquí podrías meter sesión si quieres
                HttpContext.Session.SetString("Usuario", user.Nombre);

                return Json(new
                {
                    success = true,
                    redirectUrl = Url.Action("Index", "Home")
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Error al validar el login con Google"
                });
            }
        }
    }
}
