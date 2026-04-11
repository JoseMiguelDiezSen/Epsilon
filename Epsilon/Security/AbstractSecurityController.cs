using Microsoft.AspNetCore.Mvc;
using Negocio.Servicios;

namespace Calipso.Security
{
    public class AbstractSecurityController : Controller
    {
        protected ILogger _logger;



        protected AbstractSecurityController()
        {
          
        }

        protected AbstractSecurityController(ILogger logger)
        {
            _logger = logger;
        }
    }
}
