using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Negocio.Persistencia.Modelos;
using System.ComponentModel.DataAnnotations;

namespace Epsilon.ViewModels
{
    public class ViewFormAgregarPeriodo
    {
        [BindProperty]
        public long IdPeriodo { get; set; }

        //[BindProperty]
        //public int IdMiTipo { get; set; } = (int)EnumTipoEntidad.PermisoTipo;

        [DataType(DataType.Date)]
        [BindProperty]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Desde { get; set; }


        [DataType(DataType.Date)]
        [BindProperty]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Hasta { get; set; }

        public int Ejercicio { get; set; }

        public int EjercicioDestino { get; set; }

        public SelectList EjerciciosAnteriores { get; set; } = new SelectList(Enumerable.Empty<int>());
    }
}
