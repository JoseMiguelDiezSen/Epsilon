using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Negocio.Excepciones;
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

        public IQueryable<DatosPacientes> GetDatosPacientes()
        {
            logger.LogTrace(GetEventId(), MethodBase.GetCurrentMethod()?.Name);
            return Context.DatosPacientes;
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

        public bool EliminarModeloCorreo(int idCorreo)
        {
            var correo = Context.CorreoElectronico.Find(idCorreo);

            if (correo == null)
                return false;

            Context.CorreoElectronico.Remove(correo);
            Context.SaveChanges();
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
        /// Alta de un nuevo correo electrónico
        /// </summary>
        /// <param name="correoElectronico"></param>
        /// <returns></returns>
        //public bool GuardarCorreoNuevo(CorreosElectronicos correoElectronico)
        //{
        //    using (var trans = Context.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            Context.Add(correoElectronico);
        //            Context.SaveChanges();
        //            trans.Commit();
        //        }
        //        catch (ValidacionException ex)
        //        {
        //            ex.Message.ToString();
        //            trans.Rollback();
        //        }
        //    }
        //    return true;
        //}
        #endregion
    }
}
