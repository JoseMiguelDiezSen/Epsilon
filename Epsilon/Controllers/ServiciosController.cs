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
using System.Linq;
using System.Threading.Tasks;

namespace Epsilon.Controllers
{
    public class ServiciosController : AbstractSecurityController
    {
        private readonly IRazorRenderService _razorRenderService;
        private readonly IGestionServicios _gestionServicios;

        public ServiciosController(
            ILogger<ServiciosController> logger,
            IGestionServicios gestionServicios,
            IRazorRenderService renderService) : base(logger)
        {
            _gestionServicios = gestionServicios;
            _razorRenderService = renderService;
        }

        public IActionResult Index()
        {
            ServiciosViewModel vm = new ServiciosViewModel();

            IQueryable<DatosServicios> datos = _gestionServicios
                .GetDatosServicios()
                .AsNoTracking()
                .OrderBy(x => x.IdServicio)
                .Skip((vm.PaginaActual - 1) * vm.RegistrosPorPagina)
                .Take(vm.RegistrosPorPagina);

            vm.Servicios = datos
                .ToList()
                .Select(x => new ViewServicio(x))
                .ToList();

            return View("Index", vm);
        }

        [HttpGet, AjaxOnly]
        public async Task<ActionResult> ModalAgregarServicio()
        {
            ViewFormAgregarServicio vm = new ViewFormAgregarServicio();
            string data = await _razorRenderService.ToStringAsync("FormAddServicio", vm);
            return new JsonResult(new JsonResponse("200", "OperaciÃ³n realizada correctamente.", data));
        }

        [HttpPost, AjaxOnly]
        public JsonResult AgregarServicio(ViewFormAgregarServicio vmServicio)
        {
            try
            {
                Servicio s = new Servicio
                {
                    NombreServicio = vmServicio.NombreServicio,
                    Duracion = vmServicio.Duracion,
                    Color = vmServicio.Color,
                    Precio = vmServicio.Precio
                };

                _gestionServicios.AddServicio(s);
                return new JsonResult(new { StatusCode = 200, message = "Servicio creado con Ã©xito." });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { StatusCode = 500, message = "Error: " + ex.Message });
            }
        }
        [HttpGet, AjaxOnly]
        public async Task<ActionResult> ModalModificarServicio(int idServicio)
        {
            var servicio = _gestionServicios.Context.Servicios.FirstOrDefault(s => s.IdServicio == idServicio);
            if (servicio == null) return new JsonResult(new JsonResponse("404", "No encontrado", ""));

            ViewFormAgregarServicio vm = new ViewFormAgregarServicio
            {
                IdServicio = servicio.IdServicio,
                NombreServicio = servicio.NombreServicio,
                Duracion = servicio.Duracion,
                Color = servicio.Color,
                Precio = servicio.Precio
            };
            string data = await _razorRenderService.ToStringAsync("FormUpdateServicio", vm);
            return new JsonResult(new JsonResponse("200", "Ok", data));
        }

        [HttpPost, AjaxOnly]
        public JsonResult ModificarServicio(ViewFormAgregarServicio vm)
        {
            try
            {
                var s = _gestionServicios.Context.Servicios.FirstOrDefault(x => x.IdServicio == vm.IdServicio);
                if (s != null)
                {
                    s.NombreServicio = vm.NombreServicio;
                    s.Duracion = vm.Duracion;
                    s.Color = vm.Color;
                    s.Precio = vm.Precio;
                    _gestionServicios.Context.SaveChanges();
                }
                return new JsonResult(new { StatusCode = 200, message = "Modificado" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { StatusCode = 500, message = "Error" });
            }
        }

        [HttpGet, AjaxOnly]
        public async Task<JsonResult> ModalEliminarServicio(int idServicio)
        {
            var servicio = _gestionServicios.Context.Servicios.FirstOrDefault(s => s.IdServicio == idServicio);
            ViewFormAgregarServicio vm = new ViewFormAgregarServicio { IdServicio = idServicio, NombreServicio = servicio?.NombreServicio };
            string data = await _razorRenderService.ToStringAsync("FormDeleteServicio", vm);
            return new JsonResult(new JsonResponse("200", "Ok", data));
        }

        [HttpPost, AjaxOnly]
        public JsonResult EliminarServicio(int idServicio)
        {
            try
            {
                var s = _gestionServicios.Context.Servicios.FirstOrDefault(x => x.IdServicio == idServicio);
                if (s != null)
                {
                    _gestionServicios.Context.Servicios.Remove(s);
                    _gestionServicios.Context.SaveChanges();
                }
                return new JsonResult(new { StatusCode = 200, message = "Eliminado" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { StatusCode = 500, message = "Error" });
            }
        }
    }
}
