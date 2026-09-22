using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;

namespace Epsilon.ViewModels
{
    public class ViewFormAgregarMedico
    {
        [BindProperty]
        public int IdMedico { get; set; }

        [BindProperty]
        public string? NombreMedico { get; set; }

        [BindProperty]
        public string? DNI { get; set; }

        [BindProperty]
        public int NumeroColegiado { get; set; }

        [BindProperty]
        public string? Especialidad { get; set; }

        [BindProperty]
        public string? Telefono { get; set; }

        [BindProperty]
        public string? EMail { get; set; }

        [BindProperty]
        public string? FechaContratacion { get; set; }

        [BindProperty]
        public bool Activo { get; set; } = true;

        [BindProperty]
        public string? Observaciones { get; set; }

        [BindProperty]
        public int? IdClinica { get; set; }

        [BindProperty]
        public int? IdUsuario { get; set; }

        public IFormFile? Foto { get; set; }

        public string? FotoBase64 { get; set; }

        public SelectList? Clinicas { get; set; }
    }
}
