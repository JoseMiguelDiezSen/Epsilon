using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Negocio.Persistencia.Modelos;
using Negocio.Persistencia.Modelos.Comun;
using Negocio.Servicios.Comun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Persistencia.Interceptores
{
    //public class EpsilonforSecurityInterceptor : SaveChangesInterceptor
    //{
    //    EspsilonDbContext? contextDb;
    //    string? usuario;
    //    int? IDP;


    //    public EpsilonSecurityInterceptor(IPrincipal principal) : base()
    //    {
    //        usuario = principal.Identity?.Name;
    //    }

    //    ////public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result) {
    //    ////    DbContext? context = eventData.Context;
    //    ////    if (context == null) return base.SavingChanges(eventData, result);
    //    ////    if (context is not EspsilonDbContext) return base.SavingChanges(eventData, result);
    //    ////    contextDb = (EspsilonDbContext)context;

    //    ////    if (IDP == null) {
    //    ////        IDP = contextDb.Usuarios.Single(u => u.Nombre == usuario).IDP;
    //    ////    }


    //    ////    foreach (EntityEntry entry in contextDb.ChangeTracker.Entries()) {

    //    ////        if (entry.Entity is ICalipsoforSecurityModel entidad) {

    //    ////            switch (entry.State) {

    //    ////                case EntityState.Modified:
    //    ////                    result = HayPermisoModificacion(entidad) ? result : InterceptionResult<int>.SuppressWithResult(0);
    //    ////                    break;


    //    ////                case EntityState.Deleted:
    //    ////                    result = HayPermisoEliminacion(entidad) ? result : InterceptionResult<int>.SuppressWithResult(0);
    //    ////                    break;
    //    ////            }
    //    ////        }
    //    ////    }

    //    ////    return base.SavingChanges(eventData, result);
    //    ////}


    //    protected bool HayPermisoEliminacion<T>(T securityModelEntity) where T : ICalipsoforSecurityModel {

    //        if (contextDb == null || securityModelEntity == null) return false;

    //            if (IsAdmin()) return true;

    //        EntidadSecurizada? es = contextDb.Extensions.EntidadesSecurizadas.GetES(securityModelEntity.IdEntidad, securityModelEntity.IdTipo);

    //        if (es == null) return true;

    //        return contextDb.HayPermisoEliminacion(IDP ?? 0, es.IdEntidadSecurizada);

    //    }

    //    protected bool HayPermisoModificacion<T>(T securityModelEntity) where T : ICalipsoforSecurityModel {

    //        if (contextDb == null || securityModelEntity == null) return false;
    //        if (IsAdmin()) return true;

    //        EntidadSecurizada? es = contextDb.Extensions.EntidadesSecurizadas.GetES(securityModelEntity.IdEntidad, securityModelEntity.IdTipo);

    //        return contextDb.HayPermisoModificacion(IDP ?? 0, es.IdEntidadSecurizada);
    //    }
        
    //    protected bool IsAdmin() {

    //        if (contextDb == null) return false;
    //        return contextDb.Roles.Join(contextDb.UsuariosRoles, r => r.IdRol, u => u.IdRol, (r, u) => new { r, u })
    //        .Where(z => z.r.Prelacion == (int)NivelesPrelacion.Administrador && z.u.IDP == IDP).Any();
    //    }
    //}
}