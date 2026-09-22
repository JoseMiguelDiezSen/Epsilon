using Calipso.Security;
using Epsilon.Attributes;
using Epsilon.Models;
using Epsilon.Models.Comun;
using Epsilon.Renders;
using Epsilon.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Negocio.Persistencia.Modelos;
using Negocio.Servicios;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Epsilon.Controllers
{
    public class MedicosController : AbstractSecurityController
    {
        private IRazorRenderService _renderService;
        private IGestionMedicos _gestionMedicos;

        /// <summary>
        /// Constructor del controlador de Médicos
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="gestionMedicos"></param>
        /// <param name="renderService"></param>
        public MedicosController(ILogger<MedicosController> logger, IGestionMedicos gestionMedicos, IRazorRenderService renderService) : base(logger)
        {
            _gestionMedicos = gestionMedicos;
            _renderService = renderService;
        }

        /// <summary>
        /// Metodo de acceso a la pagina
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            MedicosViewModel vmMedicos = new MedicosViewModel();

            IQueryable<DatosMedicos> datosMedicos = _gestionMedicos
                .GetDatosMedicos()
                .AsNoTracking()
                .OrderBy(x => x.IdMedico)
                .Skip((vmMedicos.PaginaActual - 1) * vmMedicos.RegistrosPorPagina)
                .Take(vmMedicos.RegistrosPorPagina);

            var medicos = datosMedicos
                .ToList() // aquí materializas
                .Select(x => new ViewMedicos(x)) // aquí usas tu constructor
                .ToList();

            vmMedicos.Medicos = medicos;

            return View("Index", vmMedicos);
        }

        /// <summary>
        /// Método utilizado para el filtrado de médicos en función de los criterios seleccionados
        /// </summary>
        /// <param name="vmMedicos"></param>
        /// <returns></returns>
        [HttpPost, AjaxOnly]
        public async Task<JsonResult> FiltrarMedicosAsync(MedicosViewModel vmMedicos)
        {
            JsonResponse? jsonResponse = null;

            try
            {
                IQueryable<DatosMedicos> datosMedicos = _gestionMedicos.GetDatosMedicos();

                if (!string.IsNullOrWhiteSpace(vmMedicos.NombreMedico))
                {
                    datosMedicos = datosMedicos.Where(p => p.NombreMedico != null && p.NombreMedico.Contains(vmMedicos.NombreMedico));
                }
                if (!string.IsNullOrWhiteSpace(vmMedicos.DNI))
                {
                    datosMedicos = datosMedicos.Where(p => p.DNI == vmMedicos.DNI);
                }
                if (!string.IsNullOrWhiteSpace(vmMedicos.Especialidad))
                {
                    datosMedicos = datosMedicos.Where(p => p.Especialidad != null && p.Especialidad.Contains(vmMedicos.Especialidad));
                }
                if (!string.IsNullOrWhiteSpace(vmMedicos.EMail))
                {
                    datosMedicos = datosMedicos.Where(p => p.EMail == vmMedicos.EMail);
                }
                if (!string.IsNullOrWhiteSpace(vmMedicos.Telefono))
                {
                    datosMedicos = datosMedicos.Where(p => p.Telefono == vmMedicos.Telefono);
                }
                if (vmMedicos.NumeroColegiado > 0)
                {
                    datosMedicos = datosMedicos.Where(p => p.NumeroColegiado == vmMedicos.NumeroColegiado);
                }

                vmMedicos.Medicos = await datosMedicos
                    .OrderBy(x => x.IdMedico)
                    .Skip((vmMedicos.PaginaActual - 1) * vmMedicos.RegistrosPorPagina)
                    .Take(vmMedicos.RegistrosPorPagina)
                    .Select(x => new ViewMedicos(x))
                    .ToListAsync();

                string data = await _renderService.ToStringAsync("TablaMedicos", vmMedicos.Medicos);
                jsonResponse = new JsonResponse("200", "Operación realizada correctamente.", data);
            }
            catch (Exception ex)
            {
                jsonResponse = new JsonResponse("500", "La operación no se pudo realizar.", string.Empty, "Error: " + ex.Message);
            }

            return new JsonResult(jsonResponse);
        }

        #region AgregarMedico

        /// <summary>
        /// Método para abrir la ventana modal de agregar médicos
        /// </summary>
        /// <returns></returns>
        [HttpGet, AjaxOnly]
        public async Task<ActionResult> ModalAgregarMedico()
        {
            JsonResponse? jsonResponse = new JsonResponse("400", "Error en el servidor", "");
            ViewFormAgregarMedico vmAgregarMedico = new ViewFormAgregarMedico();
            vmAgregarMedico.FechaContratacion = DateTime.Now.ToString("yyyy-MM-dd");
            vmAgregarMedico.Activo = true;
            vmAgregarMedico.Clinicas = new SelectList(_gestionMedicos.Context.Clinicas.ToList(), nameof(Clinica.IdClinica), nameof(Clinica.NombreClinica));

            string data = await _renderService.ToStringAsync("FormAddMedico", vmAgregarMedico);
            jsonResponse = new JsonResponse("200", "Operación realizada correctamente.", data);
            return new JsonResult(jsonResponse);
        }

        /// <summary>
        /// Método que contiene la funcionalidad de Añadir Médicos
        /// </summary>
        /// <param name="vmMedico"></param>
        /// <returns></returns>
        [HttpPost, AjaxOnly]
        public async Task<JsonResult> AgregarMedico([FromForm] ViewFormAgregarMedico vmMedico)
        {
            JsonResult result = new JsonResult(new { StatusCode = 500, message = "No se pudo realizar la operación solicitada" });

            try
            {
                byte[]? foto = null;

                if (vmMedico.Foto != null && vmMedico.Foto.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        await vmMedico.Foto.CopyToAsync(ms);
                        foto = ms.ToArray();
                    }
                }

                Medico medico = new Medico()
                {
                    NombreMedico = vmMedico.NombreMedico,
                    DNI = vmMedico.DNI,
                    NumeroColegiado = vmMedico.NumeroColegiado,
                    Especialidad = vmMedico.Especialidad,
                    Telefono = vmMedico.Telefono,
                    EMail = vmMedico.EMail,
                    FechaContratacion = vmMedico.FechaContratacion,
                    Activo = vmMedico.Activo,
                    Observaciones = vmMedico.Observaciones,
                    IdClinica = vmMedico.IdClinica,
                    Foto = foto
                };

                _gestionMedicos.AddMedico(medico);

                result = new JsonResult(new
                {
                    StatusCode = 200,
                    message = "Médico agregado correctamente"
                });
            }
            catch (Exception ex)
            {
                result = new JsonResult(new { StatusCode = 500, message = ex.Message });
            }

            return result;
        }

        #endregion

        #region ModificarMedico

        /// <summary>
        /// Método para abrir la ventana modal de modificar médico
        /// </summary>
        /// <param name="idMedico">Identificador del médico a modificar</param>
        /// <returns></returns>
        [HttpGet, AjaxOnly]
        public async Task<ActionResult> GetModalModificarMedico(int idMedico)
        {
            Medico? medico = _gestionMedicos.Context.Medicos.FirstOrDefault(x => x.IdMedico == idMedico);

            if (medico == null)
            {
                return new JsonResult(new JsonResponse("404", "Médico no encontrado", ""));
            }

            ViewFormAgregarMedico vmModificarMedico = new ViewFormAgregarMedico
            {
                IdMedico = medico.IdMedico,
                NombreMedico = medico.NombreMedico,
                DNI = medico.DNI,
                NumeroColegiado = medico.NumeroColegiado,
                Especialidad = medico.Especialidad,
                Telefono = medico.Telefono,
                EMail = medico.EMail,
                FechaContratacion = medico.FechaContratacion,
                Activo = medico.Activo,
                Observaciones = medico.Observaciones,
                IdClinica = medico.IdClinica,
                Clinicas = new SelectList(_gestionMedicos.Context.Clinicas.ToList(), nameof(Clinica.IdClinica), nameof(Clinica.NombreClinica), medico.IdClinica)
            };

            if (medico.Foto != null && medico.Foto.Length > 0)
            {
                vmModificarMedico.FotoBase64 = $"data:image/jpeg;base64,{Convert.ToBase64String(medico.Foto)}";
            }

            string data = await _renderService.ToStringAsync("FormModificarMedico", vmModificarMedico);
            JsonResponse jsonResponse = new JsonResponse("200", "Operación realizada correctamente.", data);

            return new JsonResult(jsonResponse);
        }

        /// <summary>
        /// Método para modificar los datos de un médico
        /// </summary>
        /// <param name="vmMedico"></param>
        /// <returns></returns>
        [HttpPost, AjaxOnly]
        public async Task<ActionResult> ModificarMedico([FromForm] ViewFormAgregarMedico vmMedico)
        {
            JsonResult result = new JsonResult(new { StatusCode = 500, message = "No se pudo realizar la operación solicitada" });

            try
            {
                Medico medico = new Medico()
                {
                    IdMedico = vmMedico.IdMedico,
                    NombreMedico = vmMedico.NombreMedico,
                    DNI = vmMedico.DNI,
                    NumeroColegiado = vmMedico.NumeroColegiado,
                    Especialidad = vmMedico.Especialidad,
                    Telefono = vmMedico.Telefono,
                    EMail = vmMedico.EMail,
                    FechaContratacion = vmMedico.FechaContratacion,
                    Activo = vmMedico.Activo,
                    Observaciones = vmMedico.Observaciones,
                    IdClinica = vmMedico.IdClinica
                };

                if (vmMedico.Foto != null && vmMedico.Foto.Length > 0)
                {
                    using var ms = new MemoryStream();
                    await vmMedico.Foto.CopyToAsync(ms);
                    medico.Foto = ms.ToArray();
                }

                _gestionMedicos.UpdateMedico(medico);

                result = new JsonResult(new { StatusCode = 200, message = "Médico actualizado correctamente" });
            }
            catch (Exception ex)
            {
                result = new JsonResult(new { StatusCode = 500, message = ex.Message });
            }

            return result;
        }

        #endregion

        #region EliminarMedico

        /// <summary>
        /// Metodo para eliminar un medico 
        /// </summary>
        /// <param name="idMedico"> Identificador del médico a eliminar </param>
        /// <returns></returns>
        [HttpGet, AjaxOnly]
        public async Task<JsonResult> EliminarMedicoAsync(long idMedico)
        {
            JsonResponse response = new JsonResponse("200", "Ok");
            try
            {
                Medico medico = _gestionMedicos.Context.Medicos.Where(u => u.IdMedico == idMedico).First();
                JsonResponse? jsonResponse = new JsonResponse("400", "Error en el servidor", "");
                ViewFormAgregarMedico vmAgregarMedico = new ViewFormAgregarMedico();
                vmAgregarMedico.IdMedico = medico.IdMedico;
                vmAgregarMedico.NombreMedico = medico.NombreMedico;
                string data = await _renderService.ToStringAsync("FormDeleteMedico", vmAgregarMedico);
                jsonResponse = new JsonResponse("200", "Operación realizada correctamente.", data);
                return new JsonResult(jsonResponse);
            }
            catch (Exception ex)
            {
                response.Status = "500";
                response.StatusMessage = "Se ha producido un error al intentar eliminar el Médico";
            }
            return new JsonResult(response);
        }

        /// <summary>
        /// Metodo para eliminar un médico
        /// </summary>
        /// <param name="idMedico"> Identificador del médico a eliminar </param>
        /// <returns></returns>
        [HttpPost, AjaxOnly]
        public async Task<JsonResult> EliminarMedicoAsync(int idMedico)
        {
            JsonResponse response = new JsonResponse("200", "Ok");
            try
            {
                Medico medico = new Medico();
                medico = _gestionMedicos.Context.Medicos.Where(x => x.IdMedico == idMedico).First();
                _gestionMedicos.DeleteMedico(medico.IdMedico);
                response.Data = "Médico eliminado correctamente";
            }
            catch (Exception ex)
            {
                response.Status = "500";
                response.StatusMessage = "Se ha producido un error al intentar eliminar el Médico";
                response.ErrorData = ex.ToString();
                ex.Message.ToString();
            }
            return new JsonResult(response);
        }

        #endregion

        /// <summary>
        /// Devuelve una vista parcial que muestra los detalles del médico identificado.
        /// </summary>
        /// <param name="idMedico">Identificador único del médico</param>
        /// <returns></returns>
        [HttpGet, AjaxOnly]
        public IActionResult DetalleMedico(int idMedico)
        {
            var medico = _gestionMedicos.GetDetalleMedico(idMedico);
            return PartialView("DetalleMedico", medico);
        }
    }
}
