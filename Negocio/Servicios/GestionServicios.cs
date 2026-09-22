using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Negocio.Persistencia;
using Negocio.Persistencia.Modelos;
using Negocio.Servicios.Comun;
using Negocio.Validadores.Comun;
using System;
using System.Linq;

namespace Negocio.Servicios
{
    public class GestionServicios : ServicioAbstractoEpsilon, IGestionServicios
    {
        public GestionServicios(EpsilonDbContext context, ILogger<GestionServicios> logger, IValidadoresProgesfor registroValidadores)
            : base(context, logger, registroValidadores)
        {
            logger.LogTrace(GetEventId(), "Servicio GestionServicios iniciado");
        }

        public IQueryable<Servicio> GetAllServicios()
        {
            return Context.Servicios.AsNoTracking();
        }

        public IQueryable<DatosServicios> GetDatosServicios()
        {
            return Context.DatosServicios.AsNoTracking();
        }

        public Servicio AddServicio(Servicio servicio)
        {
            Context.Servicios.Add(servicio);
            Context.SaveChanges();
            return servicio;
        }

        public Servicio UpdateServicio(Servicio servicio)
        {
            var entity = Context.Servicios.Find(servicio.IdServicio);
            if (entity == null)
            {
                throw new Exception($"Servicio con ID {servicio.IdServicio} no encontrado.");
            }

            entity.NombreServicio = servicio.NombreServicio;
            entity.Duracion = servicio.Duracion;
            entity.Color = servicio.Color;
            entity.Precio = servicio.Precio;

            Context.SaveChanges();
            return entity;
        }

        public bool DeleteServicio(int idServicio)
        {
            var entity = Context.Servicios.Find(idServicio);
            if (entity == null)
            {
                return false;
            }

            Context.Servicios.Remove(entity);
            Context.SaveChanges();
            return true;
        }

        public Servicio? GetServicio(int idServicio)
        {
            return Context.Servicios.Find(idServicio);
        }
    }
}
