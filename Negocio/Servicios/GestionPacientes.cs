using Microsoft.Extensions.Logging;
using Negocio.Persistencia;
using Negocio.Persistencia.Modelos;
using Negocio.Servicios.Comun;
using Negocio.Validadores.Comun;
using System.Reflection;

namespace Negocio.Servicios
{
    public class GestionPacientes : ServicioAbstractoEpsilon , IGestionPacientes
    {
        protected ISeguridad _seguridad;

        /// <summary>
        /// Constructor del servicio
        /// </summary>
        /// <param name="context"></param>
        public GestionPacientes(EpsilonDbContext context, ILogger<GestionPacientes> logger, ISeguridad seguridad, IValidadoresProgesfor registroValidadores) : base(context, logger, registroValidadores)
        {
            _seguridad = seguridad;
            logger.LogTrace(GetEventId(), "Servicion iniciado");
        }

        /// <summary>
        /// Metodo para añadir pacientes
        /// </summary>
        /// <param name="paciente"></param>
        /// <returns></returns>
        public Paciente AddPaciente(Paciente paciente)
        {
            using (var transaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.Add(paciente);
                    Context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                    transaction.Rollback();
                }
            }
            return (paciente);
        }

        /// <summary>
        /// Metodo para actualizar un paciente
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public Paciente UpdatePaciente(Paciente paciente)
        {
            var pacienteActualizado = Context.Pacientes.Single(u => u.IdPaciente == paciente.IdPaciente);
            var entity = Context.Pacientes.Update(pacienteActualizado);
            Context.SaveChanges();
            return entity.Entity;
        }

        /// <summary>
        /// Metodo para obtener un paciente
        /// </summary>
        /// <param name="idPaciente"></param>
        /// <returns></returns>
        public Paciente GetPaciente(int idPaciente)
        {
            // Opcion 1
            var paciente = Context.Pacientes.Where(u => u.IdPaciente == idPaciente).First();
            return paciente;

            //Opcion 2
            //return Context.Usuarios.Single(u => u.IdUsuario == idUsuario);
        }

        /// <summary>
        /// Metodo para eliminar un paciente
        /// </summary>
        /// <param name="idPaciente"></param>
        /// <returns></returns>
        public bool DeletePaciente(int idPaciente)
        {
            //Opcion 1 con Single
            var paciente = Context.Pacientes.Single(u => u.IdPaciente == idPaciente);

            // Opcion 2 con Where y First()
            var paciente1 = Context.Pacientes.Where(u => u.IdPaciente == idPaciente).First();
            Context.Pacientes.Remove(paciente);
            return true;
        }

        /// <summary>
        /// Metodo para obtener los pacientes
        /// </summary>
        /// <returns></returns>
        public IQueryable<Paciente> GetAllPacientes()
        {
            logger.LogTrace(GetEventId(), MethodBase.GetCurrentMethod()?.Name);
            return Context.Pacientes;
        }
    }
}
