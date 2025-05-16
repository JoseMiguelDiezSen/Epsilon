using Negocio.Persistencia.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Persistencia.Extensiones
{
    public class ExtensionPermisos :ExtensionAbstracta<EpsilonDbContext>
    {

       public ExtensionPermisos(EpsilonDbContext context) : base(context) { }

        //public IQueryable<PermisoUsuarioEntidad> GetPermisosUsuarioEntidad(Usuario usu, EntidadSecurizada entidad) {

        //    if (usu == null || entidad == null) {
        //        return Context.PermisoUsuarioEntidad.Take(0);   
        //    }

        //    IQueryable<PermisoUsuarioEntidad> permisos = (from p in Context.PermisoUsuarioEntidad join u in Context.Usuarios on p.IDP equals u.IDP where p.IdEntidadSecurizada == entidad.IdEntidadSecurizada select p);
        //    return permisos;
        //}

        //public IQueryable<PermisoRolTipoEntidad> GetPermisosRolTipoEntidad(Rol rol, TipoEntidad tipoEntidad) { 
        
        //    if (rol == null || tipoEntidad == null) {
            
        //        return Context.PermisoRolTipoEntidad.Take(0);
            
        //    }

        //    IQueryable<PermisoRolTipoEntidad> permisos = (from p in Context.PermisoRolTipoEntidad join r in Context.Roles on p.IdRol equals r.IdRol where p.IdTipoEntidad == tipoEntidad.IdTipoEntidad select p);
        //    return permisos;
        //}

        //public IQueryable<PermisoRolEntidad> GetPermisosRolEntidad(Rol rol, EntidadSecurizada entidad) {

        //    if (rol == null || entidad == null)
        //    {
        //        return Context.PermisoRolEntidad.Take(0);
        //    }

        //    IQueryable <PermisoRolEntidad> permisos = (from p in Context.PermisoRolEntidad join r in Context.Roles on p.IdRol equals r.IdRol where p.IdEntidadSecurizada == entidad.IdEntidadSecurizada select p);
        //    return permisos;
        //}
    }
}
