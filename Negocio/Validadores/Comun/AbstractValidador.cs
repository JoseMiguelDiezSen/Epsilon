using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Negocio.Validadores.Comun
{
    /// <summary> Metodo encargado de obtener los subplanes correspondientes a un plan especifico </summary>
    public abstract class AbstractValidador<T> : AbstractValidator<T>, IValidador<T> where T : class
    {
        /// <summary> Metodo encargado de obtener el tipo de entidad validada </summary>
        public Type GetTypeEntidadValidada()
        {
            return typeof(T);
        }
        /// <summary> Metodo encargado de obtener la operacion correspondiente </summary>
        public virtual string GetOperacion()
        {
            return OperacionesValidacion.OPERACION_INSERTAR;
        }

        /// <summary> Metodo encargado de validar entidades </summary>
        public IEnumerable<string> Valida(T entidad)
        {
            var result = Validate(entidad);
            return result.Errors.Select(e => e.ToString());
        }

    }
}
