using Negocio.Persistencia.Modelos;

namespace Negocio.Servicios
{
    public interface IGestionUsuarios : IServicioEpsilon
    {
        /// <summary> 
        /// Obtener un usuario. 
        /// </summary>
        /// <param name="idUsuario"> El ID del usuario a obtener.</param>
        /// <returns>El usuario obtenido.</returns>
        Usuario GetUser(long idUsuario);

        /// <summary>
        /// Agregar un nuevo usuario.
        /// </summary>
        /// <param name="usuario">El usuario a agregar.</param>
        /// <returns>El usuario agregado.</returns>
        Usuario AddUser(Usuario usuario);

        /// <summary>
        /// Actualizar un usuario existente.
        /// </summary>
        /// <param name="usuario">El usuario a actualizar.</param>
        /// <returns>El usuario actualizado.</returns>
        public Usuario UpdateUser(Usuario usuario);

        /// <summary>
        /// Eliminar un usuario existente.
        /// </summary>
        /// <param name="idUsuario">El ID del usuario a eliminar.</param>
        /// <returns>True si el usuario fue eliminado, false en caso contrario.</returns>
        public bool DeleteUser(int idUsuario);

        /// <summary>
        /// Obtener los datos de un usuario, se utiliza para mostrar la información del usuario en la vista de perfil.
        /// </summary>
        /// <returns>Una consulta con los datos del usuario.</returns>
        IQueryable<DatosUsuario> GetDatosUsuario();


        /// <summary> Obtener un listado de usuarios. </summary>
        //IQueryable <Usuario> GetAllUsers();
    }
}
