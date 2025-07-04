﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Negocio.Persistencia.Modelos
{
    [Table("Pacientes")]
    public class Paciente
    {
        [Key]
        public int IdPaciente { get; set; }

        [Required]
        public string? NombrePaciente { get; set; }

        [Required]
        [MaxLength(50)]
        public string? DNI { get; set; }

        [Required]
        public int Telefono { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(50)]
        public string? EMail { get; set; }

        public DateTime FechaNacimiento { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Direccion { get; set; }

        public string? Ciudad { get; set; }
        
        public DateTime FechaAlta { get; set; }

        public int NumeroConsultas { get; set; }

        public bool Asegurado { get; set; }
    }
}
