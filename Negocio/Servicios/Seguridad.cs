using FluentValidation;
using Microsoft.Extensions.Logging;
using Negocio.Persistencia.Modelos;
using Negocio.Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Negocio.Persistencia.Modelos.Comun;
using Negocio.Servicios.Comun;
using System.Collections.Immutable;
using System.Security.Principal;

namespace Negocio.Servicios
{
    ///<summary> Clase encargada de las funcionalidades relacionadas con la Seguridad de la aplicacion</summary>
    public class Seguridad : ServicioAbstractoEpsilon, ISeguridad
    {
        public Seguridad(EpsilonDbContext context, ILogger<Seguridad> milogger) : base(context, milogger)
        {
  
        }

        #region Métodos de la tabla Usuarios Preferencias

        ///// <summary>  
        ///// Obtiene las preferencias de un usuario por su identificador y nombre.  
        ///// </summary>  
        ///// <param name="idp">El identificador del usuario.</param>  
        ///// <param name="nombre">El nombre de la preferencia.</param>  
        ///// <returns>La preferencia del usuario.</returns>  
        //public PreferenciasUsuarios? GetPreferenciasUsuario(int? idp, string nombre)
        //{
        //    logger.LogTrace(GetEventId(), MethodBase.GetCurrentMethod()?.Name);
        //    return Context.PreferenciasUsuarios.Where(x => x.IdUsuario == idp).Select(x => x).FirstOrDefault();
        //}



        ///// <summary>  
        ///// Elimina las preferencias de un usuario por su identificador y nombre.  
        ///// </summary>  
        ///// <param name="idp">El identificador del usuario.</param>  
        ///// <param name="nombre">El nombre de la preferencia.</param>  
        ////[OperacionAsegurada]
        //public void DeletePreferenciasUsuarios(int idp, string nombre)
        //{
        //    logger.LogInformation(GetEventId(), MethodBase.GetCurrentMethod()?.Name);
        //    PreferenciasUsuarios? pr = Context.PreferenciasUsuarios.SingleOrDefault(c => c.IdUsuario == idp);
        //    if (pr != null)
        //    {
        //        Context.Remove(pr);
        //        Context.SaveChanges();
        //    }
        //}

        ///// <summary>  
        ///// Agrega una nueva preferencia de usuario.  
        ///// </summary>  
        ///// <param name="preferenciaUsuario">La preferencia de usuario a agregar.</param>  
        //public void AddPreferenciaUsuario(PreferenciasUsuarios preferenciaUsuario)
        //{
        //    logger.LogInformation(GetEventId(), MethodBase.GetCurrentMethod()?.Name);
        //    Context.PreferenciasUsuarios.Add(preferenciaUsuario);
        //    Context.SaveChanges();
        //}

        ///// <summary>  
        ///// Actualiza una preferencia de usuario existente.  
        ///// </summary>  
        ///// <param name="preferenciaUsuario">La preferencia de usuario a actualizar.</param>  
        ///// <returns>La preferencia de usuario actualizada.</returns>  
        ///// <exception cref="InvalidOperationException">Se lanza cuando no se aplican los cambios.</exception>  
        //public PreferenciasUsuarios UpdatePreferenciaUsuario(PreferenciasUsuarios preferenciaUsuario)
        //{
        //    PreferenciasUsuarios preferenciaGuardar = Context.PreferenciasUsuarios.Single(x => x.IdPreferencia == preferenciaUsuario.IdPreferencia);
        //    preferenciaGuardar.IdUsuario = preferenciaUsuario.IdUsuario;
        //    preferenciaGuardar.IdPaciente = preferenciaUsuario.IdPaciente;
        //    //preferenciaGuardar.Valor = preferenciaUsuario.Valor;
        //    //preferenciaGuardar.Tipo = preferenciaUsuario.Tipo;
        //    var entity = Context.PreferenciasUsuarios.Update(preferenciaGuardar);
        //    if (Context.SaveChanges() == 0)
        //    {
        //        throw new InvalidOperationException("No se aplicaron los cambios");
        //    }
        //    return entity.Entity;
        //}
        #endregion
    }
}



