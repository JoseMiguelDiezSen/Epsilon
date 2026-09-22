using Calipso.Security;
using Epsilon.Attributes;
using Epsilon.Models.Comun;
using Epsilon.Renders;
using Epsilon.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Negocio.Persistencia.Modelos;
using Negocio.Servicios;
using Epsilon.Hubs;


namespace Epsilon.Controllers
{
    /// <summary>
    /// Controlador responsable de la gestión de la Agenda y el calendario FullCalendar.
    /// </summary>
    public class AgendaController : AbstractSecurityController
    {
        private readonly IRazorRenderService _renderService;
        private readonly IGestionCitas _gestionCitas;
        // Permite notificar por SignalR a la página de Facturación cuando cambia una cita
        private readonly IHubContext<FacturacionHub> _hubContext;

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
            IRazorRenderService renderService,
            IHubContext<FacturacionHub> hubContext)

        {
            _gestionCitas = gestionCitas;
            _renderService = renderService;
            _hubContext = hubContext;
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
                var citas = _gestionCitas.ObtenerTodas().ToList();
                var clientes = _gestionCitas.GetClientes()
                    .GroupBy(p => p.IdCliente)
                    .ToDictionary(g => g.Key, g => g.First().NombreCliente ?? "Cliente");

                var personal = _gestionCitas.GetPersonal()
                    .GroupBy(m => m.IdEmpleado)
                    .ToDictionary(g => g.Key, g => g.First().NombreEmpleado ?? "Personal");

                var eventos = citas.Select(cita =>
                {
                    // Nombre del cliente y empleado
                    string nombreCliente = clientes.TryGetValue(cita.IdCliente, out var cli) ? cli : (clientes.TryGetValue(cita.IdCliente, out var cli2) ? cli2 : "Cliente");
                    string nombrePersonal = personal.TryGetValue(cita.IdEmpleado, out var per) ? per : (personal.TryGetValue(cita.IdEmpleado, out var per2) ? per2 : "Personal");

                    // Servicio vinculado a la cita
                    var servicio = _gestionCitas.GetServicioCita(cita.IdCita);
                    string nombreServicio = servicio?.NombreServicio ?? "Servicio estándar";

                    // Determinamos el color: primero el color del servicio, o si no el color según el empleado
                    string colorFinal = _coloresDoctores[Math.Abs(cita.IdEmpleado) % _coloresDoctores.Length];
                    if (!string.IsNullOrWhiteSpace(servicio?.Color))
                    {
                        string col = servicio.Color.Trim();
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
                        title = $"{nombreCliente} — {nombreServicio}",
                        start = cita.FechaInicio.ToString("yyyy-MM-ddTHH:mm:ss"),
                        end = cita.FechaFin.ToString("yyyy-MM-ddTHH:mm:ss"),
                        backgroundColor = colorFinal,
                        borderColor = colorFinal,
                        textColor = "#ffffff",
                        display = "block", // Obliga a FullCalendar a pintar un bloque sólido con color de fondo
                        extendedProps = new
                        {
                            idCita = cita.IdCita,
                            paciente = nombreCliente,
                            medico = nombrePersonal,
                            tratamiento = nombreServicio,
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
        /// Carga la vista parcial modal FormAdd para agregar una nueva cita.
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
                var clientes = _gestionCitas.GetClientes()
                    .Select(p => new { Id = p.IdCliente, Texto = $"{p.NombreCliente} (DNI: {p.DNI})" })
                    .ToList();

                var sedes = _gestionCitas.GetSedes()
                    .Select(c => new { Id = c.IdSede, Texto = c.NombreSede })
                    .ToList();

                var sedesDict = sedes.ToDictionary(c => c.Id, c => c.Texto);
                var gruposSedes = sedesDict.ToDictionary(
                    kvp => kvp.Key,
                    kvp => new SelectListGroup { Name = kvp.Value }
                );

                var personal = _gestionCitas.GetPersonal()
                    .Select(m => new SelectListItem
                    {
                        Value = m.IdEmpleado.ToString(),
                        Text = $"{m.NombreEmpleado} - {m.Puesto}",
                        Group = m.IdSede.HasValue && gruposSedes.ContainsKey(m.IdSede.Value)
                            ? gruposSedes[m.IdSede.Value]
                            : null
                    })
                    .ToList();

                var servicios = _gestionCitas.GetServicios()
                    .Select(t => new { Id = t.IdServicio, Texto = $"{t.NombreServicio} ({t.Duracion} min)" })
                    .ToList();

                var sedesDb = _gestionCitas.GetSedes();
                var personalDb = _gestionCitas.GetPersonal();

                int idSedeInicial = sedes.FirstOrDefault()?.Id ?? 3;

                ViewFormAgregarCita vm = new ViewFormAgregarCita
                {
                    FechaInicio = fechaInicio,
                    FechaFin = fechaFin,
                    IdSede = idSedeInicial,
                    ListaSedes = new SelectList(sedes, "Id", "Texto", idSedeInicial),
                    ListaClientes = new SelectList(clientes, "Id", "Texto"),
                    ListaPersonal = new SelectList(personal, "Value", "Text", null, "Group.Name"),
                    ListaServicios = new SelectList(servicios, "Id", "Texto"),
                    PersonalDisponible = personalDb,
                    SedesDisponibles = sedesDb
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
        public async Task<JsonResult> AgregarCita(ViewFormAgregarCita vm)
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
                    IdSede = vm.IdSede > 0 ? vm.IdSede : 1,
                    IdCliente = vm.IdCliente,
                    IdEmpleado = vm.IdEmpleado,
                    FechaInicio = vm.FechaInicio,
                    FechaFin = vm.FechaFin,
                    Observaciones = vm.Observaciones
                };

                cita = _gestionCitas.Add(cita); if(vm.IdServicio.HasValue) { _gestionCitas.AddCitaServicio(new Negocio.Persistencia.Modelos.CitaServicio { IdCita = cita.IdCita, IdServicio = vm.IdServicio.Value }); }

                // Notificar a los clientes conectados que hubo un cambio en facturación
                await _hubContext.Clients.All.SendAsync("ActualizacionFacturacion");

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
                var cita = _gestionCitas.Get(idCita);
                if (cita == null)
                {
                    jsonResponse = new JsonResponse("404", "No se encontró la cita especificada.", string.Empty);
                    return new JsonResult(jsonResponse);
                }

                // Obtenemos el tratamiento actual asociado a la cita
                var tratamiento = _gestionCitas.GetServicioCita(idCita);

                // Cargar listas desplegables
                var clientes = _gestionCitas.GetClientes()
                    .Select(p => new { Id = p.IdCliente, Texto = $"{p.NombreCliente} (DNI: {p.DNI})" })
                    .ToList();

                var sedes = _gestionCitas.GetSedes()
                    .Select(c => new { Id = c.IdSede, Texto = c.NombreSede })
                    .ToList();

                var sedesDict = sedes.ToDictionary(c => c.Id, c => c.Texto);
                var gruposSedes = sedesDict.ToDictionary(
                    kvp => kvp.Key,
                    kvp => new SelectListGroup { Name = kvp.Value }
                );

                var personal = _gestionCitas.GetPersonal()
                    .Select(m => new SelectListItem
                    {
                        Value = m.IdEmpleado.ToString(),
                        Text = $"{m.NombreEmpleado} - {m.Puesto}",
                        Group = m.IdSede.HasValue && gruposSedes.ContainsKey(m.IdSede.Value)
                            ? gruposSedes[m.IdSede.Value]
                            : null
                    })
                    .ToList();

                var servicios = _gestionCitas.GetServicios()
                    .Select(t => new { Id = t.IdServicio, Texto = $"{t.NombreServicio} ({t.Duracion} min)" })
                    .ToList();

                // Obtenemos la clínica del médico asignado si existe
                int idSedeActual = cita.IdSede;
                var empleadoActual = _gestionCitas.GetPersonal().FirstOrDefault(m => m.IdEmpleado == cita.IdEmpleado);
                if (empleadoActual?.IdSede != null && empleadoActual.IdSede.Value > 0)
                {
                    idSedeActual = empleadoActual.IdSede.Value;
                }

                var sedesDb = _gestionCitas.GetSedes();
                var personalDb = _gestionCitas.GetPersonal();

                ViewFormAgregarCita vm = new ViewFormAgregarCita
                {
                    IdCita = cita.IdCita,
                    IdSede = idSedeActual,
                    IdCliente = cita.IdCliente,
                    IdEmpleado = cita.IdEmpleado,
                    IdServicio = tratamiento?.IdServicio,
                    FechaInicio = cita.FechaInicio,
                    FechaFin = cita.FechaFin,
                    Observaciones = cita.Observaciones,
                    ListaSedes = new SelectList(sedes, "Id", "Texto", idSedeActual),
                    ListaClientes = new SelectList(clientes, "Id", "Texto", cita.IdCliente),
                    ListaPersonal = new SelectList(personal, "Value", "Text", cita.IdEmpleado.ToString(), "Group.Name"),
                    ListaServicios = new SelectList(servicios, "Id", "Texto", tratamiento?.IdServicio),
                    PersonalDisponible = personalDb,
                    SedesDisponibles = sedesDb
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
        public async Task<JsonResult> ModificarCita(ViewFormAgregarCita vm)
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
                    IdSede = vm.IdSede > 0 ? vm.IdSede : 1,
                    IdCliente = vm.IdCliente,
                    IdEmpleado = vm.IdEmpleado,
                    FechaInicio = vm.FechaInicio,
                    FechaFin = vm.FechaFin,
                    Observaciones = vm.Observaciones
                };

                bool resultado = _gestionCitas.Update(cita); if(resultado && vm.IdServicio.HasValue) { _gestionCitas.UpdateCitaServicio(new Negocio.Persistencia.Modelos.CitaServicio { IdCita = cita.IdCita, IdServicio = vm.IdServicio.Value }); }
                if (resultado)
                {
                    // Notificar a los clientes conectados que hubo un cambio en facturación
                    await _hubContext.Clients.All.SendAsync("ActualizacionFacturacion");
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

                var cToUpdate = _gestionCitas.Get(idCita); bool resultado = false; if(cToUpdate != null) { cToUpdate.FechaInicio = fechaInicio; cToUpdate.FechaFin = fin; resultado = _gestionCitas.Update(cToUpdate); }
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
        /// Carga la vista parcial modal FormDelete para confirmar la eliminación de una cita médica.
        /// </summary>
        /// <param name="idCita">Identificador de la cita médica a eliminar</param>
        [HttpGet, AjaxOnly]
        public async Task<ActionResult> ModalEliminarCita(int idCita)
        {
            JsonResponse? jsonResponse;

            try
            {
                var cita = _gestionCitas.Get(idCita);
                if (cita == null)
                {
                    jsonResponse = new JsonResponse("404", "No se encontró la cita especificada.", string.Empty);
                    return new JsonResult(jsonResponse);
                }

                var paciente = _gestionCitas.GetClientes().FirstOrDefault(p => p.IdCliente == cita.IdCliente);
                var medico = _gestionCitas.GetPersonal().FirstOrDefault(m => m.IdEmpleado == cita.IdEmpleado);

                ViewFormAgregarCita vm = new ViewFormAgregarCita
                {
                    IdCita = cita.IdCita,
                    IdCliente = cita.IdCliente,
                    IdEmpleado = cita.IdEmpleado,
                    FechaInicio = cita.FechaInicio,
                    FechaFin = cita.FechaFin,
                    NombreCliente = paciente?.NombreCliente ?? "el paciente",
                    NombreEmpleado = medico?.NombreEmpleado ?? "el médico"
                };

                string html = await _renderService.ToStringAsync("FormDeleteCita", vm);
                jsonResponse = new JsonResponse("200", "Operación realizada correctamente.", html);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al preparar modal de eliminar cita con Id {IdCita}", idCita);
                jsonResponse = new JsonResponse("500", "Error al cargar el formulario de eliminación: " + ex.Message, string.Empty);
            }

            return new JsonResult(jsonResponse);
        }

        /// <summary>
        /// Elimina una cita médica por su identificador.
        /// </summary>
        [HttpPost, AjaxOnly]
        public async Task<JsonResult> EliminarCita(int idCita)
        {
            try
            {
                bool resultado = _gestionCitas.Delete(idCita);
                if (resultado)
                {
                    // Notificar a los clientes conectados que hubo un cambio en facturación
                    await _hubContext.Clients.All.SendAsync("ActualizacionFacturacion");
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


