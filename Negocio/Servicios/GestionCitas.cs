using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Negocio.Persistencia;
using Negocio.Persistencia.Modelos;
using Negocio.Validadores.Comun;
using System;
using System.Collections.Generic;
using System.Linq;
using Negocio.Servicios.Comun;

namespace Negocio.Servicios
{
    public class GestionCitas : ServicioAbstractoEpsilon, IGestionCitas
    {
        public GestionCitas(EpsilonDbContext context, ILogger<GestionCitas> logger, IValidadoresProgesfor registroValidadores) 
            : base(context, logger, registroValidadores)
        {
        }

        public List<Citas> ObtenerTodas()
        {
            return Context.Citas.ToList();
        }

        public Citas? Get(int id)
        {
            return Context.Citas.FirstOrDefault(c => c.IdCita == id);
        }

        public Citas Add(Citas entity)
        {
            Context.Citas.Add(entity);
            Context.SaveChanges();
            return entity;
        }

        public bool Update(Citas entity)
        {
            var existente = Context.Citas.FirstOrDefault(c => c.IdCita == entity.IdCita);
            if (existente == null) return false;

            existente.IdSede = entity.IdSede;
            existente.FechaInicio = entity.FechaInicio;
            existente.FechaFin = entity.FechaFin;
            existente.IdCliente = entity.IdCliente;
            existente.IdEmpleado = entity.IdEmpleado;
            existente.Observaciones = entity.Observaciones;

            Context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var cita = Context.Citas.FirstOrDefault(c => c.IdCita == id);
            if (cita == null) return false;

            Context.Citas.Remove(cita);
            Context.SaveChanges();
            return true;
        }

        public List<Servicio> GetServicios()
        {
            return Context.Servicios.OrderBy(s => s.NombreServicio).ToList();
        }

        public List<Cliente> GetClientes()
        {
            return Context.Clientes.OrderBy(c => c.NombreCliente).ToList();
        }

        public List<Personal> GetPersonal()
        {
            return Context.Personal.Where(p => p.Activo).OrderBy(p => p.NombreEmpleado).ToList();
        }

        public List<Sede> GetSedes()
        {
            return Context.Sedes.OrderBy(s => s.NombreSede).ToList();
        }

        public Servicio? GetServicioCita(int idCita)
        {
            var relacion = Context.CitaServicios.FirstOrDefault(cs => cs.IdCita == idCita);
            if (relacion != null)
            {
                return Context.Servicios.FirstOrDefault(s => s.IdServicio == relacion.IdServicio);
            }
            return null;
        }

        public CitaServicio AddCitaServicio(CitaServicio entity)
        {
            Context.CitaServicios.Add(entity);
            Context.SaveChanges();
            return entity;
        }

        public bool UpdateCitaServicio(CitaServicio entity)
        {
            var existente = Context.CitaServicios.FirstOrDefault(c => c.IdCita == entity.IdCita);
            if (existente == null)
            {
                Context.CitaServicios.Add(entity);
            }
            else
            {
                existente.IdServicio = entity.IdServicio;
            }
            Context.SaveChanges();
            return true;
        }
    }
}
