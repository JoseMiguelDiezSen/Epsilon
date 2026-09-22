using Calipso.Security;
using Epsilon.Attributes;
using Epsilon.Models.Comun;
using Epsilon.Renders;
using Epsilon.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Negocio.Persistencia.Modelos;
using Negocio.Servicios;

namespace Epsilon.Controllers
{
    /// <summary>
    /// Controlador responsable de la gestión de la Agenda y el calendario FullCalendar.
    /// </summary>
    public class AgendaController : AbstractSecurityController
    {
        private readonly IRazorRenderService _renderService;
        private readonly IGestionCitas _gestionCitas;

        // Paleta de colores para diferenciar citas por doctor si el tratamiento no tiene color
        private readonly string[] _coloresDoctores = new[]
        {
            "#2c3e50", // Azul medianoche
            "#27ae60", // Verde esmeralda
            "#8e44ad", // Púrpura
            "#d35400", // Naranja óxido
            "#16a085", // Verde azulado
            "#c0392b", // Rojo granate
            "#2980b9"  // Azul océano
        };

        public AgendaController(
            ILogger<AgendaController> logger,
            IGestionCitas gestionCitas,
            IRazorRenderService renderService) : base(logger)
        {
            _gestionCitas = gestionCitas;
            _renderService = renderService;
        }

        /// <summary>
        /// Vista principal de la agenda con el calendario FullCalendar.
        /// </summary>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Devuelve el listado de citas en formato JSON compatible con FullCalendar.
        /// Se llama automáticamente al cargar el calendario y al refrescarlo.
        /// </summary>
        [HttpGet]
        public IActionResult GetEventosCalendario()
        {
            try
            {
                // Obtenemos las citas y los catálogos en memoria para armar los títulos y colores
                var citas = _gestionCitas.GetCitas().ToList();
                var pacientes = _gestionCitas.GetPacientes()
                    .GroupBy(p => p.IdPaciente)
                    .ToDictionary(g => g.Key, g => g.First().NombrePaciente);

                var medicos = _gestionCitas.GetMedicos()
                    .GroupBy(m => m.IdMedico)
                    .ToDictionary(g => g.Key, g => g.First().NombreMedico);

                var tratamientos = _gestionCitas.GetTratamientos()
                    .GroupBy(t => t.IdTratamiento)
                    .ToDictionary(g => g.Key, g => g.First());

                var eventos = citas.Select(cita =>
                {
                    // Nombre del paciente y médico
                    string nombrePaciente = pacientes.TryGetValue(cita.IdPaciente, out var pac) ? pac : "Paciente";
                    string nombreMedico = medicos.TryGetValue(cita.IdMedico, out var med) ? med : "Médico";

                    // Tratamiento vinculado a la cita
                    var tratamiento = _gestionCitas.GetTratamientoCita(cita.IdCita);
                    string nombreTratamiento = tratamiento?.NombreTratamiento ?? "Consulta general";

                    // Determinamos el color: primero el color del tratamiento, o si no el color según el médico
                    string colorFinal = _coloresDoctores[Math.Abs(cita.IdMedico) % _coloresDoctores.Length];
                    if (!string.IsNullOrWhiteSpace(tratamiento?.Color))
                    {
                        string col = tratamiento.Color.Trim();
                        if (col.StartsWith("#") || col.StartsWith("rgb", StringComparison.OrdinalIgnoreCase))
                        {
                            colorFinal = col;
                        }
                        else if (col.Length == 6 && System.Text.RegularExpressions.Regex.IsMatch(col, @"\A\b[0-9a-fA-F]+\b\Z"))
                        {
                            colorFinal = "#" + col;
                        }
                        else
                        {
                            colorFinal = col;
                        }
                    }

                    return new
                    {
                        id = cita.IdCita,
                        title = $"{nombrePaciente} — {nombreTratamiento}",
                        start = cita.FechaInicio.ToString("yyyy-MM-ddTHH:mm:ss"),
                        end = cita.FechaFin.ToString("yyyy-MM-ddTHH:mm:ss"),
                        backgroundColor = colorFinal,
                        borderColor = colorFinal,
                        textColor = "#ffffff",
                        display = "block", // Obliga a FullCalendar a pintar un bloque sólido con color de fondo
                        extendedProps = new
                        {
                            idCita = cita.IdCita,
                            paciente = nombrePaciente,
                            medico = nombreMedico,
                            tratamiento = nombreTratamiento,
                            observaciones = cita.Observaciones ?? string.Empty
                        }
                    };
                }).ToList();

                return Json(eventos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar los eventos del calendario");
                return Json(new List<object>());
            }
        }

        /// <summary>
        /// Carga la vista parcial modal FormAddCita para agregar una nueva cita.
        /// </summary>
        /// <param name="date">Fecha y hora seleccionada en el calendario</param>
        [HttpGet, AjaxOnly]
        public async Task<ActionResult> ModalAgregarCita(string? date)
        {
            JsonResponse? jsonResponse;

            try
            {
                // Fecha de inicio: la seleccionada en el calendario o la hora actual
                DateTime fechaInicio = DateTime.Now;
                if (!string.IsNullOrWhiteSpace(date) && DateTime.TryParse(date, out DateTime parsedDate))
                {
                    fechaInicio = parsedDate;
                }

                // Fecha de fin por defecto: 30 minutos después
                DateTime fechaFin = fechaInicio.AddMinutes(30);

                // Cargar listas desplegables para el formulario
                var pacientes = _gestionCitas.GetPacientes()
                    .Select(p => new { Id = p.IdPaciente, Texto = $"{p.NombrePaciente} (DNI: {p.DNI})" })
                    .ToList();

                var clinicas = _gestionCitas.GetClinicas()
                    .Select(c => new { Id = c.IdClinica, Texto = c.NombreClinica })
                    .ToList();

                var clinicasDict = clinicas.ToDictionary(c => c.Id, c => c.Texto);
                var gruposClinicas = clinicasDict.ToDictionary(
                    kvp => kvp.Key,
                    kvp => new SelectListGroup { Name = kvp.Value }
                );

                var medicos = _gestionCitas.GetMedicos()
                    .Select(m => new SelectListItem
                    {
                        Value = m.IdMedico.ToString(),
                        Text = $"{m.NombreMedico} - {m.Especialidad}",
                        Group = m.IdClinica.HasValue && gruposClinicas.ContainsKey(m.IdClinica.Value)
                            ? gruposClinicas[m.IdClinica.Value]
                            : null
                    })
                    .ToList();

                var tratamientos = _gestionCitas.GetTratamientos()
                    .Select(t => new { Id = t.IdTratamiento, Texto = $"{t.NombreTratamiento} ({t.Duracion} min)" })
                    .ToList();

                var clinicasDb = _gestionCitas.GetClinicas();
                var medicosDb = _gestionCitas.GetMedicos();

                int idClinicaInicial = clinicas.FirstOrDefault()?.Id ?? 3;

                ViewFormAgregarCita vm = new ViewFormAgregarCita
                {
                    FechaInicio = fechaInicio,
                    FechaFin = fechaFin,
                    IdClinica = idClinicaInicial,
                    ListaClinicas = new SelectList(clinicas, "Id", "Texto", idClinicaInicial),
                    ListaPacientes = new SelectList(pacientes, "Id", "Texto"),
                    ListaMedicos = new SelectList(medicos, "Value", "Text", null, "Group.Name"),
                    ListaTratamientos = new SelectList(tratamientos, "Id", "Texto"),
                    MedicosDisponibles = medicosDb,
                    ClinicasDisponibles = clinicasDb
                };

                // Renderizamos la vista parcial a HTML
                string html = await _renderService.ToStringAsync("FormAddCita", vm);
                jsonResponse = new JsonResponse("200", "Operación realizada correctamente.", html);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al preparar modal de agregar cita");
                jsonResponse = new JsonResponse("500", "Error al cargar el formulario: " + ex.Message, string.Empty);
            }

            return new JsonResult(jsonResponse);
        }

        /// <summary>
        /// Acción POST invocada por AJAX para insertar una nueva cita en la base de datos.
        /// </summary>
        [HttpPost, AjaxOnly]
        public JsonResult AgregarCita(ViewFormAgregarCita vm)
        {
            try
            {
                // Validación básica de coherencia de fechas
                if (vm.FechaFin <= vm.FechaInicio)
                {
                    vm.FechaFin = vm.FechaInicio.AddMinutes(30);
                }

                Citas cita = new Citas
                {
                    IdClinica = vm.IdClinica > 0 ? vm.IdClinica : 1,
                    IdPaciente = vm.IdPaciente,
                    IdMedico = vm.IdMedico,
                    FechaInicio = vm.FechaInicio,
                    FechaFin = vm.FechaFin,
                    Observaciones = vm.Observaciones
                };

                _gestionCitas.AddCita(cita, vm.IdTratamiento);

                return new JsonResult(new { StatusCode = 200, message = "Cita creada con éxito." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al insertar cita médica");
                Response.StatusCode = 500;
                return new JsonResult(new { StatusCode = 500, message = "Error al crear la cita: " + ex.Message });
            }
        }

        /// <summary>
        /// Carga la vista parcial modal FormModificarCita con los datos de una cita existente para su edición o borrado.
        /// </summary>
        /// <param name="idCita">Identificador de la cita médica a modificar</param>
        [HttpGet, AjaxOnly]
        public async Task<ActionResult> ModalModificarCita(int idCita)
        {
            JsonResponse? jsonResponse;

            try
            {
                var cita = _gestionCitas.GetCita(idCita);
                if (cita == null)
                {
                    jsonResponse = new JsonResponse("404", "No se encontró la cita especificada.", string.Empty);
                    return new JsonResult(jsonResponse);
                }

                // Obtenemos el tratamiento actual asociado a la cita
                var tratamiento = _gestionCitas.GetTratamientoCita(idCita);

                // Cargar listas desplegables
                var pacientes = _gestionCitas.GetPacientes()
                    .Select(p => new { Id = p.IdPaciente, Texto = $"{p.NombrePaciente} (DNI: {p.DNI})" })
                    .ToList();

                var clinicas = _gestionCitas.GetClinicas()
                    .Select(c => new { Id = c.IdClinica, Texto = c.NombreClinica })
                    .ToList();

                var clinicasDict = clinicas.ToDictionary(c => c.Id, c => c.Texto);
                var gruposClinicas = clinicasDict.ToDictionary(
                    kvp => kvp.Key,
                    kvp => new SelectListGroup { Name = kvp.Value }
                );

                var medicos = _gestionCitas.GetMedicos()
                    .Select(m => new SelectListItem
                    {
                        Value = m.IdMedico.ToString(),
                        Text = $"{m.NombreMedico} - {m.Especialidad}",
                        Group = m.IdClinica.HasValue && gruposClinicas.ContainsKey(m.IdClinica.Value)
                            ? gruposClinicas[m.IdClinica.Value]
                            : null
                    })
                    .ToList();

                var tratamientos = _gestionCitas.GetTratamientos()
                    .Select(t => new { Id = t.IdTratamiento, Texto = $"{t.NombreTratamiento} ({t.Duracion} min)" })
                    .ToList();

                // Obtenemos la clínica del médico asignado si existe
                int idClinicaActual = cita.IdClinica;
                var medicoActual = _gestionCitas.GetMedicos().FirstOrDefault(m => m.IdMedico == cita.IdMedico);
                if (medicoActual?.IdClinica != null && medicoActual.IdClinica.Value > 0)
                {
                    idClinicaActual = medicoActual.IdClinica.Value;
                }

                var clinicasDb = _gestionCitas.GetClinicas();
                var medicosDb = _gestionCitas.GetMedicos();

                ViewFormAgregarCita vm = new ViewFormAgregarCita
                {
                    IdCita = cita.IdCita,
                    IdClinica = idClinicaActual,
                    IdPaciente = cita.IdPaciente,
                    IdMedico = cita.IdMedico,
                    IdTratamiento = tratamiento?.IdTratamiento,
                    FechaInicio = cita.FechaInicio,
                    FechaFin = cita.FechaFin,
                    Observaciones = cita.Observaciones,
                    ListaClinicas = new SelectList(clinicas, "Id", "Texto", idClinicaActual),
                    ListaPacientes = new SelectList(pacientes, "Id", "Texto", cita.IdPaciente),
                    ListaMedicos = new SelectList(medicos, "Value", "Text", cita.IdMedico.ToString(), "Group.Name"),
                    ListaTratamientos = new SelectList(tratamientos, "Id", "Texto", tratamiento?.IdTratamiento),
                    MedicosDisponibles = medicosDb,
                    ClinicasDisponibles = clinicasDb
                };

                string html = await _renderService.ToStringAsync("FormModificarCita", vm);
                jsonResponse = new JsonResponse("200", "Operación realizada correctamente.", html);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al preparar modal de modificar cita con Id {IdCita}", idCita);
                jsonResponse = new JsonResponse("500", "Error al cargar el formulario de modificación: " + ex.Message, string.Empty);
            }

            return new JsonResult(jsonResponse);
        }

        /// <summary>
        /// Acción POST invocada por AJAX para guardar las modificaciones de una cita médica.
        /// </summary>
        [HttpPost, AjaxOnly]
        public JsonResult ModificarCita(ViewFormAgregarCita vm)
        {
            try
            {
                if (vm.FechaFin <= vm.FechaInicio)
                {
                    vm.FechaFin = vm.FechaInicio.AddMinutes(30);
                }

                Citas cita = new Citas
                {
                    IdCita = vm.IdCita,
                    IdClinica = vm.IdClinica > 0 ? vm.IdClinica : 1,
                    IdPaciente = vm.IdPaciente,
                    IdMedico = vm.IdMedico,
                    FechaInicio = vm.FechaInicio,
                    FechaFin = vm.FechaFin,
                    Observaciones = vm.Observaciones
                };

                bool resultado = _gestionCitas.UpdateCita(cita, vm.IdTratamiento);
                if (resultado)
                {
                    return new JsonResult(new { StatusCode = 200, message = "Cita modificada correctamente." });
                }

                return new JsonResult(new { StatusCode = 404, message = "No se pudo encontrar la cita para modificar." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al modificar la cita médica");
                return new JsonResult(new { StatusCode = 500, message = "Error al modificar la cita: " + ex.Message });
            }
        }

        /// <summary>
        /// Actualiza la fecha/hora de inicio y fin de una cita existente.
        /// Se ejecuta al arrastrar (drop) o redimensionar (resize) una cita en FullCalendar.
        /// </summary>
        [HttpPost, AjaxOnly]
        public JsonResult ActualizarFechaCita(int idCita, DateTime fechaInicio, DateTime? fechaFin)
        {
            try
            {
                DateTime fin = fechaFin ?? fechaInicio.AddMinutes(30);

                bool resultado = _gestionCitas.UpdateFechaCita(idCita, fechaInicio, fin);
                if (resultado)
                {
                    return new JsonResult(new { StatusCode = 200, message = "Fecha de la cita actualizada correctamente." });
                }

                return new JsonResult(new { StatusCode = 404, message = "No se encontró la cita especificada." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la fecha de la cita");
                return new JsonResult(new { StatusCode = 500, message = "Error al actualizar la cita: " + ex.Message });
            }
        }

        /// <summary>
        /// Elimina una cita médica por su identificador.
        /// </summary>
        [HttpPost, AjaxOnly]
        public JsonResult EliminarCita(int idCita)
        {
            try
            {
                bool resultado = _gestionCitas.DeleteCita(idCita);
                if (resultado)
                {
                    return new JsonResult(new { StatusCode = 200, message = "Cita eliminada correctamente." });
                }

                return new JsonResult(new { StatusCode = 404, message = "No se encontró la cita a eliminar." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la cita");
                return new JsonResult(new { StatusCode = 500, message = "Error al eliminar la cita: " + ex.Message });
            }
        }
    }
}
