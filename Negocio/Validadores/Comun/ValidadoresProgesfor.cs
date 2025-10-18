using Azure;
using Microsoft.Extensions.Logging;
using Negocio.Persistencia;
using Negocio.Persistencia.Modelos;
using Negocio.Persistencia.Modelos.Comun;
using Negocio.Servicios;
using System.Diagnostics;
using System.Reflection;
namespace Negocio.Validadores.Comun
{
    /// <summary>
    /// Clase encargada de gestionar las funciones de los validadores 
    /// </summary>
    public class ValidadoresProgesfor : IValidadoresProgesfor
    {
        private readonly IDictionary<Type, IDictionary<string, IValidador>> cacheValidadores = new Dictionary<Type, IDictionary<string, IValidador>>();
        private ILogger<ValidadoresProgesfor> _logger;

        /// <summary>
        /// Contexto de la base de datos
        /// </summary>
        protected EpsilonDbContext DbContext { get; set; }
        
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="dbContext">Parametro de entrada con el contexto de la base de datos</param>
        /// <param name="logger">Parametro de entrada de la interfaz ILogger</param>
        public ValidadoresProgesfor(EpsilonDbContext dbContext, ILogger<ValidadoresProgesfor> logger)
        {
            DbContext = dbContext;
            _logger = logger;
            RegistraValidatores();
            _logger.LogInformation("Validadores registrados: " + string.Join(", ", cacheValidadores.Keys.Select(t => t.Name)));
        }

        /// <summary>
        /// Metodo encargado de Validar las entidades que reciba
        /// </summary>
        /// <typeparam name="T">Tipo de contexto</typeparam>
        /// <param name="entidad">Parametro de entrada que contiene la entidad a validar</param>
        /// <returns></returns>
        public IValidador<T>? GetValidador<T>(T entidad) where T : ProgesforModel
        {
            return GetValidador(entidad, OperacionesValidacion.OPERACION_INSERTAR);
        }

        /// <summary>
        /// Metodo encargado de Validar las entidades que reciba
        /// </summary>
        /// <typeparam name="T">Tipo de contexto</typeparam>
        /// <param name="entidad">Parametro de entrada que contiene la entidad a validar</param>
        /// <param name="operacion">Parametro de entrada que contiene la operacion a validar</param>
        /// <returns></returns>
        public IValidador<T>? GetValidador<T>(T entidad, string operacion) where T : ProgesforModel
        {
            if (entidad == null)
            {
                throw new ArgumentNullException(nameof(entidad));
            }

            IValidador<T>? result = null;

            Type type = typeof(T);
            _logger.LogTrace(GetEventId(), MethodBase.GetCurrentMethod()?.Name + " (" + type.FullName + ").");
            if (cacheValidadores.ContainsKey(type) && cacheValidadores[type].ContainsKey(operacion))
            {
                result = (IValidador<T>)cacheValidadores[type][operacion];

            }
            return result;
        }

        /// <summary>
        /// Metodo encargado de validar un texto
        /// </summary>
        /// <typeparam name="T">Tipo de contexto</typeparam>
        /// <returns></returns>
        public IValidador<T>? GetValidador<T>() where T : ProgesforModel
        {
            return GetValidador<T>(OperacionesValidacion.OPERACION_INSERTAR);
        }

        /// <summary>
        /// Metodo que contiene la funcionalidad necesaria para obtener un validador
        /// </summary>
        /// <typeparam name="T">Tipo de contexto</typeparam>
        /// <param name="operacion">Tipo de operacion a validar</param>
        /// <returns></returns>
        public IValidador<T>? GetValidador<T>(string operacion) where T : ProgesforModel
        {
            IValidador<T>? result = null;
            Type type = typeof(T);
            _logger.LogTrace(GetEventId(), MethodBase.GetCurrentMethod()?.Name + " (" + type.FullName + ").");
            if (cacheValidadores.ContainsKey(type) && cacheValidadores[type].ContainsKey(operacion))
            {
                result = (IValidador<T>?)cacheValidadores[type][operacion];
            }
            return (IValidador<T>?)result;
        }

        /// <summary>
        /// Metodo que contiene la funcionalidad necesaria para realizar modificaciones sobre un validador
        /// </summary>
        /// <typeparam name="T">Tipo de contexto</typeparam>
        /// <param name="validador">Parametro de entrada con el validador a modificar</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidCastException"></exception>
        public void SetValidador<T>(IValidador<T> validador) where T : ProgesforModel
        {
            if (validador == null) throw new ArgumentNullException(nameof(validador));
            if (!typeof(IValidador).IsAssignableFrom(validador.GetType())) throw new InvalidCastException(nameof(validador));

            Type type = typeof(T);
            if (cacheValidadores.ContainsKey(type))
            {
                _logger.LogTrace(GetEventId(), "Se asigna el validador (" + type.Name + ").");
                cacheValidadores[type][validador.GetOperacion()] = validador;
            }
            else
            {
                _logger.LogTrace(GetEventId(), "Se registra el validador (" + type.Name + ").");
                cacheValidadores.Add(type, new Dictionary<string, IValidador>() { { OperacionesValidacion.OPERACION_INSERTAR, validador } });
            }
        }

        /// <summary>
        /// Metodo encargado de realizar modificaciones a un validador
        /// </summary>
        /// <param name="validador">Parametro de entrada que contiene el validador a modificar</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void SetValidador(IValidador validador)
        {
            if (validador == null) throw new ArgumentNullException(nameof(validador));

            Type type = validador.GetTypeEntidadValidada();
            if (cacheValidadores.ContainsKey(type))
            {
                _logger.LogTrace(GetEventId(), "Se asigna el validador (" + type.Name + ").");
                cacheValidadores[type][validador.GetOperacion()] = validador;
            }
            else
            {
                _logger.LogTrace(GetEventId(), "Se registra el validador (" + type.Name + ").");
                cacheValidadores.Add(type, new Dictionary<string, IValidador>() { {validador.GetOperacion(), validador } });
            }
        }

        /// <summary>
        /// Metodo encargado de verificar si existe validador o no
        /// </summary>
        /// <param name="type"></param>
        public bool ContainsValidadores(Type type)
        {
            return cacheValidadores.ContainsKey(type);
        }

        /// <summary>
        /// Metodo sobrecargado encargado de verificar si existe validador o no
        /// </summary>
        /// <param name="type"></param>
        /// <param name="operacion"></param>
        /// <returns></returns>
        public bool ContainsValidador(Type type, string operacion)
        {
            return cacheValidadores.ContainsKey(type) && cacheValidadores[type].ContainsKey(operacion);
        }

        /// <summary>
        /// Metodo encargado de comprobar si un validador ha sido eliminado
        /// </summary>
        /// <param name="type">Parametro de entrada de tipo</param>
        /// <returns></returns>
        public bool RemoveValidadores(Type type)
        {
            _logger.LogTrace(GetEventId(), MethodBase.GetCurrentMethod()?.Name + " (" + type.FullName + ").");
            return cacheValidadores.Remove(type);
        }

        /// <summary>
        /// Metodo encargado de eliminar un validador
        /// </summary>
        /// <param name="type">Parametro de entrada de tipo</param>
        /// <param name="operacion">Parametro de entrada con el tipo de operacion</param>
        /// <returns></returns>
        public bool RemoveValidador(Type type, string operacion)
        {
            _logger.LogTrace(GetEventId(), MethodBase.GetCurrentMethod()?.Name + " (" + type.FullName + ").");

            bool result = false;
            if (ContainsValidador(type, operacion)) result = cacheValidadores[type].Remove(operacion);
            return result;
        }

        /// <summary>
        /// Metodo encargado de eliminar un validador
        /// </summary>
        /// <typeparam name="T">Tipo de contexto</typeparam>
        /// <param name="validator">Parametro de entrada con el validador</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public bool RemoveValidador<T>(IValidador<T> validator) where T : ProgesforModel
        {
            _logger.LogTrace(GetEventId(), MethodBase.GetCurrentMethod()?.Name);

            if (validator == null) throw new ArgumentNullException(nameof(validator));
            Type type = typeof(T);
            _logger.LogTrace(GetEventId(), MethodBase.GetCurrentMethod()?.Name + " (" + type.FullName + ").");
            return RemoveValidador(type, validator.GetOperacion());
        }

        /// <summary>
        /// Metodo encargado de realizar una limieza o reseteo de los validadores
        /// </summary>
        public void Clear()
        {
            _logger.LogTrace(GetEventId(), MethodBase.GetCurrentMethod()?.Name);
            cacheValidadores.Clear();
        }

        /// <summary>
        /// Metodo encargado de contar el numero de validadores
        /// </summary>
        public int Count()
        {
            return cacheValidadores.Count;
        }
        
        /// <summary>
        /// Metodo encargado de validar entidades
        /// </summary>
        /// <typeparam name="T">Tipo de contexto</typeparam>
        /// <param name="entidad">Entidad a validar que recibe el metodo</param>
        /// <returns></returns>
        public IEnumerable<string> Valida<T>(T entidad) where T : ProgesforModel
        {
            if (entidad == null) return Enumerable.Empty<string>();
            _logger.LogTrace(GetEventId(), MethodBase.GetCurrentMethod()?.Name + " (" + entidad.GetType().Name + ").");
            IValidador<T>? validador = GetValidador<T>();
            return validador != null ? validador.Valida(entidad) : Enumerable.Empty<string>();
        }

        /// <summary>
        /// Metodo encargado de registrar validadores
        /// </summary>
        protected virtual void RegistraValidatores()
        {
            _logger.LogInformation(GetEventId(), MethodBase.GetCurrentMethod()?.Name);

            cacheValidadores.Clear();
            IEnumerable<Type> tiposValidadores = Assembly.GetExecutingAssembly().GetTypes().Where(p => typeof(IValidador).IsAssignableFrom(p) && p.IsClass && !p.IsAbstract && p.Namespace == "Negocio.Validadores");
            foreach (Type tipoValidador in tiposValidadores)
            {
                try
                {
                    Type? baseValidador = tipoValidador.BaseType;
                    if (baseValidador != null && baseValidador.GenericTypeArguments.Length > 0)
                    {
                        Type tipoEntidad = baseValidador.GenericTypeArguments[0];
                        IValidador? valid = CreaValidador(tipoValidador);
                        if (valid != null)
                        {
                            SetValidador(valid);
                        }
                        else
                        {
                            _logger.LogInformation(GetEventId(), "El validador (" + tipoValidador.FullName + ") no se pudo crear.");
                        }
                    }
                    else
                    {
                        _logger.LogInformation(GetEventId(), "El validador (" + tipoValidador.FullName + ") no implementa IValidador<T>.");
                    }
                }
                catch (Exception ex) // El fallo en el registro de un validador no aborta el proceso para el resto
                {
                    _logger.LogInformation(GetEventId(), "Se ha producido un error al cargar el validador (" + tipoValidador.Name + ").\n Error: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Metodo encargado de buscar validadores
        /// </summary>
        /// <typeparam name="T">Tipo de contexto</typeparam>
        /// <returns></returns>
        protected virtual IValidador<T>? BuscaValidador<T>() where T : ProgesforModel
        {
            _logger.LogInformation(GetEventId(), MethodBase.GetCurrentMethod()?.Name);

            Type tipoABuscar = typeof(Type);
            IEnumerable<Type> tiposValidadores = Assembly.GetExecutingAssembly().GetTypes().Where(p => typeof(IValidador).IsAssignableFrom(p) && p.IsClass && !p.IsAbstract && p.Namespace == "Negocio.Validadores");
            foreach (Type tipoValidador in tiposValidadores)
            {
                try
                {
                    Type? baseValidador = tipoValidador.BaseType;
                    if (baseValidador != null && baseValidador.GenericTypeArguments.Length > 0)
                    {
                        Type tipoEntidad = baseValidador.GenericTypeArguments[0];
                        if (tipoABuscar == tipoEntidad)
                        {
                            return (IValidador<T>?)CreaValidador(tipoValidador);
                        }
                    }
                    else
                    {
                        _logger.LogInformation(GetEventId(), "El validador (" + tipoValidador.FullName + ") no implementa IValidador<T>.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(GetEventId(), "Se ha producido un error al obtener el validador (" + tipoValidador.Name + ").\n Error: " + ex.Message);
                    throw;
                }
            }
            return null;
        }

        /// <summary>
        /// Metodo encargado de obtener el evento
        /// </summary>
        /// <returns></returns>
        protected virtual EventId GetEventId()
        {
            return new EventId((GetType().FullName ?? "ValidadoresProgesfor").GetHashCode(), GetType().FullName);
        }

        /// <summary>
        /// Metodo encargado de crear validador
        /// </summary>
        /// <param name="tipoValidador">Parametro de entrada con el tipo de validador</param>
        /// <returns></returns>
        protected virtual IValidador? CreaValidador(Type tipoValidador)
        {
            _logger.LogTrace(GetEventId(), MethodBase.GetCurrentMethod()?.Name);
            if (tipoValidador == null) return null;

            IValidador? result = null;
            Type[] types = new Type[1] { typeof(EpsilonDbContext) };
            ConstructorInfo? constructor = tipoValidador.GetConstructor(
            BindingFlags.Instance | BindingFlags.Public, null, CallingConventions.HasThis, types, null);
            if (constructor != null)
            {
                try
                {
                    result = (IValidador?)constructor.Invoke(new object[] { DbContext });
                    _logger.LogTrace(GetEventId(), "Se ha creado el validador (" + tipoValidador.FullName + ").");
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(GetEventId(), "El validador (" + tipoValidador.FullName + ") fallo en su creación con el error: " + ex.Message);
                }
            }
            else
            {
                _logger.LogTrace(GetEventId(), "El validador (" + tipoValidador.FullName + ") no tiene un constructor válido.");
            }
            return result;
        }
    }
}
