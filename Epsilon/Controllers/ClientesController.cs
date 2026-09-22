using Calipso.Security;
using Epsilon.Attributes;
using Epsilon.Models;
using Epsilon.Models.Comun;
using Epsilon.Renders;
using Epsilon.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Negocio.Persistencia;
using Negocio.Persistencia.Modelos;
using Negocio.Servicios;
using Negocio.Servicios.Negocio.Servicios;
using OfficeOpenXml;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Epsilon.Controllers
{
    public class ClientesController : AbstractSecurityController
    {
        private readonly IRazorRenderService _renderService;
        private readonly IGestionClientes _gestionClientes;
        private readonly EpsilonDbContext _context;
        private readonly IInformes _informes;
        private readonly IGestionEmail _email;

        public ClientesController(
            ILogger<ClientesController> logger,
            IGestionClientes gestionClientes,
            IRazorRenderService renderService,
            EpsilonDbContext context,
            IInformes informes,
            IGestionEmail email) : base(logger)
        {
            _gestionClientes = gestionClientes;
            _renderService = renderService;
            _context = context;
            _informes = informes;
            _email = email;
        }

        public IActionResult Index()
        {
            ClientesViewModel vmClientes = new ClientesViewModel();

            IQueryable<DatosClientes> datosClientes = _gestionClientes
                .GetDatosClientes()
                .AsNoTracking()
                .OrderBy(x => x.IdCliente)
                .Skip((vmClientes.PaginaActual - 1) * vmClientes.RegistrosPorPagina)
                .Take(vmClientes.RegistrosPorPagina);

            var clientes = datosClientes
                .ToList()
                .Select(x => new ViewClientes(x))
                .ToList();

            vmClientes.Clientes = clientes;

            return View("Index", vmClientes);
        }

        [HttpPost, AjaxOnly]
        public async Task<JsonResult> FiltrarClientesAsync(ClientesViewModel vmClientes)
        {
            JsonResponse? jsonResponse;

            try
            {
                IQueryable<DatosClientes> datosClientes = _gestionClientes.GetDatosClientes();

                if (!string.IsNullOrWhiteSpace(vmClientes.NombreCliente))
                {
                    datosClientes = datosClientes.Where(p => p.NombreCliente != null && p.NombreCliente.Contains(vmClientes.NombreCliente));
                }
                if (!string.IsNullOrWhiteSpace(vmClientes.DNI))
                {
                    datosClientes = datosClientes.Where(p => p.DNI == vmClientes.DNI);
                }
                if (vmClientes.Telefono > 0)
                {
                    datosClientes = datosClientes.Where(p => p.Telefono == vmClientes.Telefono);
                }
                if (!string.IsNullOrWhiteSpace(vmClientes.Ciudad))
                {
                    datosClientes = datosClientes.Where(p => p.Ciudad != null && p.Ciudad.Contains(vmClientes.Ciudad));
                }

                vmClientes.Clientes = await datosClientes
                    .OrderBy(x => x.IdCliente)
                    .Skip((vmClientes.PaginaActual - 1) * vmClientes.RegistrosPorPagina)
                    .Take(vmClientes.RegistrosPorPagina)
                    .Select(x => new ViewClientes(x))
                    .ToListAsync();

                string data = await _renderService.ToStringAsync("TablaClientes", vmClientes.Clientes);
                jsonResponse = new JsonResponse("200", "Operación realizada correctamente.", data);
            }
            catch (Exception ex)
            {
                jsonResponse = new JsonResponse("500", "La operación no se pudo realizar.", string.Empty, "Error: " + ex.Message);
            }

            return new JsonResult(jsonResponse);
        }

        #region CRUD-CLIENTES

        [HttpGet, AjaxOnly]
        public async Task<ActionResult> ModalAgregarCliente()
        {
            ViewFormAgregarCliente vmAgregarCliente = new ViewFormAgregarCliente
            {
                FechaAlta1 = DateTime.Now
            };
            string data = await _renderService.ToStringAsync("FormAddCliente", vmAgregarCliente);
            JsonResponse jsonResponse = new JsonResponse("200", "Operación realizada correctamente.", data);
            return new JsonResult(jsonResponse);
        }

        [HttpPost, AjaxOnly]
        public JsonResult AgregarCliente(ViewFormAgregarCliente vmCliente)
        {
            try
            {
                Cliente cliente = new Cliente
                {
                    NombreCliente = vmCliente.NombreCliente,
                    DNI = vmCliente.DNI,
                    Telefono = vmCliente.Telefono,
                    EMail = vmCliente.EMail,
                    FechaNacimiento = vmCliente.FechaNacimiento,
                    Direccion = vmCliente.Direccion,
                    Ciudad = vmCliente.Ciudad,
                    FechaAlta = vmCliente.FechaAlta1,
                    NumeroServicios = vmCliente.NumeroServicios,
                    Preferente = vmCliente.Preferente,
                    Observaciones = vmCliente.Observaciones ?? vmCliente.Comentarios
                };

                _gestionClientes.AddCliente(cliente);
                return new JsonResult(new { StatusCode = 200, message = "Cliente agregado correctamente" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { StatusCode = 500, message = "Error: " + ex.Message });
            }
        }

        [HttpGet, AjaxOnly]
        public async Task<ActionResult> GetModalModificarCliente(int idCliente)
        {
            var cliente = _gestionClientes.Context.Clientes.First(x => x.IdCliente == idCliente);

            ViewFormAgregarCliente vmCliente = new ViewFormAgregarCliente
            {
                IdCliente = idCliente,
                NombreCliente = cliente.NombreCliente,
                DNI = cliente.DNI,
                Telefono = cliente.Telefono,
                EMail = cliente.EMail,
                Direccion = cliente.Direccion,
                FechaNacimiento = cliente.FechaNacimiento,
                Ciudad = cliente.Ciudad,
                FechaAlta1 = cliente.FechaAlta,
                NumeroServicios = cliente.NumeroServicios,
                Preferente = cliente.Preferente,
                Observaciones = cliente.Observaciones
            };

            string data = await _renderService.ToStringAsync("FormModificarCliente", vmCliente);
            JsonResponse jsonResponse = new JsonResponse("200", "Operación realizada correctamente.", data);
            return new JsonResult(jsonResponse);
        }

        [HttpPost, AjaxOnly]
        public JsonResult ModificarCliente(ViewFormAgregarCliente vmCliente)
        {
            try
            {
                Cliente cliente = new Cliente
                {
                    IdCliente = vmCliente.IdCliente,
                    NombreCliente = vmCliente.NombreCliente,
                    DNI = vmCliente.DNI,
                    Telefono = vmCliente.Telefono,
                    EMail = vmCliente.EMail,
                    Direccion = vmCliente.Direccion,
                    FechaNacimiento = vmCliente.FechaNacimiento,
                    Ciudad = vmCliente.Ciudad,
                    FechaAlta = vmCliente.FechaAlta1,
                    NumeroServicios = vmCliente.NumeroServicios,
                    Preferente = vmCliente.Preferente,
                    Observaciones = vmCliente.Observaciones ?? vmCliente.Comentarios
                };

                _gestionClientes.UpdateCliente(cliente);
                return new JsonResult(new { StatusCode = 200, message = "Cliente actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { StatusCode = 500, message = "Error: " + ex.Message });
            }
        }

        [HttpGet, AjaxOnly]
        public async Task<JsonResult> EliminarClienteAsync(long idCliente)
        {
            try
            {
                var cliente = _gestionClientes.Context.Clientes.First(u => u.IdCliente == (int)idCliente);
                ViewFormAgregarCliente vm = new ViewFormAgregarCliente
                {
                    IdCliente = cliente.IdCliente,
                    NombreCliente = cliente.NombreCliente
                };
                string data = await _renderService.ToStringAsync("FormDeleteCliente", vm);
                return new JsonResult(new JsonResponse("200", "Operación realizada correctamente.", data));
            }
            catch (Exception ex)
            {
                return new JsonResult(new JsonResponse("500", "Error al procesar solicitud: " + ex.Message));
            }
        }

        [HttpPost, AjaxOnly]
        public JsonResult ConfirmarEliminarCliente(int idCliente)
        {
            try
            {
                _gestionClientes.DeleteCliente(idCliente);
                return new JsonResult(new { StatusCode = 200, message = "Cliente eliminado con éxito." });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { StatusCode = 500, message = "Error: " + ex.Message });
            }
        }

        public IActionResult DetalleCliente(int idCliente)
        {
            var detalle = _gestionClientes.GetDetalleCliente(idCliente);
            if (detalle == null) return NotFound();
            return PartialView("DetalleCliente", detalle);
        }

        #endregion

        #region INFORMES-CLIENTE

        public IActionResult GenerarInformeCliente(int idCliente)
        {
            byte[] data = _informes.GeneraInforme("/Informes/", "Cliente", new Dictionary<string, string> { { "idCliente", idCliente.ToString() } });
            return File(data, "application/pdf");
        }

        public IActionResult HistorialCliente(int idCliente)
        {
            var historial = _context.DatosHistoricoCliente
                .Where(x => x.IdCliente == idCliente)
                .OrderByDescending(x => x.FechaInicio)
                .ToList();

            var cliente = _gestionClientes.Context.Clientes.FirstOrDefault(c => c.IdCliente == idCliente);
            ViewBag.NombreCliente = cliente?.NombreCliente ?? "Cliente";
            ViewBag.IdCliente = idCliente;

            return View("HistorialCliente", historial);
        }

        public IActionResult GenerarListadoPDFHistorial(int idCliente)
        {
            var historial = _context.DatosHistoricoCliente
                .Where(c => c.IdCliente == idCliente)
                .OrderByDescending(c => c.FechaInicio)
                .ToList();

            if (!historial.Any())
            {
                return NotFound();
            }

            var nombreCliente = historial.First().NombreCliente ?? "Cliente";
            var totalImporte = historial.Sum(x => x.Precio);
            var totalDuracion = historial.Sum(x => x.Duracion);

            var pdfBytes = QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(20);

                    page.Header()
                        .Text($"Informe de servicios de {nombreCliente}")
                        .FontSize(16)
                        .Bold();

                    page.Content().Column(col =>
                    {
                        col.Spacing(10);

                        foreach (var c in historial)
                        {
                            col.Item().Text($"Fecha: {c.FechaInicio:dd/MM/yyyy HH:mm} - {c.FechaFin:dd/MM/yyyy HH:mm}");
                            col.Item().Text($"Profesional: {c.NombreEmpleado}");
                            col.Item().Text($"Sede: {c.NombreSede}");
                            col.Item().Text($"Observaciones: {c.Observaciones}");
                            col.Item().Text($"Servicio: {c.NombreServicio}");
                            col.Item().Text($"Precio: {c.Precio} €");
                            col.Item().Text($"Duración: {c.Duracion} min");
                            col.Item().LineHorizontal(1);
                        }

                        col.Item().LineHorizontal(1);
                        col.Item().Text("TOTALES").Bold();
                        col.Item().Text($"Total Importe: {totalImporte} €");
                        col.Item().Text($"Total Duración: {(totalDuracion / 60.0):0.0} horas");
                    });
                });
            })
            .GeneratePdf();

            return File(pdfBytes, "application/pdf", $"InformeServicios_{nombreCliente}.pdf");
        }

        public IActionResult GenerarPDFTablaHistorial(int idCliente)
        {
            var historial = _context.DatosHistoricoCliente
                .Where(c => c.IdCliente == idCliente)
                .OrderByDescending(c => c.FechaInicio)
                .ToList();

            if (!historial.Any())
            {
                return NotFound();
            }

            var nombreCliente = historial.First().NombreCliente ?? "Cliente";
            var totalImporte = historial.Sum(x => x.Precio);
            var totalDuracion = historial.Sum(x => x.Duracion);

            var pdfBytes = QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(20);

                    page.Header()
                        .Text($"Informe de servicios de {nombreCliente}")
                        .FontSize(16)
                        .Bold();

                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("Inicio");
                            header.Cell().Element(CellStyle).Text("Fin");
                            header.Cell().Element(CellStyle).Text("Profesional");
                            header.Cell().Element(CellStyle).Text("Sede");
                            header.Cell().Element(CellStyle).Text("Observaciones");
                            header.Cell().Element(CellStyle).Text("Servicio");
                            header.Cell().Element(CellStyle).Text("Precio");
                            header.Cell().Element(CellStyle).Text("Duración");
                        });

                        foreach (var c in historial)
                        {
                            table.Cell().Element(CellStyle).Text(c.FechaInicio.ToString("dd/MM/yyyy HH:mm"));
                            table.Cell().Element(CellStyle).Text(c.FechaFin.ToString("dd/MM/yyyy HH:mm"));
                            table.Cell().Element(CellStyle).Text(c.NombreEmpleado ?? "");
                            table.Cell().Element(CellStyle).Text(c.NombreSede ?? "");
                            table.Cell().Element(CellStyle).Text(c.Observaciones ?? "");
                            table.Cell().Element(CellStyle).Text(c.NombreServicio ?? "");
                            table.Cell().Element(CellStyle).Text(c.Precio.ToString("C"));
                            table.Cell().Element(CellStyle).Text(c.Duracion.ToString() + " min");
                        }

                        table.Cell().Element(CellStyle).Text("");
                        table.Cell().Element(CellStyle).Text("");
                        table.Cell().Element(CellStyle).Text("");
                        table.Cell().Element(CellStyle).Text("");
                        table.Cell().Element(CellStyle).Text("");
                        table.Cell().Element(CellStyle).Text("TOTALES");
                        table.Cell().Element(CellStyle).Text(totalImporte.ToString("C"));
                        table.Cell().Element(CellStyle).Text((totalDuracion / 60.0).ToString("0.0") + " h");

                        static IContainer CellStyle(IContainer container)
                        {
                            return container.Padding(5).BorderBottom(1).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten2);
                        }
                    });
                });
            })
            .GeneratePdf();

            return File(pdfBytes, "application/pdf", $"TablaServicios_{nombreCliente}.pdf");
        }

        public IActionResult GenerarTxtHistorial(int idCliente)
        {
            var historial = _context.DatosHistoricoCliente
                .Where(c => c.IdCliente == idCliente)
                .OrderByDescending(c => c.FechaInicio)
                .ToList();

            var nombreCliente = historial.FirstOrDefault()?.NombreCliente ?? "Cliente";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"=================================================");
            sb.AppendLine($"INFORME DE SERVICIOS - CLIENTE: {nombreCliente}");
            sb.AppendLine($"FECHA DE EXPORTACIÓN: {DateTime.Now:dd/MM/yyyy HH:mm}");
            sb.AppendLine($"=================================================\n");

            foreach (var c in historial)
            {
                sb.AppendLine($"Fecha: {c.FechaInicio:dd/MM/yyyy HH:mm} - {c.FechaFin:dd/MM/yyyy HH:mm}");
                sb.AppendLine($"Profesional: {c.NombreEmpleado}");
                sb.AppendLine($"Sede: {c.NombreSede}");
                sb.AppendLine($"Servicio: {c.NombreServicio} | Precio: {c.Precio} € | Duración: {c.Duracion} min");
                sb.AppendLine($"Observaciones: {c.Observaciones}");
                sb.AppendLine(new string('-', 50));
            }

            byte[] bytes = Encoding.UTF8.GetBytes(sb.ToString());
            return File(bytes, "text/plain", $"HistorialServicios_{nombreCliente}.txt");
        }

        public IActionResult GenerarListadoEnZip(int idCliente)
        {
            var fileListado = GenerarListadoPDFHistorial(idCliente) as FileContentResult;
            var fileTabla = GenerarPDFTablaHistorial(idCliente) as FileContentResult;
            var fileTxt = GenerarTxtHistorial(idCliente) as FileContentResult;

            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    if (fileListado != null)
                    {
                        var entry = archive.CreateEntry(fileListado.FileDownloadName, System.IO.Compression.CompressionLevel.Fastest);
                        using var stream = entry.Open();
                        stream.Write(fileListado.FileContents, 0, fileListado.FileContents.Length);
                    }
                    if (fileTabla != null)
                    {
                        var entry = archive.CreateEntry(fileTabla.FileDownloadName, System.IO.Compression.CompressionLevel.Fastest);
                        using var stream = entry.Open();
                        stream.Write(fileTabla.FileContents, 0, fileTabla.FileContents.Length);
                    }
                    if (fileTxt != null)
                    {
                        var entry = archive.CreateEntry(fileTxt.FileDownloadName, System.IO.Compression.CompressionLevel.Fastest);
                        using var stream = entry.Open();
                        stream.Write(fileTxt.FileContents, 0, fileTxt.FileContents.Length);
                    }
                }
                return File(memoryStream.ToArray(), "application/zip", $"DocumentacionCliente_{idCliente}.zip");
            }
        }

        #endregion

        #region ENVIO-CORREO

        [HttpGet, AjaxOnly]
        public async Task<JsonResult> ModalEnvioCorreoCliente(int idCliente)
        {
            ViewFormCorreoElectronico vm = new ViewFormCorreoElectronico
            {
                ModelosCorreo = new SelectList(_context.CorreoElectronico.ToList(), nameof(CorreosElectronicos.IdCorreo), nameof(CorreosElectronicos.NombreCorreo))
            };

            var cliente = _context.Clientes
                .Where(p => p.IdCliente == idCliente)
                .Select(p => new { p.NombreCliente, p.EMail })
                .FirstOrDefault();

            if (cliente != null)
            {
                vm.NombrePaciente = cliente.NombreCliente;
                vm.EmailPaciente = cliente.EMail;
            }

            string data = await _renderService.ToStringAsync("FormEnvioCorreo", vm);
            return new JsonResult(new JsonResponse("200", "Operación realizada correctamente.", data));
        }

        [HttpGet, AjaxOnly]
        public JsonResult GetCorreoInfo(int idCorreo)
        {
            var correo = _context.CorreoElectronico
                .Where(c => c.IdCorreo == idCorreo)
                .Select(c => new { c.IdCorreo, c.Asunto, c.CuerpoMensaje, c.NombreCorreo })
                .FirstOrDefault();

            JsonResponse jsonResponse = new JsonResponse("200", "Ok", JsonSerializer.Serialize(correo));
            return new JsonResult(jsonResponse);
        }

        [HttpPost]
        public JsonResult EnviarCorreoCliente(ViewFormCorreoElectronico vmCorreo)
        {
            JsonResponse jsonResponse = new JsonResponse("200", "Correo enviado correctamente.");

            try
            {
                switch (vmCorreo.ConAdjuntos, vmCorreo.SolicitarRespuesta)
                {
                    case (false, false):
                        _email.EnviarEmailSinAdjunto(vmCorreo.EmailPaciente ?? "", vmCorreo.Asunto ?? "", vmCorreo.CuerpoMensaje ?? "");
                        break;
                    case (true, false):
                        _email.EnviarEmailConAdjunto(vmCorreo.EmailPaciente ?? "", vmCorreo.Asunto ?? "", vmCorreo.CuerpoMensaje ?? "", vmCorreo.InputAdjuntos);
                        break;
                    case (true, true):
                        _email.EnviarEmailConAdjuntoYReply(vmCorreo.EmailPaciente ?? "", vmCorreo.Asunto ?? "", vmCorreo.CuerpoMensaje ?? "", "no_responder@gmail.com", vmCorreo.InputAdjuntos);
                        break;
                }
            }
            catch (Exception ex)
            {
                jsonResponse.Status = "500";
                jsonResponse.StatusMessage = "Error: " + ex.Message;
            }
            return new JsonResult(jsonResponse);
        }

        [HttpPost, AjaxOnly]
        public JsonResult AddModeloCorreo(string nombreCorreo, string asunto, string cuerpoMensaje)
        {
            try
            {
                CorreosElectronicos ce = new CorreosElectronicos
                {
                    NombreCorreo = nombreCorreo,
                    Asunto = asunto,
                    CuerpoMensaje = cuerpoMensaje
                };
                _context.CorreoElectronico.Add(ce);
                _context.SaveChanges();
                return new JsonResult(new JsonResponse("200", "Modelo creado con éxito."));
            }
            catch (Exception ex)
            {
                return new JsonResult(new JsonResponse("500", "Error: " + ex.Message));
            }
        }

        [HttpPost, AjaxOnly]
        public JsonResult EliminarModeloCorreo(int idCorreo)
        {
            try
            {
                var ce = _context.CorreoElectronico.Find(idCorreo);
                if (ce != null)
                {
                    _context.CorreoElectronico.Remove(ce);
                    _context.SaveChanges();
                }
                return new JsonResult(new JsonResponse("200", "Modelo eliminado con éxito."));
            }
            catch (Exception ex)
            {
                return new JsonResult(new JsonResponse("500", "Error: " + ex.Message));
            }
        }

        #endregion

        #region IMPORTACION-EXCEL

        [HttpGet, AjaxOnly]
        public async Task<JsonResult> ModalImportarExcel()
        {
            string data = await _renderService.ToStringAsync("FormImportarClientes", new FormImportarExcel());
            return new JsonResult(new JsonResponse("200", "Operación realizada correctamente.", data));
        }

        [HttpPost, AjaxOnly]
        public async Task<JsonResult> ImportarExcel(IFormFile fileExcel)
        {
            try
            {
                if (fileExcel == null || fileExcel.Length == 0)
                {
                    return new JsonResult(new JsonResponse("400", "No se proporcionó ningún archivo"));
                }

                using var stream = new MemoryStream();
                await fileExcel.CopyToAsync(stream);
                using var package = new ExcelPackage(stream);
                var worksheet = package.Workbook.Worksheets.FirstOrDefault();

                if (worksheet == null)
                {
                    return new JsonResult(new JsonResponse("400", "El archivo Excel está vacío"));
                }

                int rowCount = worksheet.Dimension.Rows;
                List<Cliente> clientesAInsertar = new List<Cliente>();

                for (int row = 2; row <= rowCount; row++)
                {
                    string? nombre = worksheet.Cells[row, 1].Value?.ToString();
                    if (string.IsNullOrWhiteSpace(nombre)) continue;

                    string? dni = worksheet.Cells[row, 2].Value?.ToString() ?? "SIN DNI";
                    int.TryParse(worksheet.Cells[row, 3].Value?.ToString(), out int tel);
                    string? email = worksheet.Cells[row, 4].Value?.ToString();
                    string? direccion = worksheet.Cells[row, 5].Value?.ToString() ?? "Dirección estándar";
                    string? ciudad = worksheet.Cells[row, 6].Value?.ToString();

                    clientesAInsertar.Add(new Cliente
                    {
                        NombreCliente = nombre,
                        DNI = dni,
                        Telefono = tel,
                        EMail = email,
                        Direccion = direccion,
                        Ciudad = ciudad,
                        FechaAlta = DateTime.Now
                    });
                }

                _context.Clientes.AddRange(clientesAInsertar);
                await _context.SaveChangesAsync();

                return new JsonResult(new JsonResponse("200", $"Se han importado {clientesAInsertar.Count} clientes correctamente."));
            }
            catch (Exception ex)
            {
                return new JsonResult(new JsonResponse("500", "Error al importar: " + ex.Message));
            }
        }

        #endregion
    }
}
