using Negocio.Persistencia.Modelos;

namespace Negocio.Servicios
{
    public interface IGestionUsuarios : IServicioEpsilon
    {
        /// <summary> Obtener un listado de usuarios. </summary>
        //IQueryable <Usuario> GetAllUsers();

        /// <summary> Obtener un usuario. </summary>
        Usuario GetUser(long idUsuario);

        /// <summary> Agregar un usuario. </summary>
        Usuario AddUser(Usuario usuario);

        Usuario UpdateUser(Usuario usuario);

        bool DeleteUser(int idUsuario);

        IQueryable<DatosUsuario> GetDatosUsuario();
    }
}
