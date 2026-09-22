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
    public class GestionClientes : ServicioAbstractoEpsilon, IGestionClientes
    {
        public GestionClientes(EpsilonDbContext context, ILogger<GestionClientes> logger, IValidadoresProgesfor registroValidadores) 
            : base(context, logger, registroValidadores)
        {
            logger.LogTrace(GetEventId(), "Servicio GestionClientes iniciado");
        }

        public Cliente AddCliente(Cliente cliente)
        {
            using (var transaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.Clientes.Add(cliente);
                    Context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    logger.LogError(GetEventId(), ex, "Error al insertar cliente: {Message}", ex.Message);
                    transaction.Rollback();
                    throw;
                }
            }
            return cliente;
        }

        public Cliente UpdateCliente(Cliente cliente)
        {
            var entity = Context.Clientes.Find(cliente.IdCliente);
            if (entity == null)
            {
                throw new Exception($"Cliente con ID {cliente.IdCliente} no encontrado.");
            }

            entity.NombreCliente = cliente.NombreCliente;
            entity.DNI = cliente.DNI;
            entity.Telefono = cliente.Telefono;
            entity.EMail = cliente.EMail;
            entity.FechaNacimiento = cliente.FechaNacimiento;
            entity.Direccion = cliente.Direccion;
            entity.Ciudad = cliente.Ciudad;
            entity.FechaAlta = cliente.FechaAlta;
            entity.NumeroServicios = cliente.NumeroServicios;
            entity.Preferente = cliente.Preferente;
            entity.Observaciones = cliente.Observaciones;

            Context.SaveChanges();
            return entity;
        }

        public bool DeleteCliente(int idCliente)
        {
            var cliente = Context.Clientes.FirstOrDefault(u => u.IdCliente == idCliente);
            if (cliente == null)
            {
                return false;
            }

            Context.Clientes.Remove(cliente);
            Context.SaveChanges();
            return true;
        }

        public Cliente GetCliente(int idCliente)
        {
            return Context.Clientes.First(u => u.IdCliente == idCliente);
        }

        public IQueryable<DatosClientes> GetDatosClientes()
        {
            logger.LogTrace(GetEventId(), MethodBase.GetCurrentMethod()?.Name);
            return Context.DatosClientes;
        }

        public DatosClientes? GetDetalleCliente(int idCliente)
        {
            return Context.DatosClientes.FirstOrDefault(x => x.IdCliente == idCliente);
        }
    }
}
