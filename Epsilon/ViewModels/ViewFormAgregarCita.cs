using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace Epsilon.ViewModels
{
    public class ViewFormAgregarCita
    {
        public int IdCita { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un cliente.")]
        [Display(Name = "Cliente")]
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un empleado.")]
        [Display(Name = "Empleado")]
        public int IdEmpleado { get; set; }

        [Display(Name = "Servicio")]
        public int? IdServicio { get; set; }

        [Required(ErrorMessage = "Debe especificar la fecha y hora de inicio.")]
        [Display(Name = "Fecha y Hora de Inicio")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "Debe especificar la fecha y hora de fin.")]
        [Display(Name = "Fecha y Hora de Fin")]
        public DateTime FechaFin { get; set; }

        [Display(Name = "Observaciones")]
        public string? Observaciones { get; set; }

        public int IdSede { get; set; } = 1;

        public SelectList? ListaClientes { get; set; }
        public SelectList? ListaPersonal { get; set; }
        public SelectList? ListaServicios { get; set; }
        public SelectList? ListaSedes { get; set; }

        public List<Negocio.Persistencia.Modelos.Personal>? PersonalDisponible { get; set; }
        public List<Negocio.Persistencia.Modelos.Sede>? SedesDisponibles { get; set; }

        public string? NombreCliente { get; set; }
        public string? NombreEmpleado { get; set; }
    }
}
