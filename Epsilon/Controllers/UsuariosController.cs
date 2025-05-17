
using Calipso.Security;
using Epsilon.Attributes;
using Epsilon.Models.Comun;
using Epsilon.Models;
using Epsilon.Renders;
using Epsilon.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Negocio.Persistencia.Modelos;
using Negocio.Servicios;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Epsilon.Controllers
{
    public class UsuariosController : AbstractSecurityController
    {
        private IGestionUsuarios _gestionUsuarios;
        private readonly IRazorRenderService _razorRenderService;

        /// <summary>
        /// Constructor d
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="seguridad"></param>
        /// <param name="gestionUsuarios"></param>
        public UsuariosController(ILogger<UsuariosController> logger, ISeguridad seguridad, IGestionUsuarios gestionUsuarios, IRazorRenderService renderService) : base(logger, seguridad)
        {
            _gestionUsuarios = gestionUsuarios;
            _razorRenderService = renderService;
        }

        /// <summary>
        /// Método principal de entrada a la pantalla de Planificacion de Usuarios 
        /// </summary>
        /// <returns> Metodo de acceso al controlador </returns>
        public IActionResult Index()
        {
            UsuariosViewModel vmUsuarios = new UsuariosViewModel();
            IQueryable<DatosUsuario> datosUsuario = _gestionUsuarios.GetDatosUsuario();
            IEnumerable<ViewUsuario> usuarios = new List<ViewUsuario>();

            datosUsuario = datosUsuario.Skip((vmUsuarios.PaginaActual - 1) * vmUsuarios.RegistrosPorPagina).Take(vmUsuarios.RegistrosPorPagina);

            if (datosUsuario.Any())
            {
                usuarios = datosUsuario.Select(x => new ViewUsuario(x)).ToList();
            }

            vmUsuarios.Usuarios = usuarios;
            return View("Index", vmUsuarios);
        }

        /// <summary> Metodo utilizado para el filtrado de periodos en funcion de los criterios seleccionados</summary>
        /// <param name="vmUsuarios"></param>
        /// <returns> Devuelve una lista de los periodos que coincidan con los datos introducidos</returns>
        [HttpPost, AjaxOnly]
        public async Task<JsonResult> FiltrarUsuariosAsync(UsuariosViewModel vmUsuarios)
        {
            JsonResponse? jsonResponse = null;

            try
            {
                var dbContext = _seguridad.Context;
                IQueryable<DatosUsuario> datosUsuario = _gestionUsuarios.GetDatosUsuario();

                if (vmUsuarios.Nombre != null)
                {
                    datosUsuario = datosUsuario.Where(p => (p.Nombre == vmUsuarios.Nombre));
                }
                if (vmUsuarios.Email != null)
                {
                    datosUsuario = datosUsuario.Where(p => (p.EMail == vmUsuarios.Email));
                }
                if (vmUsuarios?.Telefono != null)
                {
                    datosUsuario = datosUsuario.Where(p => (p.Telefono == vmUsuarios.Telefono));
                }

                vmUsuarios.Usuarios = await datosUsuario.
                    OrderBy(x => x.Nombre).
                    Skip((vmUsuarios.PaginaActual - 1) * vmUsuarios.RegistrosPorPagina).Take(vmUsuarios.RegistrosPorPagina).
                    Select(x => new ViewUsuario(x)).
                    ToListAsync();

                string data = await _razorRenderService.ToStringAsync("TablaUsuarios", vmUsuarios.Usuarios);
                jsonResponse = new JsonResponse("200", "Operación realizada correctamente.", data);
            }

            catch (Exception ex)
            {
                jsonResponse = new JsonResponse("500", "La operación no se pudo realizar.", string.Empty, "Error: " + ex.Message);
            }

            return new JsonResult(jsonResponse);
        }

        #region AgregarUsuario

        /// <summary>
        /// Método para abrir la ventana modal de agregar usuarios
        /// </summary>
        /// <returns></returns>

        [HttpGet, AjaxOnly]
        public async Task<ActionResult> ModalAgregarUsuario()
        {
            JsonResponse? jsonResponse = new JsonResponse("400", "Error en el servidor", "");
            ViewFormAgregarUsuario vmAgregarUsuario = new ViewFormAgregarUsuario();
            vmAgregarUsuario.FechaAlta = DateTime.Now;
            string data = await _razorRenderService.ToStringAsync("FormAddUser", vmAgregarUsuario);
            jsonResponse = new JsonResponse("200", "Operación realizada correctamente.", data);
            return new JsonResult(jsonResponse);
        }

        /// <summary>
        /// Método que contiene la funcionalidad de Añadir Periodos
        /// </summary>
        /// <param name="vmperiodo"></param>
        /// <returns></returns>
        [HttpPost, AjaxOnly]
        public async Task<JsonResult> AgregarUsuarioAsync(ViewFormAgregarUsuario vmUsuario)
        {
            JsonResult result = new JsonResult(new { StatusCode = 500, message = "No se pudo realizar la operación solicitada" });

            try
            {
                Usuario usuario = new Usuario()
                {
                    Nombre = vmUsuario.Nombre,
                    Password = vmUsuario.Password,
                    Email = vmUsuario.Email,
                    FechaAlta = vmUsuario.FechaAlta,
                    Telefono = vmUsuario.Telefono,
                    RutaFoto = vmUsuario.RutaFoto
                };

                _gestionUsuarios.AddUser(usuario);
                result = new JsonResult(new { StatusCode = 200, message = "Usuario agregado correctamente" });
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return result;
        }

        #endregion

        #region ModificarUsuario

        [HttpGet, AjaxOnly]
        public async Task<ActionResult> ModalModificarUsuarioAsync(int idUsuario)
        {
            JsonResponse? jsonResponse = new JsonResponse("400", "Error en el servidor", "");

            Usuario usuario = new Usuario();

            //Obtenemos el usuario
            usuario = _gestionUsuarios.Context.Usuarios.Where(x => x.IdUsuario == idUsuario).First();

            //Obtenmos los datos del usuario
            ViewFormAgregarUsuario vmModificarUsuario = new ViewFormAgregarUsuario();
            vmModificarUsuario.Nombre = usuario.Nombre;
            vmModificarUsuario.Password = usuario.Password;
            vmModificarUsuario.Email = usuario.Email;
            vmModificarUsuario.FechaAlta = usuario.FechaAlta; ;
            vmModificarUsuario.Telefono = usuario.Telefono;
            vmModificarUsuario.RutaFoto = usuario.RutaFoto;

            //Mostrar Modal
            string data = await _razorRenderService.ToStringAsync("FormUpdateUser", vmModificarUsuario);
            jsonResponse = new JsonResponse("200", "Operación realizada correctamente.", data);
            return new JsonResult(jsonResponse);
        }

        /// <summary>
        /// Método que contiene la funcionalidad de Modificar Periodos
        /// </summary>
        /// <param name="vmperiodo"></param>
        /// <returns></returns>
        [HttpPost, AjaxOnly]
        public async Task<JsonResult> ModificarUsuarioAsync(ViewFormAgregarUsuario vmUsuario)
        {
            JsonResult result = new JsonResult(new { StatusCode = 500, message = "No se pudo realizar la operación solicitada" });

            var periodo = _gestionUsuarios.GetUser(1);

            try
            {
                Usuario usuario = new Usuario()
                {
                    Nombre = vmUsuario.Nombre,
                    Password = vmUsuario.Password,
                    Email = vmUsuario.Email,
                    Telefono = vmUsuario.Telefono,
                    FechaAlta = DateTime.Now
                };

                var res = _gestionUsuarios.UpdateUser(usuario);
                result = new JsonResult(new { StatusCode = 200, message = "Usuario actualizado correctamente" });
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return result;
        }

        #endregion

        #region EliminarUsuario

        /// <summary>
        /// Metodo para eliminar un periodo de planificacion 
        /// </summary>
        /// <param name="id"> Identificador del usuario a eliminar </param>
        /// <returns></returns>
        [HttpGet, AjaxOnly]
        public async Task<JsonResult> EliminarUsuarioAsync(long idUsuario)
        {
            JsonResponse response = new JsonResponse("200", "Ok");
            try
            {
                //Usuario usuario = new Usuario();
                //usuario = _gestionUsuarios.Context.Usuarios.Where(x => x.IdUsuario == id).First();
                //_gestionUsuarios.DeleteUser(usuario.IdUsuario);
                //response.Data = "Usuario eliminado correctamente";
                JsonResponse? jsonResponse = new JsonResponse("400", "Error en el servidor", "");
                ViewFormAgregarUsuario vmAgregarUsuario = new ViewFormAgregarUsuario();
                vmAgregarUsuario.FechaAlta = DateTime.Now;
                string data = await _razorRenderService.ToStringAsync("FormDeleteUser", vmAgregarUsuario);
                jsonResponse = new JsonResponse("200", "Operación realizada correctamente.", data);
                return new JsonResult(jsonResponse);




            }
            catch (Exception ex)
            {
                response.Status = "500";
                response.StatusMessage = "Se ha producido un error al intentar eliminar el Usuario";
            }
            return new JsonResult(response);
        }

        #endregion
    }
}
