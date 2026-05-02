using Calipso.Security;
using Epsilon.Attributes;
using Epsilon.Models;
using Epsilon.Models.Comun;
using Epsilon.Renders;
using Epsilon.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Negocio.Persistencia;
using Negocio.Persistencia.Modelos;
using Negocio.Servicios;
using OfficeOpenXml;
using System.Drawing;
using System.Linq;
using System.Text.Json;

namespace Epsilon.Controllers
{
    public class PacientesController : AbstractSecurityController
    {
        private readonly IRazorRenderService _renderService;
        private IGestionPacientes _gestionPacientes;
        private readonly EpsilonDbContext _context;
        /// <summary>
        /// Constructor del controlador 'Pacientes'
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="seguridad"></param>
        /// <param name="gestionPacientes"></param>
        public PacientesController(ILogger<PacientesController> logger, IGestionPacientes gestionPacientes, IRazorRenderService renderService, EpsilonDbContext context) : base(logger)
        {
            _gestionPacientes = gestionPacientes;
            _renderService = renderService;
            _context = context;
        }

        public IActionResult Index()
        {
            PacientesViewModel vmPacientes = new PacientesViewModel();

            IQueryable<DatosPacientes> datosPacientes = _gestionPacientes
                .GetDatosPacientes()
                .AsNoTracking()
                .OrderBy(x => x.IdPaciente)
                .Skip((vmPacientes.PaginaActual - 1) * vmPacientes.RegistrosPorPagina)
                .Take(vmPacientes.RegistrosPorPagina);

            var pacientes = datosPacientes
                .ToList() // aquí materializas
                .Select(x => new ViewPacientes(x)) // aquí usas tu constructor
                .ToList();

            vmPacientes.Pacientes = pacientes;

            return View("Index", vmPacientes);
        }


        [HttpPost, AjaxOnly]
        public async Task<JsonResult> FiltrarUsuariosAsync(PacientesViewModel vmPacientes)
        {
            JsonResponse? jsonResponse = null;

            try
            {
                IQueryable<DatosPacientes> datosPacientes = _gestionPacientes.GetDatosPacientes();

                if (!string.IsNullOrWhiteSpace(vmPacientes.NombrePaciente))
                {
                    datosPacientes = datosPacientes.Where(p => p.NombrePaciente == vmPacientes.NombrePaciente);
                }
                if (!string.IsNullOrWhiteSpace(vmPacientes.EMail))
                {
                    datosPacientes = datosPacientes.Where(p => p.EMail == vmPacientes.EMail);
                }
                if (vmPacientes.Telefono > 0)
                {
                    datosPacientes = datosPacientes.Where(p => p.Telefono == vmPacientes.Telefono);
                }


                vmPacientes.Pacientes = await datosPacientes.
                    OrderBy(x => x.IdPaciente)
                    .Skip((vmPacientes.PaginaActual - 1) * vmPacientes.RegistrosPorPagina)
                    .Take(vmPacientes.RegistrosPorPagina).
                    Select(x => new ViewPacientes(x)).
                    ToListAsync();

                string data = await _renderService.ToStringAsync("TablaPacientes", vmPacientes.Pacientes);
                jsonResponse = new JsonResponse("200", "Operación realizada correctamente.", data);
            }

            catch (Exception ex)
            {
                jsonResponse = new JsonResponse("500", "La operación no se pudo realizar.", string.Empty, "Error: " + ex.Message);
            }

            return new JsonResult(jsonResponse);
        }




        /// <summary>
        /// Método para abrir la ventana modal de agregar pacientes
        /// </summary>
        /// <returns></returns>

        [HttpGet, AjaxOnly]
        public async Task<ActionResult> ModalAgregarPaciente()
        {
            JsonResponse? jsonResponse = new JsonResponse("400", "Error en el servidor", "");
            ViewFormAgregarPaciente vmAgregarPaciente = new ViewFormAgregarPaciente();
            vmAgregarPaciente.FechaAlta1 = DateTime.Now;
            string data = await _renderService.ToStringAsync("FormAddPaciente", vmAgregarPaciente);
            jsonResponse = new JsonResponse("200", "Operación realizada correctamente.", data);
            return new JsonResult(jsonResponse);
        }

        /// <summary>
        /// Método que contiene la funcionalidad de Añadir Pacientes
        /// </summary>
        /// <param name="vmPaciente"></param>
        /// <returns></returns>
        [HttpPost, AjaxOnly]
        public async Task<JsonResult> AgregarPacienteAsync(ViewFormAgregarPaciente vmPaciente)
        {
            JsonResult result = new JsonResult(new { StatusCode = 500, message = "No se pudo realizar la operación solicitada" });

            try
            {
                Paciente paciente = new Paciente()
                {
                    NombrePaciente = vmPaciente.NombrePaciente,
                    DNI = vmPaciente.DNI,
                    Telefono = vmPaciente.Telefono,
                    EMail = vmPaciente.EMail,
                    FechaNacimiento = vmPaciente.FechaNacimiento,
                    Direccion = vmPaciente.Direccion,
                    Ciudad = vmPaciente.Ciudad,
                    FechaAlta = vmPaciente.FechaAlta1,
                    NumeroConsultas = vmPaciente.NumeroConsultas,
                    Asegurado = vmPaciente.Asegurado
                };

                _gestionPacientes.AddPaciente(paciente);
                result = new JsonResult(new { StatusCode = 200, message = "Paciente agregado correctamente" });
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return result;
        }

        #region ModificarUsuario

        [HttpGet, AjaxOnly]
        public async Task<ActionResult> ModalModificarPacienteAsync(int idPaciente)
        {
            JsonResponse? jsonResponse = new JsonResponse("400", "Error en el servidor", "");

            Paciente paciente = new Paciente();
            //Obtenemos el usuario
            paciente = _gestionPacientes.Context.Pacientes.Where(x => x.IdPaciente == idPaciente).First();

            //Obtenmos los datos del usuario para mostrarlos
            ViewFormAgregarPaciente vmPaciente = new ViewFormAgregarPaciente();

            vmPaciente.IdPaciente = idPaciente;
            vmPaciente.NombrePaciente = paciente.NombrePaciente;
            vmPaciente.DNI = paciente.DNI;
            vmPaciente.Telefono = paciente.Telefono;
            vmPaciente.EMail = paciente.EMail;
            vmPaciente.Direccion = paciente.Direccion;
            vmPaciente.FechaNacimiento = paciente.FechaNacimiento;
            vmPaciente.Ciudad = paciente.Ciudad;
            vmPaciente.FechaAlta1 = paciente.FechaAlta;
            vmPaciente.NumeroConsultas = paciente.NumeroConsultas;
            vmPaciente.Asegurado = paciente.Asegurado;

            //Mostrar Modal
            string data = await _renderService.ToStringAsync("FormModificarPaciente", vmPaciente);
            jsonResponse = new JsonResponse("200", "Operación realizada correctamente.", data);
            return new JsonResult(jsonResponse);
        }

        /// <summary>
        /// Método que contiene la funcionalidad de Modificar pacientes
        /// </summary>
        /// <param name="vmPaciente"></param>
        /// <returns></returns>
        [HttpPost, AjaxOnly]
        public async Task<JsonResult> ModificarPacienteAsync(ViewFormAgregarPaciente vmPaciente)
        {
            JsonResult result = new JsonResult(new { StatusCode = 500, message = "No se pudo realizar la operación solicitada" });

            try
            {
                Paciente paciente = new Paciente()
                {
                    IdPaciente = vmPaciente.IdPaciente,
                    NombrePaciente = vmPaciente.NombrePaciente,
                    DNI = vmPaciente.DNI,
                    Telefono = vmPaciente.Telefono,
                    EMail = vmPaciente.EMail,
                    Direccion = vmPaciente.Direccion,
                    FechaNacimiento = vmPaciente.FechaNacimiento,
                    Ciudad = vmPaciente.Ciudad,
                    FechaAlta = vmPaciente.FechaAlta1,
                    NumeroConsultas = vmPaciente.NumeroConsultas,
                    Asegurado = vmPaciente.Asegurado
                };

                var res = _gestionPacientes.UpdatePaciente(paciente);
                result = new JsonResult(new { StatusCode = 200, message = "Usuario actualizado correctamente" });
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return result;
        }

        #endregion

        #region EliminarPaciente

        /// <summary>
        /// Metodo para eliminar un paciente
        /// </summary>
        /// <param name="idPaciente"> Identificador del paciente a eliminar </param>
        /// <returns></returns>
        [HttpGet, AjaxOnly]
        public async Task<JsonResult> EliminarPacienteAsync(long idPaciente)
        {
            JsonResponse response = new JsonResponse("200", "Ok");
            try
            {
                Paciente paciente = _gestionPacientes.Context.Pacientes.Where(u => u.IdPaciente == idPaciente).First();
                JsonResponse? jsonResponse = new JsonResponse("400", "Error en el servidor", "");
                ViewFormAgregarPaciente vmAgregarPaciente = new ViewFormAgregarPaciente();
                vmAgregarPaciente.NombrePaciente = paciente.NombrePaciente;
                string data = await _renderService.ToStringAsync("FormDeletePaciente", vmAgregarPaciente);
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

        #region ImportarPacientes

        [HttpGet, AjaxOnly]
        public async Task<JsonResult> ModalImportarExcel()
        {
            JsonResponse? jsonResponse = new JsonResponse("400", "Error en el servidor", "");
            FormImportarExcel vmUsuariosExcel = new FormImportarExcel();           
            string data = await _renderService.ToStringAsync("FormImportarPacientes", vmUsuariosExcel);
            jsonResponse = new JsonResponse("200", "Operación realizada correctamente.", data);
            return new JsonResult(jsonResponse);
        }

        /// <summary>
        /// Devuelve una vista parcial que muestra los detalles del paciente que sea identificado.
        /// </summary>
        /// <param name="idPaciente"> Identificador unico del paciente del cual se mortaran los detalles.</param>
        /// <returns>A partial view containing the patient's details if found; otherwise, a view with no data.</returns>
        [HttpGet, AjaxOnly]
        public IActionResult DetallePaciente(int idPaciente)
        {
            var paciente = _gestionPacientes.GetDetallePaciente(idPaciente);
            return PartialView("DetallePaciente", paciente);
        }


        public IActionResult HistorialPaciente(int idPaciente)
        {
            var paciente = _context.DatosPacientes.FirstOrDefault(p => p.IdPaciente == idPaciente);
            return View("HistorialPaciente", paciente);
        }


        //public IActionResult DetallePaciente(int idPaciente)
        //{
        //    var paciente = /* aquí cargas lo que necesites */;
        //    var model = /* construyes tu ViewModel bonito */;

        //    return PartialView("DetallePaciente", model);
        //}

        [HttpPost, AjaxOnly]
        public async Task<JsonResult> ImportarExcel(IFormFile fileExcel)
        {
            JsonResult jsonResult = new JsonResult(new { StatusCode = 500, message = "No se pudo realizar la operación solicitada" });

            List <Paciente> pacientes = new List <Paciente>();
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
                    pacientesJSON = await _renderService.ToStringAsync("FormImportarPacientes", pacientesJSON);
                    return jsonResult;
                }
            };
        }
        #endregion
    }
}
