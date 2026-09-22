using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Negocio.Persistencia.Modelos
{
    [Table("Clientes")]
    public class Cliente
    {
        /// <summary>
        /// Identificador único del cliente.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCliente { get; set; }

        /// <summary>
        /// Nombre del cliente o razón social.
        /// </summary>
        [Required]
        [MaxLength(150)]
        public string? NombreCliente { get; set; }

        /// <summary>
        /// DNI, CIF o NIF del cliente.
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string? DNI { get; set; }

        /// <summary>
        /// Teléfono de contacto.
        /// </summary>
        [Required]
        public int Telefono { get; set; }

        /// <summary>
        /// Correo electrónico del cliente.
        /// </summary>
        [EmailAddress]
        [MaxLength(100)]
        public string? EMail { get; set; }

        /// <summary>
        /// Fecha de nacimiento o fecha de constitución.
        /// </summary>
        public DateTime? FechaNacimiento { get; set; }

        /// <summary>
        /// Dirección postal o fiscal.
        /// </summary>
        [MaxLength(200)]
        public string? Direccion { get; set; }

        /// <summary>
        /// Ciudad o localidad.
        /// </summary>
        [MaxLength(100)]
        public string? Ciudad { get; set; }

        /// <summary>
        /// Fecha de alta en el sistema.
        /// </summary>
        public DateTime FechaAlta { get; set; }

        /// <summary>
        /// Número de servicios contratados o consultas realizadas.
        /// </summary>
        public int NumeroServicios { get; set; }

        /// <summary>
        /// Indica si es cliente preferente, abonado o asegurado.
        /// </summary>
        public bool Preferente { get; set; }

        /// <summary>
        /// Observaciones adicionales sobre el cliente.
        /// </summary>
        public string? Observaciones { get; set; }

        /// <summary>
        /// Fecha del primer servicio o cita.
        /// </summary>
        public DateTime? FechaPrimeraCita { get; set; }

        /// <summary>
        /// Fecha del último servicio o cita.
        /// </summary>
        public DateTime? FechaUltimaCita { get; set; }
    }
}
