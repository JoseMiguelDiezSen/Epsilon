using Negocio.Excepciones;
using Negocio.Persistencia.Modelos;

namespace Negocio.Servicios
{
    public interface IGestionPacientes : IServicioEpsilon
    {
        IQueryable<Paciente> GetAllPacientes();

        /// <summary> Agregar un paciente </summary>
        Paciente AddPaciente(Paciente paciente);

        public IQueryable<DatosPacientes> GetDatosPacientes();

        Paciente UpdatePaciente(Paciente paciente);

        bool DeletePaciente(int idPaciente);

        public DatosPacientes? GetDetallePaciente(int idPaciente);

        public bool ActualizarDatosCorreo(CorreosElectronicos correosElectronicos);

        public bool EliminarModeloCorreo(int idCorreo);

        public bool AddModeloCorreo(CorreosElectronicos correoElectronico);
    }
}
