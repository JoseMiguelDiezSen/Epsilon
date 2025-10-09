using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Validadores.Comun
{
    /// <summary> Interfaz de IValidador </summary>
    public interface IValidador
    {
        /// <summary> Instancia del metodo GetTypeEntidadValidada() </summary>
        Type GetTypeEntidadValidada();

        /// <summary> Instancia del metodo GetOperacion() </summary>
        string GetOperacion();
    }

    /// <summary> Interfaz sobrecargada de IValidador </summary>
    public interface IValidador<T>: IValidador where T : class
    {
        /// <summary> Instancia del metodo Valida() </summary>
        IEnumerable<string> Valida(T entidad);
    }
}
