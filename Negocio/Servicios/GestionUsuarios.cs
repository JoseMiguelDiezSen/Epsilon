using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Negocio.Persistencia;
using Negocio.Persistencia.Modelos;
using Negocio.Servicios.Comun;
using Negocio.Validadores.Comun;
using System.Reflection;

namespace Negocio.Servicios
{
    public class GestionUsuarios : ServicioAbstractoEpsilon, IGestionUsuarios
    {
        private readonly EpsilonDbContext _context;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="context"></param>
        public GestionUsuarios(EpsilonDbContext context, ILogger<GestionUsuarios> logger, IValidadoresProgesfor registroValidadores) : base(context, logger, registroValidadores)
        {
            _registroValidadores = registroValidadores;
            _context = context;
            logger.LogTrace(GetEventId(), "Servicion iniciado");
        }

        #region Usuarios

        // Todos los usuarios
        public IEnumerable<Usuario> GetAllUsers()
        {
            // Opcion 1
            var usuarios = Context.Usuarios.ToList().OrderByDescending(u => u.Nombre);
            return usuarios;
        }


        // Todos loa usuarios que cumplan una condicion 
        public IEnumerable<Usuario> GetUsersOf2024(DateTime anio)
        {
            var usuarios = Context.Usuarios.Where(u => u.FechaAlta == anio).ToList();
            return usuarios;
        }

        // Todos loa usuarios que cumplan dos condiciones 
        public IEnumerable<Usuario> GetUsersOf2024WithPassword(DateTime anio)
        {
            var usuarios = Context.Usuarios.Where(u => u.FechaAlta == anio && u.Password != null).ToList();
            return usuarios;
        }

        public Usuario AddUser(Usuario usuario)
        {
            ValidaEntidad(usuario, OperacionesValidacion.OPERACION_INSERTAR);

            try
            {
                Context.Usuarios.Add(usuario);
                Context.SaveChanges();

                return usuario;
            }
            catch (Exception ex)
            {
                // Aquí log real si tienes logger
                throw new Exception($"Error al insertar usuario: {ex.Message}", ex);
            }
        }

        //public Usuario AddUser(Usuario usuario)
        //{
        //    ValidaEntidad(usuario, OperacionesValidacion.OPERACION_INSERTAR);

        //    using (var trans = Context.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            Context.Add(usuario);
        //            Context.SaveChanges();
        //            trans.Commit();
        //        }
        //        catch (ValidacionException ex)
        //        {
        //            ex.Message.ToString();
        //            trans.Rollback();
        //        }
        //    }
        //    return (usuario);
        //}

        //public Usuario UpdateUser(Usuario usuario)
        //{
        //    var entity = Context.Usuarios.Update(usuario);
        //    Context.SaveChanges();
        //    return entity.Entity;
        //}

        public Usuario UpdateUser(Usuario usuario)
        {
            var entity = Context.Usuarios.Find(usuario.IdUsuario);

            entity.Nombre = usuario.Nombre;
            entity.Password = usuario.Password;
            entity.Email = usuario.Email;
            entity.Telefono = usuario.Telefono;
            entity.Activo = usuario.Activo;

            if (usuario.FotoPerfil != null)
                entity.FotoPerfil = usuario.FotoPerfil;

            //Context.Update(usuario);
            Context.SaveChanges();
            return entity;
        }


        public Usuario GetUser(long idUsuario)
        {
            // Opcion 1
            var usuario = Context.Usuarios.Where(u => u.IdUsuario == idUsuario).First();
            return usuario;

            //Opcion 2
            //return Context.Usuarios.Single(u => u.IdUsuario == idUsuario);
        }

        public bool DeleteUser(int idUsuario)
        {
            //Opcion 1 con Single
            var usuario = Context.Usuarios.Single(u => u.IdUsuario == idUsuario);

            // Opcion 2 con Where y First()
            //var usuario1 = Context.Usuarios.Where(u => u.IdUsuario == idUsuario).First();
            Context.Usuarios.Remove(usuario);
            return true;
        }

        /// <summary>
        /// Metodo para obtener los datos de un usuario, se utiliza para mostrar la informacion del usuario en la vista de perfil
        /// </summary>
        /// <returns></returns>
        public IQueryable<DatosUsuario> GetDatosUsuario()
        {
            logger.LogTrace(GetEventId(), MethodBase.GetCurrentMethod()?.Name);
            return Context.DatosUsuarios;
        }

        #endregion

        #region Procedimientos Almacenados

        // Procedimiento almacenado para agregar un usuario
        public void AgregarUsuario(Usuario usuario)
        {
            _context.Database.ExecuteSqlRaw(
                "EXEC SP_AgregarUsuario @Nombre, @Password, @Email, @FechaAlta, @Telefono, @Activo, @FotoPerfil",
                new SqlParameter("@Nombre", usuario.Nombre),
                new SqlParameter("@Password", usuario.Password),
                new SqlParameter("@Email", usuario.Email),
                new SqlParameter("@FechaAlta", usuario.FechaAlta),
                new SqlParameter("@Telefono", usuario.Telefono),
                new SqlParameter("@Activo", usuario.Activo),
                new SqlParameter("@FotoPerfil", (object?)usuario.FotoPerfil ?? DBNull.Value)
            );
        }

        // Procedimiento almacenado para modificar un usuario
        public void ModificarUsuario(Usuario usuario)
        {
            _context.Database.ExecuteSqlRaw(
                "EXEC SP_ModificarUsuario @IdUsuario, @Nombre, @Password, @Email, @Telefono, @Activo, @FotoPerfil",
                new SqlParameter("@IdUsuario", usuario.IdUsuario),
                new SqlParameter("@Nombre", usuario.Nombre),
                new SqlParameter("@Password", usuario.Password),
                new SqlParameter("@Email", usuario.Email),
                new SqlParameter("@Telefono", usuario.Telefono),
                new SqlParameter("@Activo", usuario.Activo),
                new SqlParameter("@FotoPerfil", (object?)usuario.FotoPerfil ?? DBNull.Value)
            );
        }

        // Procedimiento almacenado para eliminar un usuario
        public void EliminarUsuario(int idUsuario)
        {
            _context.Database.ExecuteSqlRaw(
                "EXEC SP_EliminarUsuario @IdUsuario",
                new SqlParameter("@IdUsuario", idUsuario)
            );
        }

        #endregion
    }
}
