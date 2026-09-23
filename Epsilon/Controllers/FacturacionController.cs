using Calipso.Security;
using Epsilon.Attributes;
using Epsilon.Models;
using Epsilon.Models.Comun;
using Epsilon.Renders;
using Epsilon.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Negocio.Servicios;

namespace Epsilon.Controllers
{
    public class FacturacionController : AbstractSecurityController
    {
        private readonly IRazorRenderService _renderService;
        private IGestionUsuarios _gestionUsuarios;
        private IGestionFacturacion _gestionFacturacion;

        public FacturacionController(ILogger<FacturacionController> logger, IGestionUsuarios gestionUsuarios, IRazorRenderService renderService, IGestionFacturacion gestionFacturacion) : base(logger)
        {
            _gestionUsuarios = gestionUsuarios;
            _renderService = renderService;
            _gestionFacturacion = gestionFacturacion;
        }

        public IActionResult Index()
        {
            return View(CargarViewModel());
        }

        [HttpGet, AjaxOnly]
        public IActionResult ObtenerDatosFacturacion()
        {
            _gestionFacturacion.RecalcularFacturacionGlobal();
            var facturacion = _gestionFacturacion.GetFacturacion().ToList(); 

            return Json(new
            {
                datosChart = ObtenerDatosChartMensual(facturacion), 
                totalFacturacion = facturacion.Sum(f => f.Importe), 
                totalCitas = facturacion.Count 
            });
        }

        private FacturacionViewModel CargarViewModel()
        {
            _gestionFacturacion.RecalcularFacturacionGlobal();
            var facturacion = _gestionFacturacion.GetFacturacion().ToList();

            return new FacturacionViewModel
            {
                DatosChart = ObtenerDatosChartMensual(facturacion), 
                TotalFacturacion = facturacion.Sum(f => f.Importe),
                TotalCitas = facturacion.Count
            };
        }

        private List<FacturacionChartViewModel> ObtenerDatosChartMensual(List<Negocio.Persistencia.Modelos.Facturacion> facturacion)
        {
            int anio = DateTime.Today.Year; 

            return Enumerable.Range(1, 12) 
                .Select(mes => new FacturacionChartViewModel
                {
                    Periodo = $"{anio}-{mes:D2}",
                    TotalFacturacion = facturacion
                        .Where(f => f.FechaFactura.Year == anio && f.FechaFactura.Month == mes) 
                        .Sum(f => f.Importe), 
                    NumeroCitas = facturacion
                        .Count(f => f.FechaFactura.Year == anio && f.FechaFactura.Month == mes) 
                })
                .ToList();
        }

        [HttpGet, AjaxOnly]
        public async Task<ActionResult> ModalCalcularFactura()
        {
            JsonResponse? jsonResponse = new JsonResponse("400", "Error en el servidor", "");
            string data = await _renderService.ToStringAsync("FormCalcularFactura", new object());
            jsonResponse = new JsonResponse("200", "Operación realizada correctamente.", data);
            return new JsonResult(jsonResponse);
        }

        [HttpPost, AjaxOnly]
        public JsonResult CalcularFactura()
        {
            try
            {
                _gestionFacturacion.RecalcularFacturacionGlobal();
                return new JsonResult(new { StatusCode = 200, message = "Facturación recalculada correctamente." });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { StatusCode = 500, message = "Error al recalcular facturación: " + ex.Message });
            }
        }
    }
}

