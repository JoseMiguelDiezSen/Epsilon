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

        /// <summary>
        /// Constructor d
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="seguridad"></param>
        /// <param name="gestionUsuarios"></param>
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

        /// <summary>
        /// Obtiene los datos de facturación para refrescar el chart en tiempo real.
        /// </summary>
        [HttpGet, AjaxOnly]
        public IActionResult ObtenerDatosFacturacion()
        {
            var facturacion = _gestionFacturacion.GetFacturacion().ToList(); // Carga todos los registros de facturación

            return Json(new
            {
                datosChart = ObtenerDatosChartMensual(facturacion), // Datos de los 12 meses para el chart
                totalFacturacion = facturacion.Sum(f => f.Importe), // Suma de todos los importes facturados
                totalCitas = facturacion.Count // Número total de citas facturadas
            });
        }

        /// <summary>
        /// Construye el ViewModel de la página con los datos de facturación reales del año actual.
        /// </summary>
        private FacturacionViewModel CargarViewModel()
        {
            var facturacion = _gestionFacturacion.GetFacturacion().ToList();

            return new FacturacionViewModel
            {
                DatosChart = ObtenerDatosChartMensual(facturacion), // Mensual para el chart
                TotalFacturacion = facturacion.Sum(f => f.Importe),
                TotalCitas = facturacion.Count
            };
        }

        /// <summary>
        /// Prepara los 12 meses del año actual con el total facturado y el número de citas de cada mes.
        /// </summary>
        private List<FacturacionChartViewModel> ObtenerDatosChartMensual(List<Negocio.Persistencia.Modelos.Facturacion> facturacion)
        {
            int anio = DateTime.Today.Year; // El chart siempre muestra el año en curso

            return Enumerable.Range(1, 12) // Recorre los 12 meses del año
                .Select(mes => new FacturacionChartViewModel
                {
                    Periodo = $"{anio}-{mes:D2}",
                    TotalFacturacion = facturacion
                        .Where(f => f.FechaFactura.Year == anio && f.FechaFactura.Month == mes) // Filtra las facturas de ese mes
                        .Sum(f => f.Importe), // Suma el importe de ese mes (0 si no hubo facturas)
                    NumeroCitas = facturacion
                        .Count(f => f.FechaFactura.Year == anio && f.FechaFactura.Month == mes) // Nº de citas facturadas ese mes
                })
                .ToList();
        }



        [HttpGet, AjaxOnly]
        public async Task<ActionResult> ModalCalcularFactura()
        {
            JsonResponse? jsonResponse = new JsonResponse("400", "Error en el servidor", "");
            ViewFormAgregarPaciente vmAgregarPaciente = new ViewFormAgregarPaciente();
            vmAgregarPaciente.FechaAlta1 = DateTime.Now;
            string data = await _renderService.ToStringAsync("FormCalcularFactura", vmAgregarPaciente);
            jsonResponse = new JsonResponse("200", "Operación realizada correctamente.", data);
            return new JsonResult(jsonResponse);
        }
    }
}
