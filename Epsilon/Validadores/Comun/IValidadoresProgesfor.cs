using FluentValidation;
using FluentValidation.Results;
using Negocio.Persistencia.Modelos.Comun;

namespace Negocio.Validadores.Comun
{
    /// <summary>
    /// Interfaz correspondiente a la clase Validadores Progesfor
    /// </summary>
    public interface IValidadoresProgesfor
    {
        /// <summary>
        /// Metodo que contiene la funcionalidad necesaria para limpiar 
        /// </summary>
        void Clear();

        /// <summary>
        /// Metodo booleano que contiene la funcionalidad necesaria para comprobar si un tipo u operacion contiene validador
        /// </summary>
        /// <param name="type">Parametro de entrada con el tipo</param>
        /// <param name="operacion">Parametro de entrada con la operacion</param>
        /// <returns></returns>
        bool ContainsValidador(Type type, string operacion);

        /// <summary>
        /// Metodo booleano que contiene la funcionalidad necesaria para comprobar si un tipo contiene validador
        /// </summary>
        /// <param name="type">Parametro de entrada con el tipo</param>
        bool ContainsValidadores(Type type);
        
        /// <summary>
        /// Variable definida encargada de realizar conteos de items
        /// </summary>
        /// <returns></returns>
        int Count();

        /// <summary>
        /// Metodo que contiene la funcionalidad nesesaria para obtener los validadores existentes
        /// </summary>
        /// <typeparam name="T">Tipo de contexto</typeparam>
        /// <returns></returns>
        IValidador<T>? GetValidador<T>() where T : ProgesforModel;

        /// <summary>
        /// Metodo que contiene la funcionalidad nesesaria para obtener los validadores existentes en funcion de la operacion
        /// </summary>
        /// <typeparam name="T">Tipo de contexto</typeparam>
        /// <param name="operacion">Parametro de entrada que contiene la operacion</param>
        /// <returns></returns>
        IValidador<T>? GetValidador<T>(string operacion) where T : ProgesforModel;

        /// <summary>
        /// Metodo encargado de obtener los validadores en funcion de la entidad
        /// </summary>
        /// <typeparam name="T">Tipo de contexto</typeparam>
        /// <param name="entidad">Parametro de entrada que contiene la entidad de la cual se quiere obtener el validador</param>
        /// <returns></returns>
        IValidador<T>? GetValidador<T>(T entidad) where T : ProgesforModel;

        /// <summary>
        /// Metodo encargado de obtener los validadores existentes
        /// </summary>
        /// <typeparam name="T">Tipo de contexto</typeparam>
        /// <param name="entidad">Parametro de entrada que contiene la entidad de la cual se quiere obtener el validador</param>
        /// <param name="operacion">Parametro de entrada que indica la operacion</param>
        /// <returns></returns>
        IValidador<T>? GetValidador<T>(T entidad, string operacion) where T : ProgesforModel;

        /// <summary> Metodo que contiene la funcionalidad necesaria para eliminar un validador en funcion del tipo y la operacion recibidos </summary>
        /// <param name="type">Parámetro de entrada que contiene el tipo</param>
        /// <param name="operacion">Parámetro de entrada que contiene la operacion</param>
        bool RemoveValidador(Type type, string operacion);

        /// <summary>
        /// Metodo que contiene la funcionalidad necesaria para eliminar un validador
        /// </summary>
        /// <param name="validator">Parámetro de entrada que contiene el validador a eliminar</param>
        bool RemoveValidador<T>(IValidador<T> validator) where T : ProgesforModel;

        /// <summary>
        /// Metodo que contiene la funcionalidad necesaria para eliminar un validador en funcion del tipo
        /// </summary>
        /// <param name="type">Parámetro de entrada que contiene el tipo</param>
        bool RemoveValidadores(Type type);
        
        /// <summary>
        /// Metodo que contiene la funcionalidad necesaria para modificar un validador
        /// </summary>
        /// <param name="validador">Parámetro de entrada que contiene el validador</param>
        void SetValidador(IValidador validador);

        /// <summary>
        /// Metodo que contiene la funcionalidad necesaria para modificar un validador
        /// </summary>
        /// <typeparam name="T">Tipo de contexto</typeparam>
        /// <param name="validador">Parametro de entrada que contiene el validador a modificar</param>
        void SetValidador<T>(IValidador<T> validador) where T : ProgesforModel;

        /// <summary>
        /// Metodo que contiene la funcionalidad necesaria para realizar la validacion de una entidad que se le pase
        /// </summary>
        /// <typeparam name="T">Tipo de contexto</typeparam>
        /// <param name="entidad">Parametro de entrada que contiene la entidad a validar</param>
        /// <returns></returns>
        IEnumerable<string> Valida<T>(T entidad) where T : ProgesforModel;
    }
}