using Negocio.Persistencia.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Servicios
{
    public interface IGestionMedicos : IServicioEpsilon
    {
        /// <summary>
        /// Obtiene todos los médicos.
        /// </summary>
        /// <returns></returns>
        IQueryable<Medico> GetAllMedicos();

        /// <summary>
        /// Obtiene los datos de vista de los médicos.
        /// </summary>
        /// <returns></returns>
        IQueryable<DatosMedicos> GetDatosMedicos();

        /// <summary>
        /// Añade un nuevo médico.
        /// </summary>
        /// <param name="medico">Médico a añadir</param>
        /// <returns></returns>
        Medico AddMedico(Medico medico);

        /// <summary>
        /// Modifica un médico existente.
        /// </summary>
        /// <param name="medico">Médico a modificar</param>
        /// <returns></returns>
        Medico UpdateMedico(Medico medico);

        /// <summary>
        /// Elimina un médico por su ID.
        /// </summary>
        /// <param name="idMedico">Identificador del médico a eliminar</param>
        /// <returns></returns>
        bool DeleteMedico(int idMedico);

        /// <summary>
        /// Obtiene un médico por su ID.
        /// </summary>
        /// <param name="idMedico">Identificador del médico</param>
        /// <returns></returns>
        Medico? GetMedico(int idMedico);

        /// <summary>
        /// Obtiene los datos detallados de un médico por su ID.
        /// </summary>
        /// <param name="idMedico">Identificador del médico</param>
        /// <returns></returns>
        DatosMedicos? GetDetalleMedico(int idMedico);
    }
}
