using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Epsilon.ViewModels
{
    /// <summary>
    /// Modelo de vista para el formulario modal de creación y edición de citas.
    /// </summary>
    public class ViewFormAgregarCita
    {
        public int IdCita { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un paciente.")]
        [Display(Name = "Paciente")]
        public int IdPaciente { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un médico.")]
        [Display(Name = "Médico")]
        public int IdMedico { get; set; }

        [Display(Name = "Tratamiento")]
        public int? IdTratamiento { get; set; }

        [Required(ErrorMessage = "Debe especificar la fecha y hora de inicio.")]
        [Display(Name = "Fecha y Hora de Inicio")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "Debe especificar la fecha y hora de fin.")]
        [Display(Name = "Fecha y Hora de Fin")]
        public DateTime FechaFin { get; set; }

        [Display(Name = "Observaciones")]
        public string? Observaciones { get; set; }

        public int IdClinica { get; set; } = 1;

        // Listas desplegables para el formulario
        public SelectList? ListaPacientes { get; set; }
        public SelectList? ListaMedicos { get; set; }
        public SelectList? ListaTratamientos { get; set; }
        public SelectList? ListaClinicas { get; set; }

        public List<Negocio.Persistencia.Modelos.Medico>? MedicosDisponibles { get; set; }
        public List<Negocio.Persistencia.Modelos.Clinica>? ClinicasDisponibles { get; set; }

        public string? NombrePaciente { get; set; }
        public string? NombreMedico { get; set; }
    }
}
