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
        /// <summary>
        /// Constructor del servicio
        /// </summary>
        /// <param name="context"></param>
        public GestionPacientes(EpsilonDbContext context, ILogger<GestionPacientes> logger,IValidadoresProgesfor registroValidadores) : base(context, logger, registroValidadores)
        {
            logger.LogTrace(GetEventId(), "Servicion iniciado");
        }

        /// <summary>
        /// Metodo para añadir pacientes
        /// </summary>
        /// <param name="paciente"> Paciente a añadir </param>
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
        /// <param name="paciente"> Paciente a actualizar </param>
        /// <returns></returns>
        public Paciente UpdatePaciente(Paciente paciente)
        {
            var pacienteActualizado = Context.Pacientes.Single(u => u.IdPaciente == paciente.IdPaciente);
            var entity = Context.Pacientes.Update(pacienteActualizado);
            Context.SaveChanges();
            return entity.Entity;
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
        /// Metodo para obtener un paciente mediante su ID
        /// </summary>
        /// <param name="idPaciente"> Identificador del paciente </param>
        /// <returns></returns>
        public Paciente GetPaciente(int idPaciente)
        {
            // Opcion 1
            var paciente = Context.Pacientes.Where(u => u.IdPaciente == idPaciente).First();
            return paciente;

            //Opcion 2
            //return Context.Usuarios.Single(u => u.IdUsuario == idUsuario);
        }

        public IQueryable<DatosPacientes> GetDatosPacientes()
        {
            logger.LogTrace(GetEventId(), MethodBase.GetCurrentMethod()?.Name);
            return Context.DatosPacientes;
        }

        /// <summary>
        /// Obtiene los datos del Detalle del paciente
        /// </summary>
        /// <param name="idPaciente"></param>
        /// <returns></returns>
        public DatosPacientes? GetDetallePaciente(int idPaciente)
        {
            return Context.DatosPacientes.FirstOrDefault(p => p.IdPaciente == idPaciente);
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

        #region EnvioCorreoElectonico

        /// <summary>
        /// Añade un modelo de correo electronico 
        /// </summary>
        /// <param name="correoElectronico"> Modelo de correo electronico a añadir </param>
        /// <returns></returns>
        public bool AddModeloCorreo(CorreosElectronicos correoElectronico)
        {
            try
            {
                Context.Add(correoElectronico);
                Context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Elimina un modelo de correo electronico por su ID
        /// </summary>
        /// <param name="idCorreo"> Identidicador del modelo de correo electronico </param>
        /// <returns></returns>
        public bool EliminarModeloCorreo(int idCorreo)
        {
            var correo = Context.CorreoElectronico.Find(idCorreo);

            if (correo == null)
                return false;

            Context.CorreoElectronico.Remove(correo);
            Context.SaveChanges();
            return true;
        }

        #endregion
    }
}
