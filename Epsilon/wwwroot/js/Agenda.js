/**
 * ============================================================================
 * EPSILON - MÓDULO DE AGENDA Y CALENDARIO (FullCalendar)
 * ============================================================================
 * Manejo de citas médicas con FullCalendar:
 * - Carga dinámica de citas desde el servidor (JSON) con bloques de color sólidos.
 * - Creación de citas mediante modal Bootstrap y llamada AJAX.
 * - Modificación de citas al hacer clic sobre el evento en el calendario (modal).
 * - Eliminación de citas desde el modal o directamente desde el mapa.
 * - Arrastrar (drop) y expandir/comprimir (resize) habilitados con persistencia en BD.
 * - Alertas estándar del navegador (alert / confirm).
 * ============================================================================
 */

// Variable global del calendario para poder invocar refetchEvents desde cualquier función
var calendar = null;

document.addEventListener('DOMContentLoaded', function () {
    var calendarEl = document.getElementById('calendar');

    if (!calendarEl) {
        console.warn("Elemento #calendar no encontrado.");
        return;
    }

    // Inicialización de FullCalendar
    calendar = new FullCalendar.Calendar(calendarEl, {

        // Vista inicial e idioma
        initialView: 'dayGridMonth',
        locale: 'es',

        // Forzar renderizado en bloques de color sólidos en todas las vistas
        eventDisplay: 'block',

        // Barra de herramientas superior
        headerToolbar: {
            left: 'prevYear,prev,next,nextYear today',
            center: 'title',
            right: 'multiMonthYear,dayGridMonth,timeGridWeek,timeGridDay'
        },

        // Textos de los botones de la barra de herramientas
        buttonText: {
            today: 'Hoy',
            month: 'Mes',
            week: 'Semana',
            day: 'Día',
            multiMonthYear: 'Año'
        },

        // Habilitar selección de celdas y edición completa (mover y expandir/comprimir)
        selectable: true,
        editable: true,
        eventStartEditable: true,      // Permite arrastrar la cita a otro día u hora
        eventDurationEditable: true,   // Permite expandir o comprimir la duración de la cita
        eventResizableFromStart: true, // Permite redimensionar desde el inicio o desde el fin

        // Rango de horas visible en vistas de día y semana
        slotMinTime: '08:00:00',
        slotMaxTime: '21:00:00',
        height: 720,

        // Carga dinámica de eventos desde el controlador AgendaController
        events: '/Agenda/GetEventosCalendario',

        /**
         * Renderizado personalizado del contenido de cada evento:
         * Muestra la hora (si existe), el título y un icono de borrado rápido en el mapa.
         */
        eventContent: function (arg) {
            var timeHtml = arg.timeText ? '<span class="fc-event-time-custom">' + arg.timeText + '</span> ' : '';
            var titleHtml = '<span class="fc-event-title-custom">' + arg.event.title + '</span>';
            var btnDelete = '<span class="fc-event-quick-delete" title="Eliminar cita del mapa" onclick="jqGetModalDeleteCita(' + arg.event.id + ', event)"><i class="fa-solid fa-xmark"></i></span>';

            return {
                html: '<div class="fc-event-inner-wrap">' + timeHtml + titleHtml + btnDelete + '</div>'
            };
        },

        /**
         * EVENTO: Clic en una fecha o celda vacía del calendario
         * Abre la ventana modal para registrar una nueva cita en esa fecha/hora.
         */
        dateClick: function (info) {
            jqGetModalAddCita(info.dateStr);
        },

        /**
         * EVENTO: Arrastrar y soltar cita a una nueva fecha u hora
         */
        eventDrop: function (info) {
            jqActualizarFechaCita(info);
        },

        /**
         * EVENTO: Expandir o comprimir la duración de una cita (Resize)
         */
        eventResize: function (info) {
            jqActualizarFechaCita(info);
        },

        /**
         * EVENTO: Clic sobre una cita existente
         * Abre la ventana modal para ver los datos, modificarlos o eliminar la cita.
         */
        eventClick: function (info) {
            jqGetModalModificarCita(info.event.id);
        }
    });

    // Renderizamos el calendario en pantalla
    calendar.render();
});

/**
 * Formatea un objeto Date en formato ISO local (YYYY-MM-DDTHH:mm:ss) sin desfase UTC.
 * @param {Date} date - Fecha a formatear.
 * @returns {string|null} Cadena local con formato ISO.
 */
function formatFechaLocal(date) {
    if (!date) return null;
    var anio = date.getFullYear();
    var mes = String(date.getMonth() + 1).padStart(2, '0');
    var dia = String(date.getDate()).padStart(2, '0');
    var horas = String(date.getHours()).padStart(2, '0');
    var minutos = String(date.getMinutes()).padStart(2, '0');
    var segundos = String(date.getSeconds()).padStart(2, '0');
    return anio + '-' + mes + '-' + dia + 'T' + horas + ':' + minutos + ':' + segundos;
}

/**
 * Abre el modal de agregar cita cargando la vista parcial desde el servidor vía AJAX.
 * @param {string} [fechaStr] - Fecha opcional seleccionada en el calendario.
 */
function jqGetModalAddCita(fechaStr) {
    var params = fechaStr ? { date: fechaStr } : {};

    $.ajax({
        type: 'GET',
        url: '/Agenda/ModalAgregarCita',
        data: params,
        success: function (response) {
            // Inserta la vista parcial FormAddCita en el modal
            $('#addCitaModal .modal-body').html(response.data);

            // Abre el modal de Bootstrap
            var modalEl = document.getElementById('addCitaModal');
            var modal = bootstrap.Modal.getInstance(modalEl) || new bootstrap.Modal(modalEl);
            modal.show();
        },
        error: function () {
            alert("No se pudo cargar el formulario de nueva cita.");
        }
    });
}

/**
 * Abre el modal de modificar/eliminar cita cargando la vista parcial desde el servidor vía AJAX.
 * @param {number|string} idCita - Identificador de la cita a consultar.
 */
function jqGetModalModificarCita(idCita) {
    $.ajax({
        type: 'GET',
        url: '/Agenda/ModalModificarCita',
        data: { idCita: idCita },
        success: function (response) {
            // Inserta la vista parcial FormModificarCita en el modal
            $('#modificarCitaModal .modal-body').html(response.data);

            // Abre el modal de Bootstrap
            var modalEl = document.getElementById('modificarCitaModal');
            var modal = bootstrap.Modal.getInstance(modalEl) || new bootstrap.Modal(modalEl);
            modal.show();
        },
        error: function () {
            alert("No se pudo cargar los detalles de la cita médica.");
        }
    });
}

/**
 * Envía los datos del formulario de creación de cita al servidor vía AJAX (POST).
 * @param {HTMLFormElement} form - Formulario enviado.
 */
function jqPostAddCita(form) {
    try {
        $.ajax({
            type: 'POST',
            url: '/Agenda/AgregarCita',
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (response) {
                if (response && response.statusCode && response.statusCode !== 200) {
                    alert(response.message || "No se pudo guardar la cita.");
                    return;
                }

                // Cerramos la ventana modal
                var modalEl = document.getElementById('addCitaModal');
                var modal = bootstrap.Modal.getInstance(modalEl);
                if (modal) {
                    modal.hide();
                } else {
                    $(modalEl).modal('hide');
                }

                // Refrescamos el calendario
                if (calendar) {
                    calendar.refetchEvents();
                }

                alert("Cita creada correctamente.");
            },
            error: function (xhr) {
                var msg = "Ocurrió un error al guardar la cita médica.";
                if (xhr.responseJSON && xhr.responseJSON.message) {
                    msg = xhr.responseJSON.message;
                }
                alert(msg);
            }
        });
    } catch (ex) {
        console.error("Error en jqPostAddCita:", ex);
    }
    return false; // Evita el submit tradicional
}

/**
 * Envía los datos del formulario de modificación de cita al servidor vía AJAX (POST).
 * @param {HTMLFormElement} form - Formulario enviado con los cambios.
 */
function jqPostModificarCita(form) {
    try {
        $.ajax({
            type: 'POST',
            url: '/Agenda/ModificarCita',
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (response) {
                if (response && response.statusCode && response.statusCode !== 200) {
                    alert(response.message || "No se pudo modificar la cita.");
                    return;
                }

                // Cerramos la ventana modal
                var modalEl = document.getElementById('modificarCitaModal');
                var modal = bootstrap.Modal.getInstance(modalEl);
                if (modal) {
                    modal.hide();
                } else {
                    $(modalEl).modal('hide');
                }

                // Refrescamos el calendario para reflejar los cambios
                if (calendar) {
                    calendar.refetchEvents();
                }

                alert("Cita modificada correctamente.");
            },
            error: function (xhr) {
                var msg = "Ocurrió un error al modificar la cita médica.";
                if (xhr.responseJSON && xhr.responseJSON.message) {
                    msg = xhr.responseJSON.message;
                }
                alert(msg);
            }
        });
    } catch (ex) {
        console.error("Error en jqPostModificarCita:", ex);
    }
    return false; // Evita el submit tradicional
}

/**
 * Abre el modal para confirmar la eliminación de una cita médica (sin alert/confirm).
 * Cierra previamente el modal de modificar cita si estuviera abierto.
 * @param {number|string} idCita - Identificador de la cita a eliminar.
 * @param {Event} [event] - Evento DOM opcional (si se invoca desde el icono del mapa).
 */
function jqGetModalDeleteCita(idCita, event) {
    if (event) {
        event.stopPropagation();
        event.preventDefault();
    }

    // Si el modal de modificar cita estaba abierto, lo cerramos
    var modalModificarEl = document.getElementById('modificarCitaModal');
    if (modalModificarEl) {
        var modalModificar = bootstrap.Modal.getInstance(modalModificarEl);
        if (modalModificar) {
            modalModificar.hide();
        } else {
            $(modalModificarEl).modal('hide');
        }
    }

    $.ajax({
        type: 'GET',
        url: '/Agenda/ModalEliminarCita',
        data: { idCita: idCita },
        success: function (response) {
            // Inserta la vista en el modal como HTML
            $('#deleteCitaModal .modal-body').html(response.data);

            // Abre el modal (Bootstrap 5)
            var modalEl = document.getElementById('deleteCitaModal');
            var modal = bootstrap.Modal.getInstance(modalEl) || new bootstrap.Modal(modalEl);
            modal.show();
        },
        error: function () {
            alert("No se pudo cargar la confirmación de eliminación.");
        }
    });

    return false;
}

/**
 * Envía la petición de eliminación de cita médica tras confirmar en el modal (POST vía AJAX).
 * @param {HTMLFormElement} form - Formulario deleteCitaModalForm enviado.
 */
function jqPostDeleteCita(form) {
    var idCita = form.querySelector('input[name="IdCita"]')?.value;
    if (!idCita) return false;

    $.ajax({
        type: 'POST',
        url: '/Agenda/EliminarCita',
        data: { idCita: idCita },
        success: function (response) {
            // Cerramos el modal de eliminación
            var modalEl = document.getElementById('deleteCitaModal');
            var modal = bootstrap.Modal.getInstance(modalEl);
            if (modal) {
                modal.hide();
            } else {
                $(modalEl).modal('hide');
            }

            // Refrescamos el calendario
            if (calendar) {
                calendar.refetchEvents();
            }

            alert("Cita eliminada correctamente.");
        },
        error: function () {
            alert("No se pudo eliminar la cita médica.");
        }
    });

    return false; // Evita el submit tradicional
}

// Aliases para retrocompatibilidad
function jqPostEliminarCitaDesdeModal(idCita) {
    return jqGetModalDeleteCita(idCita);
}

function jqEliminarCitaDesdeMapa(idCita, event) {
    return jqGetModalDeleteCita(idCita, event);
}

/**
 * Actualiza la fecha y hora de una cita en base de datos al moverla (drop) o redimensionarla (resize) en el calendario.
 * @param {object} info - Objeto del evento de FullCalendar (contiene event y la función revert).
 */
function jqActualizarFechaCita(info) {
    var idCita = info.event.id;
    var fechaInicio = formatFechaLocal(info.event.start);
    var fechaFin = formatFechaLocal(info.event.end);

    $.ajax({
        type: 'POST',
        url: '/Agenda/ActualizarFechaCita',
        data: {
            idCita: idCita,
            fechaInicio: fechaInicio,
            fechaFin: fechaFin
        },
        success: function (response) {
            console.log("Cita actualizada en BD con éxito.");
        },
        error: function () {
            if (info.revert) {
                info.revert();
            }
            alert("No se pudo actualizar la fecha o duración de la cita en el servidor.");
        }
    });
}

/**
 * Filtra los médicos disponibles en el desplegable según la clínica seleccionada.
 * Oculta/inhabilita los optgroups o médicos que no pertenezcan a la clínica elegida.
 * Si el médico que estaba seleccionado no pertenece a la clínica elegida, resetea la selección a vacía.
 * @param {string} formId - Identificador del formulario ('addCitaForm' o 'modificarCitaForm').
 * @param {string|number} idClinica - Identificador de la clínica seleccionada.
 */
function jqFiltrarMedicosPorClinica(formId, idClinica) {
    var form = document.getElementById(formId);
    if (!form) return;

    var selectMedico = form.querySelector('select[name="IdMedico"]');
    if (!selectMedico) return;

    var valorSeleccionado = selectMedico.value;
    var sigueSiendoValido = false;

    // Buscamos los optgroups de médicos por clínica
    var optgroups = selectMedico.querySelectorAll('optgroup');
    if (optgroups && optgroups.length > 0) {
        optgroups.forEach(function (grp) {
            var grpClinica = grp.getAttribute('data-idclinica');
            // Si no hay clínica seleccionada, se muestran todos los grupos
            var mostrar = (!idClinica || idClinica === "" || grpClinica === String(idClinica));

            grp.style.display = mostrar ? "" : "none";
            grp.disabled = !mostrar;

            var options = grp.querySelectorAll('option');
            options.forEach(function (opt) {
                opt.style.display = mostrar ? "" : "none";
                opt.disabled = !mostrar;
                if (mostrar && opt.value === valorSeleccionado) {
                    sigueSiendoValido = true;
                }
            });
        });
    } else {
        // En caso de opciones directas sin optgroup
        var options = selectMedico.querySelectorAll('option');
        options.forEach(function (opt) {
            if (!opt.value) return; // Opción por defecto
            var optClinica = opt.getAttribute('data-clinica');
            var mostrar = (!idClinica || idClinica === "" || optClinica === String(idClinica));

            opt.style.display = mostrar ? "" : "none";
            opt.disabled = !mostrar;
            if (mostrar && opt.value === valorSeleccionado) {
                sigueSiendoValido = true;
            }
        });
    }

    // Si el médico seleccionado ya no es válido para la clínica elegida, resetear
    if (!sigueSiendoValido && valorSeleccionado) {
        selectMedico.value = "";
    }
}

/**
 * Cuando se selecciona un médico, sincroniza automáticamente la clínica correspondiente
 * en el desplegable de Clínicas del formulario y ajusta la visibilidad.
 * @param {string} formId - Identificador del formulario.
 * @param {HTMLSelectElement} selectMedico - Elemento select del médico.
 */
function jqAutoseleccionarClinica(formId, selectMedico) {
    if (!selectMedico) return;

    var selectedOption = selectMedico.options[selectMedico.selectedIndex];
    if (!selectedOption || !selectedOption.value) return;

    var idClinica = selectedOption.getAttribute('data-clinica');
    if (!idClinica) {
        var parentOptgroup = selectedOption.closest('optgroup');
        if (parentOptgroup) {
            idClinica = parentOptgroup.getAttribute('data-idclinica');
        }
    }

    if (idClinica && idClinica !== "0") {
        var form = document.getElementById(formId);
        if (form) {
            var selectClinica = form.querySelector('select[name="IdClinica"]');
            if (selectClinica && selectClinica.value !== String(idClinica)) {
                selectClinica.value = idClinica;
                // Ajustamos los grupos para mostrar solo los médicos de esta clínica
                jqFiltrarMedicosPorClinica(formId, idClinica);
            }
        }
    }
}