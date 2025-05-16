using Negocio.Persistencia.Modelos;
using Negocio.Persistencia.Modelos.Comun;
using Negocio.Persistencia.Extensiones;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using FluentValidation.Internal;

namespace Negocio.Persistencia
{
    public class EpsilonDbContext : DbContext
    {
        private readonly ILogger<EpsilonDbContext> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="logger"></param>
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
            modelBuilder.Entity<Usuario>().Property(u => u.IdUsuario);
            modelBuilder.Entity<Usuario>().Property(u => u.Nombre);
            modelBuilder.Entity<Usuario>().Property(u => u.Password);
            modelBuilder.Entity<Usuario>().Property(u => u.Email);
            modelBuilder.Entity<Usuario>().Property(u => u.FechaAlta);
            modelBuilder.Entity<Usuario>().Property(u => u.Telefono);
            modelBuilder.Entity<Usuario>().Property(u => u.RutaFoto);

            // TABLA PERIODOS PLANIFICACION
            modelBuilder.Entity<PeriodoPlanificacion>().HasKey(k => k.IdPeriodo);
            modelBuilder.Entity<PeriodoPlanificacion>().Property(e => e.IdPeriodo);
            modelBuilder.Entity<PeriodoPlanificacion>().Property(e => e.Desde);
            modelBuilder.Entity<PeriodoPlanificacion>().Property(e => e.Hasta);
            modelBuilder.Entity<PeriodoPlanificacion>().Property(e => e.PlanesAfectados);
            modelBuilder.Entity<PeriodoPlanificacion>().Property(e => e.Ejercicio);

            modelBuilder.Entity<PeriodoPlanificacion>().Property(e => e.Estimado);
            modelBuilder.Entity<PeriodoPlanificacion>().Property(e => e.Ejecutado);
            modelBuilder.Entity<PeriodoPlanificacion>().Property(e => e.FechaCreacion);
            // TABLA PREFERENCIAS USUARIOS
            //modelBuilder.Entity<PreferenciasUsuarios>().Property(e => e.IdUsuario);
            //modelBuilder.Entity<PreferenciasUsuarios>().Property(e => e.IdUsuario);

            //Ejemplo clave compuesta
            //modelBuilder.Entity<Usuario>().HasKey(x => new { x.IdEdicionPlanificada, x.IdConceptoCoste, x.Anio, x.Mes });


            #endregion

            #region VISTAS

            //VISTAS

            // Vista [vDatosUsuarios]
            modelBuilder.Entity<DatosUsuario>().HasKey(e => e.IdUsuario);
            modelBuilder.Entity<DatosUsuario>().Property(e => e.IdUsuario);
            modelBuilder.Entity<DatosUsuario>().Property(e => e.Nombre);
            modelBuilder.Entity<DatosUsuario>().Property(e => e.Password);
            modelBuilder.Entity<DatosUsuario>().Property(e => e.EMail);
            modelBuilder.Entity<DatosUsuario>().Property(e => e.FechaAlta);
            modelBuilder.Entity<DatosUsuario>().Property(e => e.Telefono);
            modelBuilder.Entity<DatosUsuario>().Property(e => e.RutaFoto);

            // Vista [vDatosPeriodos]
            modelBuilder.Entity<DatoPeriodo>().HasKey(k => k.IdPeriodo);
            modelBuilder.Entity<DatoPeriodo>().Property(e => e.IdPeriodo);
            //modelBuilder.Entity<DatoPeriodo>().Property(e => e.IdArea);
            modelBuilder.Entity<DatoPeriodo>().Property(e => e.Ejercicio);
            modelBuilder.Entity<DatoPeriodo>().Property(e => e.Estimado);
            modelBuilder.Entity<DatoPeriodo>().Property(e => e.Ejecutado);


            #endregion
        }

        #region COLECCION_TABLAS

        //Tablas
        public virtual DbSet<Modelos.Usuario> Usuarios { get; set; }

        public virtual DbSet<Modelos.PeriodoPlanificacion> PeriodosPlanificacion { get; set; }

        #endregion

        #region COLECCION_VISTAS

        //Vistas
        public virtual DbSet<Modelos.DatosUsuario> DatosUsuarios { get; set; }
        public virtual DbSet<Modelos.DatoPeriodo> DatosPeriodos { get; set; }
                
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

        #region Procedimientos_Almacenados











        #endregion
    }
}
 
