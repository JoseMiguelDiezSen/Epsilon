using Calipso.Security;
using Epsilon.Attributes;
using Epsilon.Models;
using Epsilon.Models.Comun;
using Epsilon.Renders;
using Epsilon.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Negocio.Persistencia.Modelos;
using Negocio.Servicios;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Epsilon.Controllers
{
    public class PersonalController : AbstractSecurityController
    {
        private readonly IRazorRenderService _renderService;
        private readonly IGestionPersonal _gestionPersonal;

        public PersonalController(
            ILogger<PersonalController> logger,
            IGestionPersonal gestionPersonal,
            IRazorRenderService renderService) : base(logger)
        {
            _gestionPersonal = gestionPersonal;
            _renderService = renderService;
        }

        public IActionResult Index()
        {
            PersonalViewModel vmPersonal = new PersonalViewModel();

            IQueryable<DatosPersonal> datosPersonal = _gestionPersonal
                .GetDatosPersonal()
                .AsNoTracking()
                .OrderBy(x => x.IdEmpleado)
                .Skip((vmPersonal.PaginaActual - 1) * vmPersonal.RegistrosPorPagina)
                .Take(vmPersonal.RegistrosPorPagina);

            var personal = datosPersonal
                .ToList()
                .Select(x => new ViewPersonal(x))
                .ToList();

            vmPersonal.Personal = personal;

            return View("Index", vmPersonal);
        }

        [HttpPost, AjaxOnly]
        public async Task<JsonResult> FiltrarPersonalAsync(PersonalViewModel vmPersonal)
        {
            JsonResponse? jsonResponse;

            try
            {
                IQueryable<DatosPersonal> datosPersonal = _gestionPersonal.GetDatosPersonal();

                if (!string.IsNullOrWhiteSpace(vmPersonal.NombreEmpleado))
                {
                    datosPersonal = datosPersonal.Where(p => p.NombreEmpleado != null && p.NombreEmpleado.Contains(vmPersonal.NombreEmpleado));
                }
                if (!string.IsNullOrWhiteSpace(vmPersonal.DNI))
                {
                    datosPersonal = datosPersonal.Where(p => p.DNI == vmPersonal.DNI);
                }
                if (!string.IsNullOrWhiteSpace(vmPersonal.Puesto))
                {
                    datosPersonal = datosPersonal.Where(p => p.Puesto != null && p.Puesto.Contains(vmPersonal.Puesto));
                }
                if (!string.IsNullOrWhiteSpace(vmPersonal.EMail))
                {
                    datosPersonal = datosPersonal.Where(p => p.EMail == vmPersonal.EMail);
                }
                if (!string.IsNullOrWhiteSpace(vmPersonal.Telefono))
                {
                    datosPersonal = datosPersonal.Where(p => p.Telefono == vmPersonal.Telefono);
                }
                if (vmPersonal.NumeroEmpleado > 0)
                {
                    datosPersonal = datosPersonal.Where(p => p.NumeroEmpleado == vmPersonal.NumeroEmpleado);
                }

                vmPersonal.Personal = await datosPersonal
                    .OrderBy(x => x.IdEmpleado)
                    .Skip((vmPersonal.PaginaActual - 1) * vmPersonal.RegistrosPorPagina)
                    .Take(vmPersonal.RegistrosPorPagina)
                    .Select(x => new ViewPersonal(x))
                    .ToListAsync();

                string data = await _renderService.ToStringAsync("TablaPersonal", vmPersonal.Personal);
                jsonResponse = new JsonResponse("200", "Operación realizada correctamente.", data);
            }
            catch (Exception ex)
            {
                jsonResponse = new JsonResponse("500", "La operación no se pudo realizar.", string.Empty, "Error: " + ex.Message);
            }

            return new JsonResult(jsonResponse);
        }

        [HttpGet, AjaxOnly]
        public async Task<ActionResult> ModalAgregarPersonal()
        {
            ViewFormAgregarPersonal vm = new ViewFormAgregarPersonal();
            string data = await _renderService.ToStringAsync("FormAddPersonal", vm);
            return new JsonResult(new JsonResponse("200", "Operación realizada correctamente.", data));
        }

        [HttpPost, AjaxOnly]
        public async Task<JsonResult> AgregarPersonal([FromForm] ViewFormAgregarPersonal vmPersonal)
        {
            try
            {
                byte[]? fotoBytes = null;
                if (vmPersonal.FotoSubida != null && vmPersonal.FotoSubida.Length > 0)
                {
                    using var memoryStream = new MemoryStream();
                    await vmPersonal.FotoSubida.CopyToAsync(memoryStream);
                    fotoBytes = memoryStream.ToArray();
                }

                Personal personal = new Personal
                {
                    NombreEmpleado = vmPersonal.NombreEmpleado,
                    DNI = vmPersonal.DNI,
                    NumeroEmpleado = vmPersonal.NumeroEmpleado,
                    Puesto = vmPersonal.Puesto,
                    Telefono = vmPersonal.Telefono,
                    EMail = vmPersonal.EMail,
                    FechaContratacion = vmPersonal.FechaContratacion ?? DateTime.Now.ToString("yyyy-MM-dd"),
                    Activo = vmPersonal.Activo,
                    Observaciones = vmPersonal.Observaciones,
                    Foto = fotoBytes,
                    IdSede = vmPersonal.IdSede,
                    IdUsuario = vmPersonal.IdUsuario
                };

                _gestionPersonal.AddPersonal(personal);
                return new JsonResult(new { StatusCode = 200, message = "Empleado agregado correctamente" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { StatusCode = 500, message = "Error: " + ex.Message });
            }
        }

        [HttpGet, AjaxOnly]
        public async Task<ActionResult> GetModalModificarPersonal(int idEmpleado)
        {
            var p = _gestionPersonal.GetPersonal(idEmpleado);
            if (p == null) return NotFound();

            ViewFormAgregarPersonal vm = new ViewFormAgregarPersonal
            {
                IdEmpleado = p.IdEmpleado,
                NombreEmpleado = p.NombreEmpleado,
                DNI = p.DNI,
                NumeroEmpleado = p.NumeroEmpleado,
                Puesto = p.Puesto,
                Telefono = p.Telefono,
                EMail = p.EMail,
                FechaContratacion = p.FechaContratacion,
                Activo = p.Activo,
                Observaciones = p.Observaciones,
                IdSede = p.IdSede,
                IdUsuario = p.IdUsuario
            };

            string data = await _renderService.ToStringAsync("FormModificarPersonal", vm);
            return new JsonResult(new JsonResponse("200", "Operación realizada correctamente.", data));
        }

        [HttpPost, AjaxOnly]
        public async Task<ActionResult> ModificarPersonal([FromForm] ViewFormAgregarPersonal vmPersonal)
        {
            try
            {
                byte[]? fotoBytes = null;
                if (vmPersonal.FotoSubida != null && vmPersonal.FotoSubida.Length > 0)
                {
                    using var memoryStream = new MemoryStream();
                    await vmPersonal.FotoSubida.CopyToAsync(memoryStream);
                    fotoBytes = memoryStream.ToArray();
                }

                Personal personal = new Personal
                {
                    IdEmpleado = vmPersonal.IdEmpleado,
                    NombreEmpleado = vmPersonal.NombreEmpleado,
                    DNI = vmPersonal.DNI,
                    NumeroEmpleado = vmPersonal.NumeroEmpleado,
                    Puesto = vmPersonal.Puesto,
                    Telefono = vmPersonal.Telefono,
                    EMail = vmPersonal.EMail,
                    FechaContratacion = vmPersonal.FechaContratacion ?? DateTime.Now.ToString("yyyy-MM-dd"),
                    Activo = vmPersonal.Activo,
                    Observaciones = vmPersonal.Observaciones,
                    Foto = fotoBytes,
                    IdSede = vmPersonal.IdSede,
                    IdUsuario = vmPersonal.IdUsuario
                };

                _gestionPersonal.UpdatePersonal(personal);
                return new JsonResult(new { StatusCode = 200, message = "Empleado modificado correctamente" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { StatusCode = 500, message = "Error: " + ex.Message });
            }
        }

        [HttpGet, AjaxOnly]
        public async Task<JsonResult> EliminarPersonalAsync(long idEmpleado)
        {
            var p = _gestionPersonal.GetPersonal((int)idEmpleado);
            if (p == null) return new JsonResult(new JsonResponse("404", "No encontrado"));

            ViewFormAgregarPersonal vm = new ViewFormAgregarPersonal
            {
                IdEmpleado = p.IdEmpleado,
                NombreEmpleado = p.NombreEmpleado
            };

            string data = await _renderService.ToStringAsync("FormDeletePersonal", vm);
            return new JsonResult(new JsonResponse("200", "Operación realizada correctamente.", data));
        }

        [HttpPost, AjaxOnly]
        public JsonResult ConfirmarEliminarPersonal(int idEmpleado)
        {
            try
            {
                _gestionPersonal.DeletePersonal(idEmpleado);
                return new JsonResult(new { StatusCode = 200, message = "Empleado eliminado con éxito." });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { StatusCode = 500, message = "Error: " + ex.Message });
            }
        }

        public IActionResult DetallePersonal(int idEmpleado)
        {
            var detalle = _gestionPersonal.GetDatosPersonal().FirstOrDefault(p => p.IdEmpleado == idEmpleado);
            if (detalle == null) return NotFound();
            return PartialView("DetallePersonal", detalle);
        }
    }
}
