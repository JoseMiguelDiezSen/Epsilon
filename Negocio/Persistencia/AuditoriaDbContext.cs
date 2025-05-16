using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using Negocio.Persistencia.Modelos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Persistencia
{
    public class AuditoriaDbContext :DbContext
    {
        private readonly ILogger<EpsilonDbContext> _logger;

        public AuditoriaDbContext(DbContextOptions<AuditoriaDbContext> options, ILogger<EpsilonDbContext> logger) : base(options) {
            _logger = logger;   
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Tabla Area Directiva
            //modelBuilder.Entity<Registro>().ToTable("Registros");
            //modelBuilder.Entity<Registro>().HasKey(k = k.IdRegistro);
            //modelBuilder.Entity<Registro>().Property(e = e.IdRegistro).UseIdentityColumn();
            //modelBuilder.Entity<Registro>().Property(e = e.Fecha);
            //modelBuilder.Entity<Registro>().Property(e = e.Usuario);
            //modelBuilder.Entity<Registro>().Property(e = e.Aplicacion);
            //modelBuilder.Entity<Registro>().Property(e = e.Evento);
            //modelBuilder.Entity<Registro>().Property(e = e.Nivel);
            //modelBuilder.Entity<Registro>().Property(e = e.Operacion);
        }
    }
}
