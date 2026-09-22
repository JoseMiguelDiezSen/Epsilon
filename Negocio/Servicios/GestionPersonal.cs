using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Negocio.Persistencia;
using Negocio.Persistencia.Modelos;
using Negocio.Servicios.Comun;
using Negocio.Validadores.Comun;
using System;
using System.Linq;
using System.Reflection;

namespace Negocio.Servicios
{
    public class GestionPersonal : ServicioAbstractoEpsilon, IGestionPersonal
    {
        public GestionPersonal(EpsilonDbContext context, ILogger<GestionPersonal> logger, IValidadoresProgesfor registroValidadores) 
            : base(context, logger, registroValidadores)
        {
            logger.LogTrace(GetEventId(), "Servicio GestionPersonal iniciado");
        }

        public IQueryable<Personal> GetAllPersonal()
        {
            logger.LogTrace(GetEventId(), MethodBase.GetCurrentMethod()?.Name);
            return Context.Personal;
        }

        public IQueryable<DatosPersonal> GetDatosPersonal()
        {
            logger.LogTrace(GetEventId(), MethodBase.GetCurrentMethod()?.Name);
            return Context.DatosPersonal;
        }

        public Personal AddPersonal(Personal personal)
        {
            using (var transaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.Personal.Add(personal);
                    Context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    logger.LogError(GetEventId(), ex, "Error al insertar personal: {Message}", ex.Message);
                    transaction.Rollback();
                    throw;
                }
            }
            return personal;
        }

        public Personal UpdatePersonal(Personal personal)
        {
            var entity = Context.Personal.Find(personal.IdEmpleado);
            if (entity == null)
            {
                throw new Exception($"Empleado con ID {personal.IdEmpleado} no encontrado.");
            }

            entity.NombreEmpleado = personal.NombreEmpleado;
            entity.DNI = personal.DNI;
            entity.NumeroEmpleado = personal.NumeroEmpleado;
            entity.Puesto = personal.Puesto;
            entity.Telefono = personal.Telefono;
            entity.EMail = personal.EMail;
            entity.FechaContratacion = personal.FechaContratacion;
            entity.Activo = personal.Activo;
            entity.Observaciones = personal.Observaciones;
            entity.IdSede = personal.IdSede;

            if (personal.Foto != null && personal.Foto.Length > 0)
            {
                entity.Foto = personal.Foto;
            }

            Context.SaveChanges();
            return entity;
        }

        public bool DeletePersonal(int idEmpleado)
        {
            var entity = Context.Personal.Find(idEmpleado);
            if (entity == null)
            {
                return false;
            }

            Context.Personal.Remove(entity);
            Context.SaveChanges();
            return true;
        }

        public Personal? GetPersonal(int idEmpleado)
        {
            return Context.Personal.Find(idEmpleado);
        }
    }
}
