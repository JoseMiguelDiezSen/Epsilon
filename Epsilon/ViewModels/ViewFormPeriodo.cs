using Microsoft.AspNetCore.Mvc;
using Negocio.Persistencia.Modelos;

namespace Epsilon.ViewModels
{
    public class ViewFormPeriodo
    {
        [BindProperty]
        public long IdPeriodo { get; set; }

        //[BindProperty]
        //public int IdMiTipo { get; set; } = (int)EnumTipoEntidad.PermisoTipo;
    


    }
}
