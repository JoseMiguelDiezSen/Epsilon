using Microsoft.Extensions.Logging;
using Negocio.Excepciones;
using Negocio.Persistencia;
using Negocio.Persistencia.Modelos;
using Negocio.Servicios.Comun;
using Negocio.Validadores.Comun;

namespace Negocio.Servicios
{
    public class Configuracion : ServicioAbstractoEpsilon, IConfiguracion
    {
        protected ISeguridad _seguridad;

        /// <summary>
        /// Constructor del servicio
        /// </summary>
        /// <param name="context"></param>
        public Configuracion(EpsilonDbContext context, ILogger<Configuracion> logger, ISeguridad seguridad, IValidadoresProgesfor registroValidadores) : base(context, logger, registroValidadores)
        {
            _registroValidadores = registroValidadores;
            _seguridad = seguridad;
            logger.LogTrace(GetEventId(), "Servicion iniciado");
        }

        /// <summary>
        /// Alta de un nuevo correo electrónico
        /// </summary>
        /// <param name="correoElectronico"></param>
        /// <returns></returns>
        public bool GuardarCorreoNuevo(CorreosElectronicos correoElectronico)
        {
            using (var trans = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.Add(correoElectronico);
                    Context.SaveChanges();
                    trans.Commit();
                }
                catch (ValidacionException ex)
                {
                    ex.Message.ToString();
                    trans.Rollback();
                }
            }
            return true;
        }

        /// <summary>
        /// Actualiza los datos de un correo electrónico existente
        /// </summary>
        /// <param name="correoElectronico"></param>
        /// <returns></returns>
        public bool ActualizarDatosCorreo(CorreosElectronicos correoElectronico)
        {
            using (var trans = Context.Database.BeginTransaction())
            {
                var entity = Context.CorreoElectronico.Update(correoElectronico);
                Context.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// Elimina un modelo de correo electrónico
        /// </summary>
        /// <param name="idCorreo"></param>
        /// <returns></returns>
        public bool EliminarModeloCorreo(int idCorreo)
        {
            var correo = Context.CorreoElectronico.Find(idCorreo);

            if (correo != null)
            {
                Context.CorreoElectronico.Remove(correo);
                Context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}