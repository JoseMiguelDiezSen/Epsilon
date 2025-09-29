using Calipso.Security;
using Epsilon.Models.Comun;
using Epsilon.Renders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Negocio.Servicios;

namespace Epsilon.Controllers
{
    public class LoginController : AbstractSecurityController
    {
        private IGestionUsuarios _gestionUsuarios;
        private readonly IRazorRenderService _razorRenderService;


        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Constructor del controlador de la pantalla de Login
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
            JsonResponse jsonResponse = new JsonResponse("OK", "Operacion realizada correctamente");

            try
            {
                var user = _gestionUsuarios.Context.Usuarios.FirstOrDefault(u => u.Nombre == nombre && u.Password == password);

                if (user != null)
                {
                    return Json(new { success = true, redirectUrl = Url.Action("Index", "Home") });
                }
                return Json(new { success = false, message = "Usuario o contraseña incorrectos." });
            }

            catch (Exception ex) {
                //jsonResponse.ErrorData = "";
                //jsonResponse.Status = "False";
                //jsonResponse.StatusMessage = "";
                //jsonResponse.Data = "";
                
                return Json(new JsonResponse("ERROR", "Ha surgido un problema en la operacion.");
            }
        }
    }
}
