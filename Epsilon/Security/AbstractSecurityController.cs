using Microsoft.AspNetCore.Mvc;
using Negocio.Servicios;
using System.Security.Principal;

namespace Calipso.Security
{
    public class AbstractSecurityController : Controller
    {
        protected ILogger _logger;
        protected ISeguridad _seguridad;

        protected AbstractSecurityController(ILogger logger, ISeguridad seguridad)
        {
            _logger = logger;
            _seguridad = seguridad;
        }
    }
}
