using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Negocio.Persistencia.Extensiones;
using Negocio.Persistencia.Modelos;
using System.Data;

namespace Negocio.Persistencia
{
    public class EpsilonDbContext : DbContext
    {
        private readonly ILogger<EpsilonDbContext> _logger;

        public EpsilonDbContext(DbContextOptions<EpsilonDbContext> options, ILogger<EpsilonDbContext> logger) : base(options)
        {
            _logger = logger;
            _extensiones = new ExtensionesEpsilon(this);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region TABLAS

            // TABLA USUARIOS
            modelBuilder.Entity<Usuario>().HasKey(u => u.IdUsuario);
            
            // TABLA ESTADOS USUARIOS
            modelBuilder.Entity<EstadosUsuario>().HasKey(u => u.IdEstadoUsuario);

            //TABLA CITAS
            modelBuilder.Entity<Citas>().HasKey(e => e.IdCita);
            modelBuilder.Entity<Citas>().Property(e => e.IdCita);
            modelBuilder.Entity<Citas>().Property(e => e.IdSede);
            modelBuilder.Entity<Citas>().Property(e => e.FechaInicio);
            modelBuilder.Entity<Citas>().Property(e => e.FechaFin);
            modelBuilder.Entity<Citas>().Property(e => e.IdCliente);
            modelBuilder.Entity<Citas>().Property(e => e.IdEmpleado);
            modelBuilder.Entity<Citas>().Property(e => e.Observaciones);

            //TABLA AGENDA
            modelBuilder.Entity<Agenda>().HasKey(a => a.IdAgenda);
            modelBuilder.Entity<Agenda>().Property(a => a.IdAgenda);
            modelBuilder.Entity<Agenda>().Property(a => a.IdEmpleado);
            modelBuilder.Entity<Agenda>().Property(a => a.HoraInicio);
            modelBuilder.Entity<Agenda>().Property(a => a.HoraFin);
            modelBuilder.Entity<Agenda>().Property(a => a.Disponible);
            modelBuilder.Entity<Agenda>().Property(a => a.IdCita);

            //TABLA FACTURACION
            modelBuilder.Entity<Facturacion>().HasKey(f => f.IdFactura);
            modelBuilder.Entity<Facturacion>().Property(f => f.IdFactura);
            modelBuilder.Entity<Facturacion>().Property(f => f.Importe);
            modelBuilder.Entity<Facturacion>().Property(f => f.FechaFactura);
            modelBuilder.Entity<Facturacion>().Property(f => f.IdCita);

            // CORREOS
            modelBuilder.Entity<CorreosElectronicos>().HasKey(k => k.IdCorreo);

            // NUEVAS ENTIDADES ERP / GESTION EMPRESARIAL
            modelBuilder.Entity<Cliente>().HasKey(c => c.IdCliente);
            modelBuilder.Entity<Personal>().HasKey(p => p.IdEmpleado);
            modelBuilder.Entity<Sede>().HasKey(s => s.IdSede);
            modelBuilder.Entity<Servicio>().HasKey(s => s.IdServicio);
            modelBuilder.Entity<CitaServicio>().HasKey(cs => cs.IdCitaServicio);

            #endregion

            #region VISTAS

            // Vista [[vDatosUsuarios]]
            modelBuilder.Entity<DatosUsuario>().HasKey(e => e.IdUsuario);

            // NUEVAS VISTAS ERP / GESTION EMPRESARIAL
            modelBuilder.Entity<DatosClientes>().HasKey(c => c.IdCliente);
            modelBuilder.Entity<DatosPersonal>().HasKey(p => p.IdEmpleado);
            modelBuilder.Entity<DatosServicios>().HasKey(s => s.IdServicio);
            modelBuilder.Entity<DatosHistoricoCliente>().HasKey(h => new { h.IdCliente, h.IdCita });

            #endregion
        }

        #region COLECCIONES

        public virtual DbSet<Modelos.Usuario> Usuarios { get; set; }
        public virtual DbSet<Modelos.EstadosUsuario> EstadosUsuario { get; set; }
        public virtual DbSet<Modelos.CorreosElectronicos> CorreoElectronico { get; set; }
        public virtual DbSet<Modelos.Citas> Citas { get; set; }
        public virtual DbSet<Modelos.Agenda> Agenda { get; set; }
        public virtual DbSet<Modelos.Facturacion> Facturacion { get; set; }

        public virtual DbSet<Modelos.Cliente> Clientes { get; set; }
        public virtual DbSet<Modelos.Personal> Personal { get; set; }
        public virtual DbSet<Modelos.Sede> Sedes { get; set; }
        public virtual DbSet<Modelos.Servicio> Servicios { get; set; }
        public virtual DbSet<Modelos.CitaServicio> CitaServicios { get; set; }

        #endregion

        #region COLECCION_VISTAS

        public virtual DbSet<Modelos.DatosUsuario> DatosUsuarios { get; set; }
        public virtual DbSet<Modelos.DatosClientes> DatosClientes { get; set; }
        public virtual DbSet<Modelos.DatosPersonal> DatosPersonal { get; set; }
        public virtual DbSet<Modelos.DatosServicios> DatosServicios { get; set; }
        public virtual DbSet<Modelos.DatosHistoricoCliente> DatosHistoricoCliente { get; set; }

        #endregion

        ExtensionesEpsilon _extensiones;

        #region Funciones SQL de usuario

        public bool HayPermisoConsulta(long IDP, long idEntidadSecurizada) => throw new NotSupportedException();
        public bool HayPermisoModificacion(long IDP, long idEntidadSecurizada) => throw new NotSupportedException();
        public bool HayPermisoEliminacion(long IDP, long idEntidadSecurizada) => throw new NotSupportedException();
        public bool HayPermisoEjecucion(long IDP, long idEntidadSecurizada) => throw new NotSupportedException();
        public bool HayPermisoTipoConsulta(long IDP, long idEntidadSecurizada) => throw new NotSupportedException();
        public bool HayPermisoTipoModificacion(long IDP, long idEntidadSecurizada) => throw new NotSupportedException();
        public bool HayPermisoTipoEliminacion(long IDP, long idEntidadSecurizada) => throw new NotSupportedException();
        public bool HayPermisoTipoEjecucion(long IDP, long idEntidadSecurizada) => throw new NotSupportedException();

        #endregion

        public virtual ExtensionesEpsilon Extensions
        {
            get {
                return _extensiones == null ? new ExtensionesEpsilon(this) : _extensiones;
            }
            set {
                _extensiones = value;
            }
        }
    }
}
