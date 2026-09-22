using Negocio.Persistencia.Modelos;
using Negocio.Persistencia;
using System.Collections.Generic;

namespace Negocio.Servicios
{
    public interface IGestionCitas : IServicioEpsilon
    {
        List<Citas> ObtenerTodas();
        Citas? Get(int id);
        Citas Add(Citas entity);
        bool Update(Citas entity);
        bool Delete(int id);

        List<Servicio> GetServicios();
        List<Cliente> GetClientes();
        List<Personal> GetPersonal();
        List<Sede> GetSedes();

        Servicio? GetServicioCita(int idCita);
        CitaServicio AddCitaServicio(CitaServicio entity);
        bool UpdateCitaServicio(CitaServicio entity);
    }
}
