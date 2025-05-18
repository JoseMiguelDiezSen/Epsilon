using Negocio.Persistencia.Modelos;
using Negocio.Persistencia.Modelos.Comun;
using Negocio.Persistencia.Extensiones;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using FluentValidation.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using System.Web.Mvc;
using System.Data.SqlClient;

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
            modelBuilder.Entity<Usuario>().Property(u => u.Activo);
            //modelBuilder.Entity<Usuario>().Property(u => u.TurnoDeTrabajo);

            //TABLA MEDICOS
            modelBuilder.Entity<Medico>().HasKey(m => m.IdMedico);
            modelBuilder.Entity<Medico>().Property(m => m.IdMedico);
            modelBuilder.Entity<Medico>().Property(m => m.NombreMedico);
            modelBuilder.Entity<Medico>().Property(m => m.DNI);
            modelBuilder.Entity<Medico>().Property(m => m.NumeroColegiado);
            modelBuilder.Entity<Medico>().Property(m => m.Especialidad);
            modelBuilder.Entity<Medico>().Property(m => m.Telefono);
            modelBuilder.Entity<Medico>().Property(m => m.EMail);
            modelBuilder.Entity<Medico>().Property(m => m.FechaContratacion);
            modelBuilder.Entity<Medico>().Property(m => m.Activo);
            modelBuilder.Entity<Medico>().Property(m => m.Observaciones);
            modelBuilder.Entity<Medico>().Property(m => m.Foto);

            //TABLA PACIENTES
            modelBuilder.Entity<Paciente>().HasKey(m => m.IdPaciente);
            modelBuilder.Entity<Paciente>().Property(m => m.IdPaciente);
            modelBuilder.Entity<Paciente>().Property(m => m.NombrePaciente);
            //modelBuilder.Entity<Paciente>().Property(m => m.DNI);
            //modelBuilder.Entity<Paciente>().Property(m => m.NumeroColegiado);
            //modelBuilder.Entity<Paciente>().Property(m => m.Especialidad);
            //modelBuilder.Entity<Paciente>().Property(m => m.Telefono);
            //modelBuilder.Entity<Paciente>().Property(m => m.EMail);
            //modelBuilder.Entity<Paciente>().Property(m => m.FechaContratacion);
            //modelBuilder.Entity<Paciente>().Property(m => m.Activo);
            //modelBuilder.Entity<Paciente>().Property(m => m.Observaviones);
            //modelBuilder.Entity<Paciente>().Property(m => m.Foto);

            //TABLA CLINICAS
            modelBuilder.Entity<Clinica>().HasKey(m => m.IdClinica);
            modelBuilder.Entity<Clinica>().Property(m => m.IdClinica);
            modelBuilder.Entity<Clinica>().Property(m => m.NombreClinica);
            modelBuilder.Entity<Clinica>().Property(m => m.DireccionClinica);
            modelBuilder.Entity<Clinica>().Property(m => m.LocalidadClinica);
            modelBuilder.Entity<Clinica>().Property(m => m.TelefonoClinica);
            modelBuilder.Entity<Clinica>().Property(m => m.EMailClinica);
            modelBuilder.Entity<Clinica>().Property(m => m.DirectorClinica);

            //TABLA CITAS
            //modelBuilder.Entity<Citas>().HasKey(m => m.IdMedico);
            //modelBuilder.Entity<Citas>().Property(m => m.IdMedico);
            //modelBuilder.Entity<Citas>().Property(m => m.NombreMedico);
            //modelBuilder.Entity<Citas>().Property(m => m.DNI);
            //modelBuilder.Entity<Citas>().Property(m => m.NumeroColegiado);
            //modelBuilder.Entity<Citas>().Property(m => m.Especialidad);
            //modelBuilder.Entity<Citas>().Property(m => m.Telefono);
            //modelBuilder.Entity<Citas>().Property(m => m.EMail);
            //modelBuilder.Entity<Citas>().Property(m => m.FechaContratacion);
            //modelBuilder.Entity<Citas>().Property(m => m.Activo);
            //modelBuilder.Entity<Citas>().Property(m => m.Observaviones);
            //modelBuilder.Entity<Citas>().Property(m => m.Foto);

            //TABLA AGENDA
            //modelBuilder.Entity<Medicos>().HasKey(m => m.IdMedico);
            //modelBuilder.Entity<Medicos>().Property(m => m.IdMedico);
            //modelBuilder.Entity<Medicos>().Property(m => m.NombreMedico);
            //modelBuilder.Entity<Medicos>().Property(m => m.DNI);
            //modelBuilder.Entity<Medicos>().Property(m => m.NumeroColegiado);
            //modelBuilder.Entity<Medicos>().Property(m => m.Especialidad);
            //modelBuilder.Entity<Medicos>().Property(m => m.Telefono);
            //modelBuilder.Entity<Medicos>().Property(m => m.EMail);
            //modelBuilder.Entity<Medicos>().Property(m => m.FechaContratacion);
            //modelBuilder.Entity<Medicos>().Property(m => m.Activo);
            //modelBuilder.Entity<Medicos>().Property(m => m.Observaviones);
            //modelBuilder.Entity<Medicos>().Property(m => m.Foto);



            //TABLA TRATAMIENTOS
            //modelBuilder.Entity<Tratamiento>().HasKey(m => m.IdMedico);
            //modelBuilder.Entity<Tratamiento>().Property(m => m.IdMedico);
            //modelBuilder.Entity<Tratamiento>().Property(m => m.NombreMedico);
            //modelBuilder.Entity<Tratamiento>().Property(m => m.DNI);
            //modelBuilder.Entity<Tratamiento>().Property(m => m.NumeroColegiado);
            //modelBuilder.Entity<Tratamiento>().Property(m => m.Especialidad);
            //modelBuilder.Entity<Tratamiento>().Property(m => m.Telefono);
            //modelBuilder.Entity<Tratamiento>().Property(m => m.EMail);
            //modelBuilder.Entity<Tratamiento>().Property(m => m.FechaContratacion);
            //modelBuilder.Entity<Tratamiento>().Property(m => m.Activo);
            //modelBuilder.Entity<Tratamiento>().Property(m => m.Observaviones);
            //modelBuilder.Entity<Tratamiento>().Property(m => m.Foto);

            //TABLA FACTURACION
            //modelBuilder.Entity<Medicos>().HasKey(m => m.IdMedico);
            //modelBuilder.Entity<Medicos>().Property(m => m.IdMedico);
            //modelBuilder.Entity<Medicos>().Property(m => m.NombreMedico);
            //modelBuilder.Entity<Medicos>().Property(m => m.DNI);
            //modelBuilder.Entity<Medicos>().Property(m => m.NumeroColegiado);
            //modelBuilder.Entity<Medicos>().Property(m => m.Especialidad);
            //modelBuilder.Entity<Medicos>().Property(m => m.Telefono);
            //modelBuilder.Entity<Medicos>().Property(m => m.EMail);
            //modelBuilder.Entity<Medicos>().Property(m => m.FechaContratacion);
            //modelBuilder.Entity<Medicos>().Property(m => m.Activo);
            //modelBuilder.Entity<Medicos>().Property(m => m.Observaviones);
            //modelBuilder.Entity<Medicos>().Property(m => m.Foto);

            // TABLA PREFERENCIAS USUARIOS
            //modelBuilder.Entity<PreferenciasUsuarios>().Property(e => e.IdUsuario);
            //modelBuilder.Entity<PreferenciasUsuarios>().Property(e => e.IdUsuario);

            //Ejemplo clave compuesta
            //modelBuilder.Entity<Usuario>().HasKey(x => new { x.IdEdicionPlanificada, x.IdConceptoCoste, x.Anio, x.Mes });

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

            #endregion

            #region VISTAS

            // Vista [[vDatosUsuarios]]
            modelBuilder.Entity<DatosUsuario>().HasKey(e => e.IdUsuario);
            modelBuilder.Entity<DatosUsuario>().Property(e => e.IdUsuario);
            modelBuilder.Entity<DatosUsuario>().Property(e => e.Nombre);
            modelBuilder.Entity<DatosUsuario>().Property(e => e.Password);
            modelBuilder.Entity<DatosUsuario>().Property(e => e.EMail);
            modelBuilder.Entity<DatosUsuario>().Property(e => e.FechaAlta);
            modelBuilder.Entity<DatosUsuario>().Property(e => e.Telefono);
            modelBuilder.Entity<DatosUsuario>().Property(e => e.RutaFoto);
            //modelBuilder.Entity<DatosUsuario>().Property(e => e.Activo);
            //modelBuilder.Entity<DatosUsuario>().Property(e => e.TurnoDeTrabajo);

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

        //Tabla usuarios
        public virtual DbSet<Modelos.Usuario> Usuarios { get; set; }

        //Tabla medicos
        public virtual DbSet<Modelos.Medico> Medicos { get; set; }

        //Tabla pacientes
        public virtual DbSet<Modelos.Paciente> Pacientes { get; set; }

        //Tabla clinicas
        public virtual DbSet<Modelos.Clinica> Clinicas { get; set; }

        //Tabla tratamientos
        public virtual DbSet<Modelos.Tratamiento> Tratamientos { get; set; }

       





        public virtual DbSet<Modelos.PeriodoPlanificacion> PeriodosPlanificacion { get; set; }

        #endregion

        #region COLECCION_VISTAS

        //Vistas
        public virtual DbSet<Modelos.DatosUsuario> DatosUsuarios { get; set; }
        public virtual DbSet<Modelos.DatoPeriodo> DatosPeriodos { get; set; }
                
        #endregion

        ExtensionesEpsilon _extensiones;
        EpsilonDbContext _context;

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

        //public async Task<IActionResult> SP_AgregarMedico(Medico medico)
        //{
        //    try
        //    {
        //        // Crear los parámetros del procedimiento almacenado
        //        var DNI = new SqlParameter("@DNI", medico.DNI);
        //        var NombreMedico = new SqlParameter("@NombreMedico", medico.NombreMedico);
        //        var NumeroColegiado = new SqlParameter("@NumeroColegiado", medico.NumeroColegiado);
        //        var Especialidad = new SqlParameter("@Especialidad", medico.Especialidad);
        //        var Telefono = new SqlParameter("@Telefono", medico.Telefono);
        //        var EMail = new SqlParameter("@EMail", medico.EMail);
        //        var FechaContratacion = new SqlParameter("@FechaContratacion", medico.FechaContratacion);
        //        var Activo = new SqlParameter("@Activo", medico.Activo);
        //        var Observaciones = new SqlParameter("@Observaciones",string.IsNullOrEmpty(medico.Observaciones) ? (object)DBNull.Value : medico.Observaciones);
        //        var Foto = new SqlParameter("@Foto", medico.Foto == null ? (object)DBNull.Value : medico.Foto);

        //        // Ejecuta el procedimiento almacenado
        //        await _context.Database.ExecuteSqlRawAsync(
        //            "EXEC spInsertarMedico @NombreMedico, @DNI, @NumeroColegiado, @Especialidad, @Telefono, @EMail, @FechaContratacion, @Activo, @Observaciones, @Foto",
        //            NombreMedico, DNI, NumeroColegiado, Especialidad, Telefono, EMail, FechaContratacion, Activo, Observaciones, Foto);

        //        // Redirige a la vista de índice (u otra acción) tras una inserción exitosa       
        //    }
        //    catch (Exception ex)
        //    {

        //    }
  
        //}

        #endregion
    }
}
 
