document.addEventListener('DOMContentLoaded', function () {
    var calendarEl = document.getElementById('calendar');

    var calendar = new FullCalendar.Calendar(calendarEl, {

        /* Vista inicial del calendario */
        initialView: 'dayGridMonth',

        /* Configuración de idioma */
        locale: 'es',

        /* Botones del header y su orden */
        headerToolbar: {
            left: 'prevYear,prev,next,nextYear',
            center: 'title',
            right: 'timeGridWeek,dayGridMonth,multiMonthYear,today'
        },

        // Permitir selección y edición de eventos
        selectable: true,

        /* Permitir arrastrar y soltar eventos para editarlos */
        editable: true,

        /* Rango de horas visible en las vistas de tiempo */
        slotMinTime: '08:00:00',
        slotMaxTime: '20:00:00',

        height: 650,

        /* Fecha inicial del calendario */
        initialDate: '2026-04-01',

        /* Eventos de prueba */
        events: [
            { title: 'Evento OK', start: '2026-04-07' },
            { title: 'Otro evento', start: '2026-04-12' },
            {
                title: 'Reunión 1 😎',
                start: '2026-04-15T10:00:00',
                end: '2026-04-15T11:30:00'
            },
            {
                title: 'Reunión 2 😎',
                start: '2026-04-18T18:00:00',
                end: '2026-04-18T18:30:00'
            },
            {
                title: 'Reunión 3 😎',
                start: '2026-04-5T13:00:00',
                end: '2026-04-15T15:30:00'
            }
        ],


        /* Vistas y formato de título */
        views: {
            dayGridMonth: { // name of view
                titleFormat: { year: 'numeric', month: '2-digit', day: '2-digit' }
                // other view-specific options here
                //{ year: 'numeric', month: 'long' }                  // like 'September 2009', for month view
                //{ year: 'numeric', month: 'short', day: 'numeric' } // like 'Sep 13 2009', for week views
                //{ year: 'numeric', month: 'long', day: 'numeric' }  // like 'September 8 2009', for day views
            },
            dayGrid: {
                // options apply to dayGridMonth, dayGridWeek, and dayGridDay views
            },
            timeGrid: {
                // options apply to timeGridWeek and timeGridDay views
            },
            week: {
                // options apply to dayGridWeek and timeGridWeek views
            },
            day: {
                // options apply to dayGridDay and timeGridDay views
            }
        },

        // Nueva Cita
        dateClick: function (info) {

            console.log("Nueva cita en:", info.date);

            // aquí irá tu modal de CREAR
            $.ajax({
                type: 'GET',
                url: 'Agenda/ModalAgregarCita',
                data: { date: info.date.toISOString() },
                contentType: false,
                processData: false,
                success: function (response) {
                    // Inserta la vista en el modal como HTML
                    $('#addCitaModal .modal-body').html(response.data);

                    // Abre el modal (Bootstrap 5)
                    let modal = new bootstrap.Modal(document.getElementById('addCitaModal'));
                    modal.show();
                },
                error: function (response) {
                    alert("No se pudo realizar la operacion");
                }
            });
        },

        //eventClick: function (info) {
        //    console.log(info.event.extendedProps);
        //    // ejemplo: extendedProps que definimos al crear el evento
        //    let tipoAccion = info.event.extendedProps.tipoAccion;

        //    switch (tipoAccion) {
        //        case 'editar':
        //            console.log("Editar cita:", info.event);
        //            alert("Editar Cita");
        //            //abrirModalEditar(info.event);
        //            break;
        //        case 'eliminar':
                 

        //                if (confirm("¿Eliminar esta cita?")) {
        //                    info.event.remove(); // la quita del calendario
        //                    // Opcional: aquí también llamas a tu backend para borrarla de la BBDD
        //                    $.ajax({
        //                        type: 'POST',
        //                        url: 'Agenda/EliminarCita',
        //                        data: { id: info.event.id }, // necesitarás un ID
        //                        success: function (resp) {
        //                            alert("Cita eliminada correctamente");
        //                        },
        //                        error: function () {
        //                            alert("Error al eliminar");
        //                        }
        //                    });
        //                }
                    
        //            break;
        //        case 'mostrar':
        //            mostrarInfo(info.event);
        //            break;
        //        default:
        //            console.log("Acción por defecto", info.event.title);
        //    }







        //}

        // Y Elimnar Cita como lo hacemos??


        eventClick: function (info) {
            console.log("Clic en evento:", info.event);

            // Definimos un tipoAccion temporal para test
            // Por ahora, solo alternamos para pruebas
            let tipoAccion = prompt("Acción para este evento: 'editar' o 'eliminar'", "editar");

            switch (tipoAccion) {
                case 'editar':
                    alert("Editar cita: " + info.event.title);
                    console.log("Editar cita:", info.event);
                    break;
                case 'eliminar':
                    alert("Eliminar cita: " + info.event.title);
                    console.log("Eliminar cita:", info.event);
                    break;
                default:
                    alert("Acción no reconocida");
                    console.log("Acción por defecto:", info.event);
            }
        }

            // DE momento no tocamos esto
            //dateClick: function (info) {
            //    DotNet.invokeMethodAsync('YourAssemblyName', 'OnDateClicked', info.dateStr);
            //}
        });

    calendar.render();
});