using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Epsilon.ViewModels
{
    public class ViewFormAgregarUsuario
    {
        public int IdUsuario { get; set; }

        [BindProperty]
        public string? Nombre { get; set; }

        [BindProperty]
        public string? Password { get; set; }

        [BindProperty]
        public string? Email { get; set; } = null;

        [DataType(DataType.Date)]
        [BindProperty]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaAlta { get; set; }

        [BindProperty]
        public int Telefono { get; set; }

        [BindProperty]
        public string? RutaFoto { get; set; }

        public bool Activo { get; set; }

        //public SelectList? TurnoDeTrabajo { get; set; }
    }
}
