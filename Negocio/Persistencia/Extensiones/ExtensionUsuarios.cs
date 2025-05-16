using Negocio.Persistencia.Modelos;
using Negocio.Persistencia.Modelos.Comun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Persistencia.Extensiones
{
    public class ExtensionUsuarios : ExtensionAbstracta<EpsilonDbContext>
    {
        public ExtensionUsuarios(EpsilonDbContext context) : base (context) { }

        //public Usuario AddSecured(Usuario usuario) { 
        
        //    Context.Usuarios.Add(usuario);

        //    EntidadSecurizada es = new EntidadSecurizada()
        //    {
        //        IdEntidadReferenciada = usuario.IdUsuario,
        //        IdTipo = usuario.IdMiTipo
        //    };
        //    Context.EntidadesSecurizadas.Add(es);

        //    Rol? rolAdmin = Context.Roles.SingleOrDefault(r => r.Prelacion == 255 && r.IdArea == 0);
        //    if (rolAdmin != null) {

        //        Context.PermisoRolEntidad.Add(new PermisoRolEntidad()
        //        {
        //            IdEntidadSecurizada = es.IdEntidadSecurizada,
        //            IdRol = rolAdmin.IdRol,
        //            ConsultaConcedida = true,
        //            ModificacionConcedida = true,
        //            EliminacionConcedida=true,
        //            EjecucionConcedida = true,
        //        });
        //    }
        //    return usuario;
        //}

        //public bool Exists(string usuario) { 
        //    int n = Context.Usuarios.Count(u=> u.Equals(u.Nombre));
        //    return n > 0;
        //}

        //public bool IsActive(string usuario) {
        //    int n = Context.Usuarios.Count(u => usuario.Equals(u.Nombre) && u.Activo == true);
        //    return n > 0;
        //}

        //public EntidadSecurizada? GrtEntidadSecurizada(DatosUsuario usu) {
        //    return Context.EntidadesSecurizadas.SingleOrDefault(e => e.IdEntidadReferenciada == usu.IdUsuario && e.IdTipo == usu.IdMiTipo);
        //}

        //public EntidadSecurizada? GetEntidadSecurizada(Usuario usu) {
        //    return Context.EntidadesSecurizadas.SingleOrDefault(e => e.IdEntidadReferenciada == usu.IdUsuario && e.IdTipo == usu.IdMiTipo);
        //}

        //public EntidadSecurizada? GetEntidadSecurizada(long idEntidadReferenciada, int idTipo) {
        //    return Context.EntidadesSecurizadas.SingleOrDefault(e => e.IdEntidadReferenciada == idEntidadReferenciada && e.IdTipo == idTipo);
        //}

        //public Usuario? GetById(long idUsuario) {
        //    return Context.Usuarios.SingleOrDefault(u => u.IdUsuario == idUsuario);
        //}

        //public Usuario? GetByIDP(long IDP) {
        //    return Context.Usuarios.SingleOrDefault(u => u.IDP == IDP);
        //}

        //public DatosUsuario? GetDatosUsuarioById(long idUsuario) {
        //    return Context.DatosUsuarios.SingleOrDefault(u => u.IdUsuario == idUsuario);
        //}

        //public DatosUsuario? GetDatosUsuarioByIDP(long IDP) {
        //    return Context.DatosUsuarios.SingleOrDefault(u => u.IDP == IDP);
        //}

        //public IQueryable <Rol> GetRoles(Usuario usu) {
        //    IQueryable<Rol> roles = (from ur in Context.UsuariosRoles join r in Context.Roles on ur.IdRol equals r.IdRol where ur.IDP == usu.IDP select r);
        //    return roles;
        //}

        //public IQueryable <Rol> GetRolesByIDP(long IDP) {
        //    IQueryable<Rol> roles = (from ur in Context.UsuariosRoles join r in Context.Roles on ur.IdRol equals r.IdRol where ur.IDP == IDP select r);
        //    return roles;
        //}

        //public IQueryable <Usuario> GetUsuarios(Usuario? user)
        //{
        //    if (user == null) { 
        //        throw new ArgumentNullException(nameof(user));
        //    }
        //    return GetUsuarios(user.IDP);
        //}

        //public IQueryable<Usuario> GetUsuarios(int IDP) { 
        
        //    UsuarioRol? usuRol = Context.UsuariosRoles.Where(ur => ur.IDP == IDP && ur.EsPrincipal == true).FirstOrDefault();
        //    if (usuRol == null) {
        //        throw new InvalidDataException("El usuario indicado no tiene un rol principal");
        //    }

        //    Rol mainRol = Context.Roles.Single(r => r.IdRol == usuRol.IdRol);

        //    return Context.Usuarios
        //        .Join(Context.UsuariosRoles, d => new { d.IDP, EsPrincipal = true }, ur => new { ur.IDP, ur.EsPrincipal }, (d, ur) => new { d, ur })
        //        .Join(Context.Roles, x => x.ur.IdRol, r => r.IdRol, (x, r) => new { x.d, r })
        //        .Where(x => x.d.IdArea == mainRol.IdArea && x.r.Prelacion < mainRol.Prelacion).Select(x => x.d);
        //}

        //public IQueryable<Usuario> GetUsuariosWithReadPermissions(long IDP) {
        //    return from u in Context.Usuarios
        //           join es in Context.EntidadesSecurizadas on new { id = u.IdUsuario, tipo = u.IdMiTipo } equals new { id = es.IdEntidadReferenciada, tipo = es.IdTipo } into uyes
        //           from ues in uyes.DefaultIfEmpty()
        //           where Context.HayPermisoConsulta(IDP, ues.IdEntidadSecurizada)
        //           select u;
        //    }

        //public bool AddRol(Usuario usu, Rol rol) {
        //    if (usu == null || rol == null) return false;
        //    UsuarioRol usuarioRol = new UsuarioRol();
        //    usuarioRol.IDP = usu.IDP;
        //    usuarioRol.IdRol = rol.IdRol;
        //    Context.UsuariosRoles.Add(usuarioRol);
        //    return true;
        //}

        //public bool RemoveRol(Usuario usu, Rol rol) {
        //    if (usu == null || rol == null) return false;
        //    UsuarioRol? usuRol = Context.UsuariosRoles.SingleOrDefault(ur => ur.IDP == usu.IDP && ur.IdRol == rol.IdRol);
        //    if(usuRol == null) return false;
        //    return true;
        //}

        //public Rol GetMainRol(Usuario usu) {

        //    if (usu == null) { 
        //        throw new ArgumentNullException(nameof(usu)); 
        //    }
        //    return GetMainRol(usu.IDP);
        //}

        //public Rol GetMainRol(int IDP) {

        //    UsuarioRol? usuRol = Context.UsuariosRoles.SingleOrDefault(ur => ur.IDP == IDP && ur.EsPrincipal);
        //    if (usuRol == null) {
        //        throw new InvalidDataException("El usuario indicado no tiene un rol principal.");
        //    }
        //    return Context.Roles.Single(r => r.IdRol == usuRol.IdRol);
        //}

        //public bool SetMainRol(Usuario usu, Rol rol) {

        //    if (usu == null || rol == null) return false;
        //    UsuarioRol? usuRol = Context.UsuariosRoles.SingleOrDefault(ur => ur.IDP == usu.IDP && ur.IdRol == rol.IdRol);
        //    if (usuRol == null) return false;
        //    Context.UsuariosRoles.Where(ur => ur.IDP == usu.IDP && ur.IdRol != usuRol.IdRol).ToList().ForEach(ur => ur.EsPrincipal = false);
        //    Context.SaveChanges();
        //    return true;
        //}

        //public IQueryable<DatosUsuario> GetDatosUsuario(Usuario? user) {

        //    if (user == null) {           
        //        throw new ArgumentNullException(nameof(user));
        //    }
        //    return GetDatosUsuarios(user.IDP);
        //}

        //public IQueryable<DatosUsuario> GetDatosUsuarios(int IDP) {

        //    UsuarioRol? usuRol = Context.UsuariosRoles.Where(ur => ur.IDP == IDP && ur.EsPrincipal == true).FirstOrDefault();

        //    if (usuRol == null) {

        //        throw new InvalidDataException("El usuario indicado no tiene un rol principal");
        //    }

        //    Rol mainRol = Context.Roles.Single(r => r.IdRol == usuRol.IdRol);

        //    return Context.DatosUsuarios
        //        .Join(Context.UsuariosRoles, d => new { d.IDP, EsPrincipal = true }, ur => new { ur.IDP, ur.EsPrincipal }, (d, ur) => new { d, ur })
        //        .Join(Context.Roles, x => x.ur.IdRol, r => r.IdRol, (x, r) => new { x.d, r })
        //        .Where(x => x.d.IdArea == mainRol.IdArea && x.r.Prelacion < mainRol.Prelacion).Select(x => x.d);
        //}
    }
}
