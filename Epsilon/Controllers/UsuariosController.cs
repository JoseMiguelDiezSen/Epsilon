
using Calipso.Security;
using Epsilon.Attributes;
using Epsilon.Models;
using Epsilon.Models.Comun;
using Epsilon.Renders;
using Epsilon.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Negocio.Excepciones;
using Negocio.Persistencia.Modelos;
using Negocio.Servicios;
using OfficeOpenXml;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json;

namespace Epsilon.Controllers
{
    /// <summary>
    /// Clase encargada de gestionar las funcionalidades de los Usuarios
    /// </summary>
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
                usuarios = datosUsuario.Select(x => new ViewUsuario(x)).ToList().OrderBy(x => x.IdUsuario);
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

                if (!string.IsNullOrWhiteSpace(vmUsuarios.Nombre))
                {
                    datosUsuario = datosUsuario.Where(p => p.Nombre == vmUsuarios.Nombre);
                }
                if (!string.IsNullOrWhiteSpace(vmUsuarios.Email))
                {
                    datosUsuario = datosUsuario.Where(p => p.EMail == vmUsuarios.Email);
                }
                if (vmUsuarios.Telefono > 0)
                {
                    datosUsuario = datosUsuario.Where(p => p.Telefono == vmUsuarios.Telefono);
                }


                vmUsuarios.Usuarios = await datosUsuario.
                    OrderBy(x => x.IdUsuario)
                    .Skip((vmUsuarios.PaginaActual - 1) * vmUsuarios.RegistrosPorPagina)
                    .Take(vmUsuarios.RegistrosPorPagina).
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
            //vmAgregarUsuario.TurnoDeTrabajo = new SelectList(_gestionUsuarios.Context.Usuarios.ToList(), nameof(Usuario.IdUsuario), nameof(Usuario.TurnoDeTrabajo));
            string data = await _razorRenderService.ToStringAsync("FormAddUser", vmAgregarUsuario);
            jsonResponse = new JsonResponse("200", "Operación realizada correctamente.", data);
            return new JsonResult(jsonResponse);
        }

        /// <summary>
        /// Método que contiene la funcionalidad de Añadir Usuarios
        /// </summary>
        /// <param name="vmUsuario"></param>
        /// <returns></returns>
        [HttpPost, AjaxOnly]
        public async Task<JsonResult> AgregarUsuarioAsync(ViewFormAgregarUsuario vmUsuario)
        {
            JsonResult result = new JsonResult(new { StatusCode = 500, message = "Nao se pudo realizar la operación solicitada" });
            JsonResponse jsonResponse = new JsonResponse("400", "Error de servidor al realizar la operacion");

            try
            {
                Usuario usuario = new Usuario()
                {
                    IdUsuario = vmUsuario.IdUsuario,
                    Nombre = vmUsuario.Nombre,
                    Password = vmUsuario.Password,
                    Email = vmUsuario.Email,
                    FechaAlta = DateTime.Now,
                    Telefono = vmUsuario.Telefono,
                    // Conversión segura de byte[] a string (por ejemplo, Base64) o asigna null si es nulo
                    //RutaFoto = vmUsuario.RutaFoto != null ? Convert.ToBase64String(vmUsuario.RutaFoto) : null,
                    Activo = vmUsuario.Activo,
                    //TurnoDeTrabajo = vmUsuario.TurnoDeTrabajo,
                };

                _gestionUsuarios.AddUser(usuario);
                result = new JsonResult(new { StatusCode = 200, message = "Usuario agregado correctamente" });
            }
            catch (ValidacionException ex)
            {
                result = new JsonResult(new { StatusCode = 400, errors = ex.Message });
            }
            catch (Exception ex){

                jsonResponse = new JsonResponse("400", "La operacion no se pudo realizar", "Error" + ex.Message);
            
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

            //Obtenmos los datos del usuario para mostrarlos
            ViewFormAgregarUsuario vmModificarUsuario = new ViewFormAgregarUsuario();

            vmModificarUsuario.IdUsuario = usuario.IdUsuario;
            vmModificarUsuario.Nombre = usuario.Nombre;
            vmModificarUsuario.Password = usuario.Password;
            vmModificarUsuario.Email = usuario.Email;
            vmModificarUsuario.FechaAlta = usuario.FechaAlta;
            vmModificarUsuario.Telefono = usuario.Telefono;
            //vmModificarUsuario.RutaFoto = !string.IsNullOrEmpty(usuario.RutaFoto) ? Convert.FromBase64String(usuario.RutaFoto) : null;
            vmModificarUsuario.Activo = usuario.Activo;

            //Mostrar Modal
            string data = await _razorRenderService.ToStringAsync("FormUpdateUser", vmModificarUsuario);
            jsonResponse = new JsonResponse("200", "Operación realizada correctamente.", data);
            return new JsonResult(jsonResponse);
        }

        /// <summary>
        /// Método que contiene la funcionalidad de Modificar Periodos
        /// </summary>
        /// <param name="vmUsuario"></param>
        /// <returns></returns>
        [HttpPost, AjaxOnly]
        public async Task<ActionResult> ModificarUsuario(ViewFormAgregarUsuario vmUsuario)
        {
            JsonResult result = new JsonResult(new { StatusCode = 500, message = "No se pudo realizar la operación solicitada" });

            try
            {
                Usuario usuario = new Usuario()
                {
                    IdUsuario = vmUsuario.IdUsuario,
                    Nombre = vmUsuario.Nombre,
                    Password = vmUsuario.Password,
                    Email = vmUsuario.Email,
                    Telefono = vmUsuario.Telefono,
                    FechaAlta = DateTime.Now,
                    Activo = vmUsuario.Activo,
                    //RutaFoto = vmUsuario.RutaFoto
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
                Usuario usuario = _gestionUsuarios.Context.Usuarios.Where(u => u.IdUsuario == idUsuario).First();
                JsonResponse? jsonResponse = new JsonResponse("400", "Error en el servidor", "");
                ViewFormAgregarUsuario vmAgregarUsuario = new ViewFormAgregarUsuario();
                vmAgregarUsuario.Nombre =  usuario.Nombre;
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

        ///// <summary>
        ///// Metodo para eliminar un periodo de planificacion 
        ///// </summary>
        ///// <param name="id"> Identificador del usuario a eliminar </param>
        ///// <returns></returns>
        [HttpPost, AjaxOnly]
        public async Task<JsonResult> EliminarUsuarioAsync(int idUsuario)
        {
            JsonResponse response = new JsonResponse("200", "Ok");
            try
            {
                Usuario usuario = new Usuario();
                usuario = _gestionUsuarios.Context.Usuarios.Where(x => x.IdUsuario == idUsuario).First();
                _gestionUsuarios.DeleteUser(usuario.IdUsuario);
                response.Data = "Usuario eliminado correctamente";
            }
            catch (Exception ex)
            {
                response.Status = "500";
                response.StatusMessage = "Se ha producido un error al intentar eliminar el Usuario";
                response.ErrorData = ex.ToString();
                //response.Data =
                ex.Message.ToString();
            }
            return new JsonResult(response);
        }

        #endregion

        #region ImportarUsuarios

        [HttpGet, AjaxOnly]
        public async Task<JsonResult> ModalImportarUsuarios()
        {
            JsonResponse? jsonResponse = new JsonResponse("400", "Error en el servidor", "");
            FormImportarExcel vmUsuariosExcel = new FormImportarExcel();
            string data = await _razorRenderService.ToStringAsync("FormImportarUsuarios", vmUsuariosExcel);
            jsonResponse = new JsonResponse("200", "Operación realizada correctamente.", data);
            return new JsonResult(jsonResponse);
        }

        [HttpPost, AjaxOnly]
        public async Task<JsonResult> ImportarUsuarios(IFormFile fileExcel)
        {
            JsonResult jsonResult = new JsonResult(new { StatusCode = 500, message = "No se pudo realizar la operación solicitada" });

            List<Paciente> pacientes = new List<Paciente>();
            using (var memoryStream = new MemoryStream())
            {
                fileExcel.CopyTo(memoryStream);
                using (var package = new ExcelPackage(memoryStream))
                {
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null)
                    {
                        throw new Exception("Excepcion");
                    }
                    var hoja1 = package.Workbook.Worksheets[0];
                    if (!"Pacientes".Equals(hoja1.Name, StringComparison.CurrentCultureIgnoreCase))
                    {
                        throw new Exception("Excepcion");
                    }
                    //Calculo de rows y cols
                    int rowCount = worksheet.Dimension.Rows;
                    int colCount = worksheet.Dimension.Columns;
                    //Row 2 : Omite Cabecera
                    for (int row = 2; row < rowCount; row++)
                    {
                        var paciente = new Paciente();
                        paciente.NombrePaciente = hoja1.GetValue(row, 1).ToString();
                        paciente.DNI = hoja1.GetValue(row, 2).ToString();
                        paciente.Telefono = Convert.ToInt32(hoja1.GetValue(row, 3));
                        paciente.EMail = hoja1.GetValue(row, 4).ToString();
                        paciente.FechaNacimiento = Convert.ToDateTime(hoja1.GetValue(row, 5).ToString());
                        paciente.Direccion = hoja1.GetValue(row, 6).ToString();
                        paciente.Ciudad = hoja1.GetValue(row, 7).ToString();
                        paciente.FechaAlta = Convert.ToDateTime(hoja1.GetValue(row, 8).ToString());
                        paciente.NumeroConsultas = Convert.ToInt32(hoja1.GetValue(row, 9));
                        paciente.Asegurado = Convert.ToBoolean(hoja1.GetValue(row, 10));

                        if (!string.IsNullOrEmpty(paciente.NombrePaciente))
                        {
                            pacientes.Add(paciente);
                        }
                    }
                    string pacientesJSON = JsonSerializer.Serialize(pacientes);
                    pacientesJSON = await _razorRenderService.ToStringAsync("FormImportarUsuarios", pacientesJSON);
                    return jsonResult;
                }
            }
            ;
        }





        #endregion


        [HttpPost]
        public IActionResult AbrirWord()
        {
            string ruta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "documentos", "RoadMap.docx");

            if (!System.IO.File.Exists(ruta))
                return NotFound("Archivo no encontrado");

            try
            {
                Process.Start(new ProcessStartInfo(ruta) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                return BadRequest("No se pudo abrir el archivo: " + ex.Message);
            }

            return Ok(); // No devolvemos el archivo
        }


    }
}
