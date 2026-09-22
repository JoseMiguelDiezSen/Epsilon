using Microsoft.Extensions.Logging;
using Negocio.Persistencia;
using Negocio.Persistencia.Modelos;
using Negocio.Servicios.Comun;
using Negocio.Validadores.Comun;
using System;
using System.Linq;
using System.Reflection;

namespace Negocio.Servicios
{
    public class GestionMedicos : ServicioAbstractoEpsilon, IGestionMedicos
    {
        /// <summary>
        /// Constructor del servicio
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        /// <param name="registroValidadores"></param>
        public GestionMedicos(EpsilonDbContext context, ILogger<GestionMedicos> logger, IValidadoresProgesfor registroValidadores) : base(context, logger, registroValidadores)
        {
            logger.LogTrace(GetEventId(), "Servicion iniciado");
        }

        /// <summary>
        /// Obtiene todos los médicos.
        /// </summary>
        /// <returns></returns>
        public IQueryable<Medico> GetAllMedicos()
        {
            logger.LogTrace(GetEventId(), MethodBase.GetCurrentMethod()?.Name);
            return Context.Medicos;
        }

        /// <summary>
        /// Obtiene los datos de vista de los médicos.
        /// </summary>
        /// <returns></returns>
        public IQueryable<DatosMedicos> GetDatosMedicos()
        {
            logger.LogTrace(GetEventId(), MethodBase.GetCurrentMethod()?.Name);
            return Context.DatosMedicos;
        }

        /// <summary>
        /// Añade un nuevo médico.
        /// </summary>
        /// <param name="medico">Médico a añadir</param>
        /// <returns></returns>
        public Medico AddMedico(Medico medico)
        {
            using (var transaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.Medicos.Add(medico);
                    Context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    logger.LogError(GetEventId(), ex, "Error al insertar médico: {Message}", ex.Message);
                    transaction.Rollback();
                    throw;
                }
            }
            return medico;
        }

        /// <summary>
        /// Actualiza un médico existente.
        /// </summary>
        /// <param name="medico">Médico a actualizar</param>
        /// <returns></returns>
        public Medico UpdateMedico(Medico medico)
        {
            var entity = Context.Medicos.Find(medico.IdMedico);
            if (entity == null)
            {
                throw new Exception($"Médico con ID {medico.IdMedico} no encontrado.");
            }

            entity.NombreMedico = medico.NombreMedico;
            entity.DNI = medico.DNI;
            entity.NumeroColegiado = medico.NumeroColegiado;
            entity.Especialidad = medico.Especialidad;
            entity.Telefono = medico.Telefono;
            entity.EMail = medico.EMail;
            entity.FechaContratacion = medico.FechaContratacion;
            entity.Activo = medico.Activo;
            entity.Observaciones = medico.Observaciones;
            entity.IdClinica = medico.IdClinica;

            if (medico.Foto != null && medico.Foto.Length > 0)
            {
                entity.Foto = medico.Foto;
            }

            Context.SaveChanges();
            return entity;
        }

        /// <summary>
        /// Elimina un médico por su ID.
        /// </summary>
        /// <param name="idMedico">Identificador del médico a eliminar</param>
        /// <returns></returns>
        public bool DeleteMedico(int idMedico)
        {
            var medico = Context.Medicos.FirstOrDefault(u => u.IdMedico == idMedico);
            if (medico == null)
            {
                return false;
            }

            // Eliminar dependencias si las hubiera para evitar conflictos de clave foránea
            var agendas = Context.Agenda.Where(a => a.IdMedico == idMedico).ToList();
            if (agendas.Any())
            {
                Context.Agenda.RemoveRange(agendas);
            }

            var citas = Context.Citas.Where(c => c.IdMedico == idMedico).ToList();
            if (citas.Any())
            {
                Context.Citas.RemoveRange(citas);
            }

            Context.Medicos.Remove(medico);
            Context.SaveChanges();
            return true;
        }

        /// <summary>
        /// Obtiene un médico por su ID.
        /// </summary>
        /// <param name="idMedico">Identificador del médico</param>
        /// <returns></returns>
        public Medico? GetMedico(int idMedico)
        {
            return Context.Medicos.Find(idMedico);
        }

        /// <summary>
        /// Obtiene los datos detallados de un médico por su ID.
        /// </summary>
        /// <param name="idMedico">Identificador del médico</param>
        /// <returns></returns>
        public DatosMedicos? GetDetalleMedico(int idMedico)
        {
            return Context.DatosMedicos.FirstOrDefault(m => m.IdMedico == idMedico);
        }
    }
}
