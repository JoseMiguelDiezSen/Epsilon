using Negocio.Persistencia.Modelos;

namespace Negocio.Servicios
{
    public interface IGestionPacientes: IServicioEpsilon
    {
        IQueryable <Paciente> GetAllPacientes();

        /// <summary> Agregar un paciente </summary>
        Paciente AddPaciente(Paciente paciente);

        Paciente UpdatePaciente(Paciente paciente);

        bool DeletePaciente(int idPaciente);
    }
}
