using Negocio.Persistencia.Modelos;

namespace Epsilon.ViewModels

{
    public class ViewUsuario
    {
        public ViewUsuario() { }

        public ViewUsuario(DatosUsuario datosUsuario)
        {
            IdUsuario = datosUsuario.IdUsuario;
            Nombre = datosUsuario.Nombre;
            Password = datosUsuario.Password;
            Email = datosUsuario.EMail;
            FechaAlta = datosUsuario.FechaAlta;
            Telefono = datosUsuario.Telefono;
            RutaFoto = datosUsuario?.RutaFoto;
        }

        public int IdUsuario { get; set; }

        public string? Nombre { get; set; }

        public string? Password { get; set; }

        public string? Email { get; set; } = null;

        public DateTime FechaAlta { get; set; }

        public int Telefono { get; set; }

        public string? RutaFoto { get; set; }
    }
}
