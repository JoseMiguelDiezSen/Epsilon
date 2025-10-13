using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Excepciones
{
    public class ValidacionException : Exception
    {
        Type TipoEntidad { get; set; }
        public IEnumerable<string> Errores { get; set; } = Enumerable.Empty<string>();
        public ValidacionException(Type tEntidad, IEnumerable<string> lErrores) : base (string.Join("\n",lErrores)) 
        {
            TipoEntidad = tEntidad;
            Errores = lErrores; 
        }
    }
}
