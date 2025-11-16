using Negocio.Persistencia.Modelos;

namespace Negocio.Servicios
{
    public interface IConfiguracion : IServicioEpsilon
    {
        public bool GuardarCorreoNuevo(CorreosElectronicos correosElectronicos);

        public bool ActualizarDatosCorreo(CorreosElectronicos correosElectronicos);
    }
}
