﻿using Calipso.Security;
using Epsilon.Renders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Negocio.Servicios;

namespace Epsilon.Controllers
{
    public class RadiologiaController : AbstractSecurityController
    {
        private readonly IRazorRenderService _renderService;
        private IGestionUsuarios _gestionUsuarios;

        /// <summary>
        /// Constructor d
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="seguridad"></param>
        /// <param name="gestionUsuarios"></param>
        public RadiologiaController(ILogger<RadiologiaController> logger, ISeguridad seguridad, IGestionUsuarios gestionUsuarios, IRazorRenderService renderService) : base(logger, seguridad)
        {
            _gestionUsuarios = gestionUsuarios;
            _renderService = renderService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
