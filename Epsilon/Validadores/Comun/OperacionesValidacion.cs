using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Validadores.Comun
{
    /// <summary>
    /// Clase encargada de registrar las posibles operaciones que se puedan realizar
    /// </summary>
    public static class OperacionesValidacion
    {
        /// <summary>
        /// Variable constante para indicar cuando una operacion es de tipo consulta
        /// </summary>
        public const string OPERACION_CONSULTAR = "Consultar";

        /// <summary>
        /// Variable constante para indicar cuando una operacion es de tipo insertar
        /// </summary>
        public const string OPERACION_INSERTAR = "Insertar";

        /// <summary>
        /// Variable constante para indicar cuando una operacion es de tipo modificar
        /// </summary>
        public const string OPERACION_MODIFICAR = "Modificar";

        /// <summary>
        /// Variable constante para indicar cuando una operacion es de tipo eliminar
        /// </summary>
        public const string OPERACION_ELIMINAR = "Eliminar";

        /// <summary>
        /// Variable constante para indicar cuando una operacion es de tipo copiar
        /// </summary>
        public const string OPERACION_COPIAR = "Copiar";

        /// <summary>
        /// Variable constante para indicar cuando una operacion es de tipo insertar asociado
        /// </summary>
        public const string OPERACION_INSERTAR_ASOCIADO = "InsertarAsociado";

        /// <summary>
        /// Variable constante para indicar cuando una operacion es de tipo modificar especial
        /// </summary>
        public const string OPERACION_MODIFICAR_ESPECIAL = "ModificarEspecial";

        /// <summary>
        /// Variable constante para indicar cuando una operacion es de tipo eliminar todos
        /// </summary>
        public const string OPERACION_ELIMINAR_TODOS = "EliminarTodos";

        /// <summary>
        /// Variable constante para indicar cuando una operacion es de tipo copiar todos
        /// </summary>
        public const string OPERACION_COPIAR_TODOS = "CopiarTodos";

        // Añadir aquí nuevas operaciones ...
    }
}
