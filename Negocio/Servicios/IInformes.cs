using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Servicios
{
    public interface IInformes : IServicioEpsilon
    {
        /// <summary>  
        /// Genera un informe en el formato especificado.  
        /// </summary>  
        /// <param name="path">La ruta del informe.</param>  
        /// <param name="nombre">El nombre del informe.</param>  
        /// <param name="parametros">Los parámetros del informe.</param>  
        /// <param name="formato">El formato del informe (por defecto es "PDF").</param>  
        /// <returns>El informe generado en formato de bytes.</returns>  
        byte[] GeneraInforme(string path, string nombre, Dictionary<string, string> parametros, string formato = "PDF");

    }
}
