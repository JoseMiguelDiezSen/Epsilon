using Microsoft.Extensions.Logging;
using Negocio.Persistencia;
using Negocio.Persistencia.Modelos;
using Negocio.Servicios.Comun;
using Negocio.Validadores.Comun;

namespace Negocio.Servicios
{
    public class GestionFacturacion : ServicioAbstractoEpsilon, IGestionFacturacion
    {
        public GestionFacturacion(EpsilonDbContext context, ILogger<GestionFacturacion> logger, IValidadoresProgesfor registroValidadores)
            : base(context, logger, registroValidadores)
        {
            logger.LogTrace(GetEventId(), "Servicio GestionFacturacion iniciado");
        }

        public IQueryable<Facturacion> GetFacturacion()
        {
            return Context.Facturacion.AsQueryable(); 
        }

        public void SincronizarFacturaCita(int idCita)
        {
            var cita = Context.Citas.FirstOrDefault(c => c.IdCita == idCita);
            if (cita == null) return;

            var idsServicios = Context.CitaServicios
                                   .Where(cs => cs.IdCita == idCita)
                                   .Select(cs => cs.IdServicio)
                                   .ToList();

            double totalImporte = Context.Servicios
                                         .Where(s => idsServicios.Contains(s.IdServicio))
                                         .Sum(s => (double)s.Precio);

            var facturaExistente = Context.Facturacion.FirstOrDefault(f => f.IdCita == idCita);
            
            // Si el importe es 0 y hay factura, se borra (o se deja en 0). Mejor dejamos el registro actualizado.
            if (facturaExistente != null)
            {
                facturaExistente.Importe = totalImporte;
                facturaExistente.FechaFactura = cita.FechaInicio;
            }
            else if (totalImporte > 0)
            {
                Context.Facturacion.Add(new Facturacion
                {
                    IdCita = idCita,
                    Importe = totalImporte,
                    FechaFactura = cita.FechaInicio
                });
            }
            Context.SaveChanges();
        }

        public void RecalcularFacturacionGlobal()
        {
            var citas = Context.Citas.ToList();
            foreach (var cita in citas)
            {
                SincronizarFacturaCita(cita.IdCita);
            }
        }

        public void EliminarFacturaCita(int idCita)
        {
            var facturaExistente = Context.Facturacion.FirstOrDefault(f => f.IdCita == idCita);
            if (facturaExistente != null)
            {
                Context.Facturacion.Remove(facturaExistente);
                Context.SaveChanges();
            }
        }
    }
}

