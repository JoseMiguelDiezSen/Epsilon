using Calipso.Security;
using Epsilon.Models;
using Epsilon.Renders;
using Epsilon.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Negocio.Persistencia.Modelos;
using Negocio.Servicios;

namespace Epsilon.Controllers
{
    public class MedicosController : AbstractSecurityController
    {
        private IRazorRenderService _renderService;
        private IGestionMedicos _gestionMedicos;

        /// <summary>
        /// Constructor d
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="seguridad"></param>
        /// <param name="gestionUsuarios"></param>
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
    }
}
