using Negocio.Excepciones;
using Negocio.Persistencia.Modelos;

namespace Negocio.Servicios
{
    public interface IGestionPacientes : IServicioEpsilon
    {
        /// <summary>
        /// Obtiene todos los pacientes
        /// </summary>
        /// <returns></returns>
        IQueryable<Paciente> GetAllPacientes();

        /// <summary>
        /// Obtiene los datos de los pacientes
        /// </summary>
        /// <returns></returns>
        public IQueryable<DatosPacientes> GetDatosPacientes();

        /// <summary>
        /// Añade un nuevo paciente.
        /// </summary>
        /// <param name="paciente"></param>
        /// <returns></returns>
        Paciente AddPaciente(Paciente paciente);

        /// <summary>
        /// Modifica un paciente existente.
        /// </summary
        /// <param name="paciente"></param>
        /// <returns></returns>
        Paciente UpdatePaciente(Paciente paciente);

        /// <summary>
        /// Elimina un paciente por su ID.
        /// </summary>
        /// <param name="idPaciente"> Identificador del paciente a eliminar </param>
        /// <returns></returns>
        bool DeletePaciente(int idPaciente);

        /// <summary>
        /// Obtiene los datos de detalle de un paciente por su ID.
        /// </summary>
        /// <param name="idPaciente"> Identificador del paciente </param>
        /// <returns></returns>
        public DatosPacientes? GetDetallePaciente(int idPaciente);

        /// <summary>
        /// Elimina un modelo de correo electronico por su id
        /// </summary>
        /// <param name="idCorreo"></param>
        /// <returns></returns>
        public bool EliminarModeloCorreo(int idCorreo);

        /// <summary>
        /// Añadir un modelo de correo electrónico para notificaciones relacionadas con pacientes.
        /// </summary>
        /// <param name="correoElectronico"> Modelo de correo electrónico a añadir </param>
        /// <returns></returns>
        public bool AddModeloCorreo(CorreosElectronicos correoElectronico);
    }
}
