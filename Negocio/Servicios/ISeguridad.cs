using Negocio.Persistencia;
using Negocio.Persistencia.Modelos;
using Negocio.Persistencia.Modelos.Comun;
using Negocio.Servicios.Comun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Servicios
{
    /// <summary>
    /// Interfaz correspondiente a la clase seguridad en la que se definen los metodos utilizados en dicha clase.
    /// </summary>
    public interface ISeguridad : IServicioEpsilon
    { 
        #region PREFERENCIASUSUARIOS
       // PreferenciasUsuarios? GetPreferenciasUsuario(int? idUsuario, string nombre);
       // void DeletePreferenciasUsuarios(int idUsuario, string nombre);
       //// public string? GetValorPreferenciaUsuarioPorNombre(int idUsuario, string nombre);
       // public void AddPreferenciaUsuario(PreferenciasUsuarios preferenciaUsuario);
       // PreferenciasUsuarios UpdatePreferenciaUsuario(PreferenciasUsuarios preferenciaUsuario);

        #endregion
    }
}















