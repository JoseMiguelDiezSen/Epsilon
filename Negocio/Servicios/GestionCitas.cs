using Microsoft.Extensions.Logging;
using Negocio.Persistencia;
using Negocio.Persistencia.Modelos;
using Negocio.Servicios.Comun;
using Negocio.Validadores.Comun;

namespace Negocio.Servicios
{
    /// <summary>
    /// Servicio que gestiona las operaciones de citas médicas y eventos de la agenda.
    /// </summary>
    public class GestionCitas : ServicioAbstractoEpsilon, IGestionCitas
    {
        public GestionCitas(EpsilonDbContext context, ILogger<GestionCitas> logger, IValidadoresProgesfor registroValidadores)
            : base(context, logger, registroValidadores)
        {
            logger.LogTrace(GetEventId(), "Servicio GestionCitas iniciado");
        }

        /// <summary>
        /// Obtiene la lista completa de citas.
        /// </summary>
        public IQueryable<Citas> GetCitas()
        {
            return Context.Citas.AsQueryable();
        }

        /// <summary>
        /// Crea una nueva cita en la base de datos y le asocia su tratamiento si se ha seleccionado.
        /// </summary>
        public Citas AddCita(Citas cita, int? idTratamiento)
        {
            // Sincronizamos la clínica con la clínica asignada al médico para garantizar consistencia con vDatosHistorialPaciente
            var medicoAsignado = Context.Medicos.FirstOrDefault(m => m.IdMedico == cita.IdMedico);
            if (medicoAsignado?.IdClinica != null && medicoAsignado.IdClinica.Value > 0)
            {
                cita.IdClinica = medicoAsignado.IdClinica.Value;
            }
            else if (cita.IdClinica <= 0 || !Context.Clinicas.Any(c => c.IdClinica == cita.IdClinica))
            {
                cita.IdClinica = Context.Clinicas.Select(c => c.IdClinica).FirstOrDefault();
            }

            // Guardamos la cita en la tabla Citas
            Context.Citas.Add(cita);
            Context.SaveChanges();

            // Vinculamos la cita con el tratamiento en CitaTratamientos (si no se especifica, se asigna el primero por defecto para que aparezca en vDatosHistorialPaciente)
            int tratamientoIdFinal = (idTratamiento.HasValue && idTratamiento.Value > 0)
                ? idTratamiento.Value
                : Context.Tratamientos.OrderBy(t => t.IdTratamiento).Select(t => t.IdTratamiento).FirstOrDefault();

            if (tratamientoIdFinal > 0)
            {
                CitaTratamiento relacion = new CitaTratamiento
                {
                    IdCita = cita.IdCita,
                    IdTratamiento = tratamientoIdFinal
                };
                Context.CitaTratamientos.Add(relacion);
                Context.SaveChanges();
            }

            // Registramos el hueco en la tabla Agenda marcando Disponible = false (0 = ocupado)
            Agenda entradaAgenda = new Agenda
            {
                IdMedico = cita.IdMedico,
                HoraInicio = cita.FechaInicio,
                HoraFin = cita.FechaFin,
                Disponible = false, // 0 = no disponible / ocupado
                IdCita = cita.IdCita
            };
            Context.Agenda.Add(entradaAgenda);
            Context.SaveChanges();

            return cita;
        }

        /// <summary>
        /// Actualiza la fecha y hora de una cita existente (usado al arrastrar o redimensionar en FullCalendar).
        /// Actualiza tanto la tabla Citas como el intervalo en la tabla Agenda.
        /// </summary>
        public bool UpdateFechaCita(int idCita, DateTime fechaInicio, DateTime fechaFin)
        {
            var cita = Context.Citas.FirstOrDefault(c => c.IdCita == idCita);
            if (cita == null)
            {
                return false;
            }

            cita.FechaInicio = fechaInicio;
            cita.FechaFin = fechaFin;

            // Actualizamos también el intervalo en la tabla Agenda
            var entradaAgenda = Context.Agenda.FirstOrDefault(a => a.IdCita == idCita);
            if (entradaAgenda != null)
            {
                entradaAgenda.HoraInicio = fechaInicio;
                entradaAgenda.HoraFin = fechaFin;
                entradaAgenda.IdMedico = cita.IdMedico;
                entradaAgenda.Disponible = false;
            }
            else
            {
                // Si no existía registro previo en Agenda, lo creamos
                Context.Agenda.Add(new Agenda
                {
                    IdMedico = cita.IdMedico,
                    HoraInicio = fechaInicio,
                    HoraFin = fechaFin,
                    Disponible = false,
                    IdCita = idCita
                });
            }

            Context.SaveChanges();

            return true;
        }

        /// <summary>
        /// Elimina una cita médica y sus registros vinculados en CitaTratamientos y Agenda.
        /// </summary>
        public bool DeleteCita(int idCita)
        {
            var cita = Context.Citas.FirstOrDefault(c => c.IdCita == idCita);
            if (cita == null)
            {
                return false;
            }

            // Eliminamos las relaciones en CitaTratamientos si las hubiera
            var tratamientosAsociados = Context.CitaTratamientos.Where(ct => ct.IdCita == idCita).ToList();
            if (tratamientosAsociados.Any())
            {
                Context.CitaTratamientos.RemoveRange(tratamientosAsociados);
            }

            // Eliminamos los registros asociados en la tabla Agenda
            var entradasAgenda = Context.Agenda.Where(a => a.IdCita == idCita).ToList();
            if (entradasAgenda.Any())
            {
                Context.Agenda.RemoveRange(entradasAgenda);
            }

            // Eliminamos la cita
            Context.Citas.Remove(cita);
            Context.SaveChanges();

            return true;
        }

        /// <summary>
        /// Devuelve el listado de tratamientos disponibles en la base de datos.
        /// </summary>
        public List<Tratamiento> GetTratamientos()
        {
            return Context.Tratamientos.OrderBy(t => t.NombreTratamiento).ToList();
        }

        /// <summary>
        /// Devuelve el listado de pacientes registrados.
        /// </summary>
        public List<Paciente> GetPacientes()
        {
            return Context.Pacientes.OrderBy(p => p.NombrePaciente).ToList();
        }

        /// <summary>
        /// Devuelve el listado de médicos activos.
        /// </summary>
        public List<Medico> GetMedicos()
        {
            return Context.Medicos.Where(m => m.Activo).OrderBy(m => m.NombreMedico).ToList();
        }

        /// <summary>
        /// Devuelve el listado de clínicas registradas.
        /// </summary>
        public List<Clinica> GetClinicas()
        {
            return Context.Clinicas.OrderBy(c => c.NombreClinica).ToList();
        }

        /// <summary>
        /// Obtiene el tratamiento asignado a una cita concreta.
        /// </summary>
        public Tratamiento? GetTratamientoCita(int idCita)
        {
            var relacion = Context.CitaTratamientos.FirstOrDefault(ct => ct.IdCita == idCita);
            if (relacion != null && relacion.IdTratamiento.HasValue)
            {
                return Context.Tratamientos.FirstOrDefault(t => t.IdTratamiento == relacion.IdTratamiento.Value);
            }
            return null;
        }

        /// <summary>
        /// Obtiene una cita médica por su identificador.
        /// </summary>
        public Citas? GetCita(int idCita)
        {
            return Context.Citas.FirstOrDefault(c => c.IdCita == idCita);
        }

        /// <summary>
        /// Actualiza los datos de una cita médica existente y sincroniza su tratamiento y el registro en la Agenda.
        /// </summary>
        public bool UpdateCita(Citas citaModificada, int? idTratamiento)
        {
            var citaExistente = Context.Citas.FirstOrDefault(c => c.IdCita == citaModificada.IdCita);
            if (citaExistente == null)
            {
                return false;
            }

            // Sincronizamos la clínica con la del médico asignado para evitar inconsistencias
            var medicoAsignado = Context.Medicos.FirstOrDefault(m => m.IdMedico == citaModificada.IdMedico);
            if (medicoAsignado?.IdClinica != null && medicoAsignado.IdClinica.Value > 0)
            {
                citaExistente.IdClinica = medicoAsignado.IdClinica.Value;
            }

            // Actualizamos los campos de la cita
            citaExistente.IdPaciente = citaModificada.IdPaciente;
            citaExistente.IdMedico = citaModificada.IdMedico;
            citaExistente.FechaInicio = citaModificada.FechaInicio;
            citaExistente.FechaFin = citaModificada.FechaFin;
            citaExistente.Observaciones = citaModificada.Observaciones;

            // Actualizamos la relación con el tratamiento en CitaTratamientos
            var relacionTratamiento = Context.CitaTratamientos.FirstOrDefault(ct => ct.IdCita == citaModificada.IdCita);
            int tratamientoModifId = (idTratamiento.HasValue && idTratamiento.Value > 0)
                ? idTratamiento.Value
                : (relacionTratamiento?.IdTratamiento ?? Context.Tratamientos.OrderBy(t => t.IdTratamiento).Select(t => t.IdTratamiento).FirstOrDefault());

            if (tratamientoModifId > 0)
            {
                if (relacionTratamiento != null)
                {
                    relacionTratamiento.IdTratamiento = tratamientoModifId;
                }
                else
                {
                    Context.CitaTratamientos.Add(new CitaTratamiento
                    {
                        IdCita = citaModificada.IdCita,
                        IdTratamiento = tratamientoModifId
                    });
                }
            }

            // Sincronizamos con el registro correspondiente en la tabla Agenda
            var entradaAgenda = Context.Agenda.FirstOrDefault(a => a.IdCita == citaModificada.IdCita);
            if (entradaAgenda != null)
            {
                entradaAgenda.IdMedico = citaModificada.IdMedico;
                entradaAgenda.HoraInicio = citaModificada.FechaInicio;
                entradaAgenda.HoraFin = citaModificada.FechaFin;
                entradaAgenda.Disponible = false;
            }
            else
            {
                Context.Agenda.Add(new Agenda
                {
                    IdMedico = citaModificada.IdMedico,
                    HoraInicio = citaModificada.FechaInicio,
                    HoraFin = citaModificada.FechaFin,
                    Disponible = false,
                    IdCita = citaModificada.IdCita
                });
            }

            Context.SaveChanges();
            return true;
        }
    }
}
