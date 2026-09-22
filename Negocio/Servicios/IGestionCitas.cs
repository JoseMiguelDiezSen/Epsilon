using Negocio.Persistencia.Modelos;

namespace Negocio.Servicios
{
    /// <summary>
    /// Contrato para la gestión y operaciones con citas médicas en la agenda.
    /// </summary>
    public interface IGestionCitas : IServicioEpsilon
    {
        /// <summary>
        /// Obtiene todas las citas registradas en el sistema.
        /// </summary>
        IQueryable<Citas> GetCitas();

        /// <summary>
        /// Inserta una nueva cita médica y opcionalmente vincula el tratamiento asignado.
        /// </summary>
        /// <param name="cita">Entidad con los datos de la cita</param>
        /// <param name="idTratamiento">Identificador opcional del tratamiento</param>
        Citas AddCita(Citas cita, int? idTratamiento);

        /// <summary>
        /// Actualiza únicamente las fechas de una cita existente (usado en drop/resize de FullCalendar).
        /// </summary>
        /// <param name="idCita">Identificador de la cita</param>
        /// <param name="fechaInicio">Nueva fecha y hora de inicio</param>
        /// <param name="fechaFin">Nueva fecha y hora de fin</param>
        bool UpdateFechaCita(int idCita, DateTime fechaInicio, DateTime fechaFin);

        /// <summary>
        /// Elimina una cita médica por su identificador.
        /// </summary>
        /// <param name="idCita">Identificador de la cita a eliminar</param>
        bool DeleteCita(int idCita);

        /// <summary>
        /// Obtiene el catálogo de tratamientos disponibles.
        /// </summary>
        List<Tratamiento> GetTratamientos();

        /// <summary>
        /// Obtiene el catálogo de pacientes registrados.
        /// </summary>
        List<Paciente> GetPacientes();

        /// <summary>
        /// Obtiene el catálogo de médicos registrados.
        /// </summary>
        List<Medico> GetMedicos();

        /// <summary>
        /// Obtiene el catálogo de clínicas registradas.
        /// </summary>
        List<Clinica> GetClinicas();

        /// <summary>
        /// Obtiene el tratamiento vinculado a una cita, si existe.
        /// </summary>
        Tratamiento? GetTratamientoCita(int idCita);

        /// <summary>
        /// Obtiene una cita médica por su identificador.
        /// </summary>
        /// <param name="idCita">Identificador de la cita</param>
        Citas? GetCita(int idCita);

        /// <summary>
        /// Actualiza los datos de una cita médica existente y su tratamiento asociado.
        /// </summary>
        /// <param name="cita">Entidad con los datos modificados de la cita</param>
        /// <param name="idTratamiento">Identificador opcional del tratamiento</param>
        bool UpdateCita(Citas cita, int? idTratamiento);
    }
}
