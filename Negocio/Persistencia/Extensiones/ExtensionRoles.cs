using Negocio.Persistencia.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Persistencia.Extensiones
{
    public class ExtensionRoles :ExtensionAbstracta<EpsilonDbContext>
    {

        public ExtensionRoles(EpsilonDbContext context) : base(context) { }


        //public Rol AddSecure(Rol rol) { 
        
        //    Context.Roles.Add(rol);
        
        //    EntidadSecurizada es = new EntidadSecurizada()
        //    {
        //        IdEntidadReferenciada = rol.IdRol,
        //        IdTipo = rol.IdMiTipo
        //    };

        //    Context.EntidadesSecurizadas.Add(es);

        //    // Se dan permisos al admin
        //    Rol? rolAdmin = Context.Roles.SingleOrDefault(r => r.Prelacion == 255 && rol.IdArea == 0);
        //    if (rolAdmin != null){

        //        Context.PermisoRolEntidad.Add(new PermisoRolEntidad()
        //        {
        //            IdEntidadSecurizada = es.IdEntidadSecurizada,
        //            IdRol = rolAdmin.IdRol,
        //            ConsultaConcedida = true,
        //            ModificacionConcedida = true,
        //            EliminacionConcedida = true,
        //            EjecucionConcedida=true,
        //        });
        //    }
        //    return rol;
        //}

        //public bool Exists(string rolName) {
        //    int n = Context.Roles.Count(u => rolName.Equals(u.NombreRol));
        //    return n > 0;   
        //}

        //public bool Exists(long idRol){
        //    int n = Context.Roles.Count(u => u.IdRol == idRol);
        //    return n > 0;
        //}

        //public EntidadSecurizada? GetEntidadSecurizada(Rol rol) {
        //    return Context.EntidadesSecurizadas.SingleOrDefault(e => e.IdEntidadReferenciada == rol.IdRol && e.IdTipo == rol.IdMiTipo);
        //}

        //public IQueryable<Usuario> GetUsuariosRol(Rol rol) {
        //    IQueryable<Usuario> usus = (from ur in Context.UsuariosRoles join u in Context.Usuarios on ur.IDP equals u.IDP where ur.IdRol == rol.IdRol select u);
        //    return usus;
        //}

        //public IQueryable<Rol> GetSecured(long IDP) {

        //    return from u in Context.Roles
        //           join es in Context.EntidadesSecurizadas on new { id = u.IdRol, tipo = u.IdMiTipo } equals new { id = es.IdEntidadReferenciada, tipo = es.IdTipo } into uyes
        //           from ues in uyes.DefaultIfEmpty()
        //           where Context.HayPermisoConsulta(IDP, ues.IdEntidadSecurizada)
        //           select u;
        //}

        //public bool AddRol(Usuario usu, Rol rol) {
        //    if (usu == null || rol == null) return false;
        //    UsuarioRol usuRol = new UsuarioRol();
        //    usuRol.IDP = usu.IDP;
        //    usuRol.IdRol = rol.IdRol;
        //    Context.UsuariosRoles.Add(usuRol);
        //    return true;
        //}

        //public bool RemoveRol(Usuario usu, Rol rol)
        //{
        //    if (usu == null || rol == null) return false;
        //    UsuarioRol? usuRol = Context.UsuariosRoles.SingleOrDefault(ur => ur.IDP == usu.IDP && ur.IdRol == rol.IdRol);

        //    if (usuRol == null) return false;
        //    Context.UsuariosRoles.Remove(usuRol);
        //    return true;
        //}
    }
}
