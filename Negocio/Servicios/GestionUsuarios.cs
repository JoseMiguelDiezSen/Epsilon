using Negocio.Persistencia;
using Negocio.Persistencia.Modelos;
using Negocio.Servicios.Comun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Negocio.Servicios
{
    public class GestionUsuarios : ServicioAbstractoEpsilon, IGestionUsuarios
    {
        protected ISeguridad _seguridad;


        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="context"></param>
        public GestionUsuarios(EpsilonDbContext context, ILogger<GestionUsuarios> logger, ISeguridad seguridad) : base(context, logger)
        {

            //_claimsPrincipal = claimsPrincipal;
            _seguridad = seguridad;
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
            using (var trans = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.Add(usuario);
                    Context.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                    trans.Rollback();
                }
            }
            return (usuario);
        }

        public Usuario UpdateUser(Usuario usuario)
        {
            var usuarioActualizado = Context.Usuarios.Single(u => u.IdUsuario == usuario.IdUsuario);
            var entity = Context.Usuarios.Update(usuarioActualizado);
            return usuarioActualizado;
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
            var usuario1 = Context.Usuarios.Where(u => u.IdUsuario == idUsuario).First();
            Context.Usuarios.Remove(usuario);
            return true;
        }

        /// <summary>
        /// Metodo para obtener los periodos de un area determinada
        /// </summary>
        /// <returns></returns>
        public IQueryable<DatosUsuario> GetDatosUsuario()
        {
            logger.LogTrace(GetEventId(), MethodBase.GetCurrentMethod()?.Name);
            return Context.DatosUsuarios;
        }

        #endregion
    }
}
