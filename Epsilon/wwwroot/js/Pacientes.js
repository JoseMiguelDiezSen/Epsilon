jQuery(function () {

    // Paginador tabla
    if (window.PaginadorPrincipal == undefined) {
        // Seguramente aqui este el fallo
        $('#RegistrosPaginaActual').val($('#idBodyTable tr').length);
        window.PaginadorPrincipal = jQuery('#idPaginadorPrincipal').paginador({
            Formulario: 'id_Form1',
            PaginaActual: 'PaginaActual',
            RegistrosPorPagina: 'RegistrosPorPagina',
            RegistrosPaginaActual: 'RegistrosPaginaActual',
            ClassButtons: 'visually-hidden'
        });
    }

    /* GET: Añadir paciente */
    jqGetModalAddPaciente = () => {
        $.ajax({
            type: 'GET',
            url: 'Pacientes/ModalAgregarPaciente',

            contentType: false,
            processData: false,
            success: function (response) {
                // Inserta la vista en el modal como HTML
                $('#addPacienteModal .modal-body').html(response.data);

                // Abre el modal (Bootstrap 5)
                let modal = new bootstrap.Modal(document.getElementById('addPacienteModal'));
                modal.show();
                $("#addPaciente").validate({
                    rules: {
                        Email: { required: true, email: true },
                        Password: { required: true, minlength: 3 }
                    },
                    messages: {
                        Email: { required: "Se debe introducir un email válido.", email: "Formato incorrecto." },
                        Password: { required: "Se debe introducir una contraseña.", minlength: "Debe tener al menos 3 caracteres." },

                    },
                    errorPlacement: function (error, element) {
                        error.addClass('text-danger'); // Bootstrap style
                        error.insertAfter(element);    // ⬅️ Esto lo pone debajo del input
                    }
                });
            },
            error: function (response) {
                alert("No se pudo realizar la operacion");
            }
        });
    }

    /* POST: Añadir paciente */
    jqPostAddPaciente = (form) => {
        try {
            $.ajax({
                type: 'POST',
                url: 'Pacientes/AgregarPaciente',
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (response) {
                    let modal = new bootstrap.Modal(document.getElementById('addPacienteModal'));
                    modal.hide();
                    alert("Paciente creado correctamente");
                },

                error: function (response) {
                    //$('#idMsgError').html(response.data);
                    //OcultarElemento('idDivMsg');
                    //MostrarElemento('idDivMsgError');
                    //$('id_add_user_form').modal('hide');
                    //var pagina = $('#PaginaActual').val();
                    //PaginadorPrincipal.irPagina(pagina);
                    //idResultadosFiltro
                    alert("Ha ocurrido un error");
                }
            })
            return false;
        } catch (ex) {
            console.log(ex);
        }
    }

    /* GET : Actualizar un paciente */
    jqGetModalUpdatePaciente = (idPaciente) => {
        try {

            $.ajax({
                type: 'GET',
                url: 'Pacientes/ModalModificarPaciente',
                data: { idPaciente: idPaciente },

                success: function (response) {

                    // Inserta la vista en el modal como HTML
                    $('#updatePacienteModal .modal-body').html(response.data);
                    // Abre el modal (Bootstrap 4)
                    //$('#updateUserModal').modal('show');

                    // Abre el modal (Bootstrap 5)
                    let modal = new bootstrap.Modal(document.getElementById('updatePacienteModal'));
                    modal.show();
                },
                error: function () {
                    console.error('Error al obtener el modal');
                }
            })
        }
        catch (ex) {
            console.error(ex);
        }
    }

    /* POST : Actualizar un paciente */
    jqPostUpdatePaciente = (form) => {
        try {
            $.ajax({
                type: "POST",
                url: '/Pacientes/ModificarPaciente',
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (response) {
                    $('#updatePacienteModal .modal-body').html(response.data);

                    let modal = new bootstrap.Modal(document.getElementById('updatePacienteModal'));
                    modal.hide();
                    alert("Usuario modificado correctamente");
                },
                error: function () {
                    alert("No se han podido modificar los datos del usuario");
                }
            })
        }
        catch (ex) {
        }
    }

    /* GET : Eliminar un paciente */
    jqGetModalDeletePaciente = (idPaciente) => {
        $.ajax({
            type: "GET",
            url: 'Pacientes/EliminarUsuario',
            data: { idPaciente: idPaciente },
            success: function (response) {
                //document.querySelector(".modal-title").textContent = "Eliminar Usuario";
                // Inserta la vista en el modal como HTML
                $('#deletePacienteModal .modal-body').html(response.data);
                // Abre el modal (Bootstrap 4)
                //$('#updateUserModal').modal('show');

                // Abre el modal (Bootstrap 5)
                let modal = new bootstrap.Modal(document.getElementById('deletePacienteModal'));
                modal.show();
            },
            error: function (response) {
                alert("No se pudo realizar la operacion");
            }
        })
    }

    /* POST : Eliminar un usuario */
    jqPostDeletePaciente = (idPaciente) => {
        try {
            $.ajax({
                type: 'POST',
                url: "Pacientes/EliminarPaciente",
                data: { idPaciente: idPaciente },

                success: function (response) {
                    //$('#idMsg').html(response.data);
                    //OcultarElemento('idDivMsgError');
                    //MostrarElemento('idDivMsg');

                    //$('#add-user-modal').modal('hide');
                    //$('#idFiltros_Form').submit();

                    //var pagina = $('#PaginaActual').val();
                    //PaginadorPrincipal.irPagina(pagina);
                    alert("Paciente correctamente eliminado");
                },

                error: function (response) {
                    // Abre el modal (Bootstrap 5)
                    let modal = new bootstrap.Modal(document.getElementById('deletePacienteModal'));
                    modal.hide();
                    //$('#idMsgError').html(response.data);
                    //OcultarElemento('idDivMsg');
                    //MostrarElemento('idDivMsgError');

                    //$('id_add_user_form').modal('hide');

                    //var pagina = $('#PaginaActual').val();
                    //PaginadorPrincipal.irPagina(pagina);
                    alert("Ha habido un error al eliminar el usuario");
                }
            })
            return false;
        } catch (ex) {
            console.log(ex);
        }
    }

    /* Funcion para el filtrado de usuarios */
    jqPostFiltrar = (ev, form) => {

        console.log('PaginaActual:', $('#PaginaActual').val());

        try {
            $.ajax({
                type: "POST",
                url: form.action,
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (response) {
                    $("#idResultadosFiltro").html(response.data === "undefined" ? response : response.data);
                    $('#RegistrosPaginaActual').val($('#idBodyTable tr').length);

                    if (window.PaginadorPrincipal == undefined) {
                        window.PaginadorPrincipal = $('#idPaginadorPrincipal').paginador({
                            Formulario: 'id_Form1',
                            PaginaActual: 'PaginaActual',
                            RegistrosPorPagina: 'RegistrosPorPagina',
                            RegistrosPaginaActual: 'RegistrosPaginaActual',
                            ClassButtons: 'visually-hidden'
                        });
                    } else {
                        window.PaginadorPrincipal.actualiza();
                    }
                },
                error: function (error) {
                    $('#idMsgError').html(err.message == null ? "No se pudo realizar la operacion" : error.message);
                    OcultarElemento('idDivMsg');
                    MostrarElemento('idDivMsgError');
                }
            })
            return false;
        } catch (ex) {
            concole.log(ex);
        }
    }

    jqAbrirHistorialPaciente = (idPaciente) => {
        window.location.href = '/Pacientes/HistorialPaciente?idPaciente=' + idPaciente;
    }

    jqAbrirRadiografiasPaciente = (idPaciente) => {
        window.location.href = '/Radiologia/Index?idPaciente=' + idPaciente;
    }

    jqGetModalExportarExcel = () => {

        $.ajax({
            url: "/Pacientes/ModalImportarExcel",
            type: "GET",

            contentType: false,
            processData: false,
            success: function (response) {
                // Inserta la vista en el modal como HTML
                $('#importarExcel .modal-body').html(response.data);
                // Abre el modal (Bootstrap 5)
                let modal = new bootstrap.Modal(document.getElementById('importarExcel'));
                modal.show();
            },
            error: function () {
                alert("Error al importar el archivo.");
            }
        })
    }


    jqPostModalImportarExcel = (form) => {

        let formData = new FormData();
        let fileInput = document.getElementById("file").files[0];

        if (!fileInput) {
            alert("Selecciona un archivo primero.");
            return;
        }

        formData.append("fileExcel", fileInput);

        $.ajax({
            url: "/Pacientes/ImportarExcel",
            type: "POST",
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                alert("Importación exitosa!");
            },
            error: function () {
                alert("Error al importar el archivo.");
            }
        })
    }



    $('#updatePacienteModal').dragablito({ handle: ".modal-header" });

    $(document).on('click', '.js-toggle-detail', function (e) {
        e.preventDefault();

        // ID del paciente desde el botón
        var id = $(this).data('id');
        if (!id) return;

        // Selector de la fila detalle (si ya existe)
        var detailSelector = '#detail-' + id;
        var $existing = $(detailSelector);

        // Fila actual (paciente)
        var $row = $(this).closest('tr');

        // Si ya existe → solo mostrar/ocultar
        if ($existing.length) {
            $existing.toggle();
            return;
        }

        // Contenedor donde se cargará la vista parcial
        var $content = $('<div class="detail-content">Cargando detalles...</div>');

        // Crear nueva fila debajo con colspan completo
        $('<tr id="detail-' + id + '" class="detail-row">' +
            '<td colspan="' + $row.children('td').length + '"></td></tr>')
            .insertAfter($row)
            .find('td')
            .append($content);

        // Llamada al servidor → devuelve la vista parcial (HTML)
        $.ajax({
            url: '/Pacientes/DetallePaciente',
            type: 'GET',
            data: { idPaciente: id },
            success: function (res) { $content.html(res); },
            error: function () { $content.html('Error al cargar'); }
        });
    });

    //$(document).on('click', '.js-toggle-detail', function (e) {
    //    e.preventDefault();

    //    var $btn = $(this);
    //    var id = $btn.data('id');
    //    if (id === undefined || id === null) {
    //        console.warn('Botón detalle sin data-id');
    //        return;
    //    }

    //    var detailId = 'detail-' + id;
    //    var $existing = $('#' + detailId);

    //    var $row = $btn.closest('tr');
    //    if ($existing.length) {
    //        // Alterna la visibilidad con un efecto suave
    //        $existing.toggle(); // si necesita animación, usar slideToggle()
    //        return;
    //    }

    //    // Crea una fila de detalle justo después de la fila actual
    //    var colspan = Math.max(1, $row.children('td').length);
    //    var $tr = $('<tr/>', { id: detailId, 'class': 'detail-row' });
    //    var $td = $('<td/>', { colspan: colspan });
    //    var $content = $('<div/>', { 'class': 'detail-content' }).text('Cargando detalles...');

    //    $td.append($content);
    //    $tr.append($td);
    //    $row.after($tr);

    //    // Intentar cargar contenido rich vía AJAX — endpoint opcional
    //    $.ajax({
    //        url: '/Pacientes/DetallePaciente',
    //        data: { idPaciente: id },
    //        type: 'GET',
    //        success: function (response) {
    //            // Si el controlador devuelve `response.data` o HTML directo
    //            $content.html(response);
    //        },
    //        error: function () {
    //            // Si no existe el endpoint o falla, mostrar un fallback
    //            $content.html('<div style="padding:8px;color:#f8f9fa;">No se pudieron cargar los detalles.</div>');
    //        }
    //    });
    //});
    //MostrarGrid = () => {

    //    if ($('#gridEdiciones').is(':hidden')) {
    //        $("#gridEdiciones").show();
    //        $("#gridEdiciones").slideDown(700);
    //        $("#spnBtn").removeClass('fa fa-chevron-down').addClass('fa fa-chevron-up');
    //    }
    //    else if ($('#gridEdiciones').is(':visible')) {
    //        $("#gridEdiciones").slideUp(700);
    //        $("#spnBtn").removeClass('fa fa-chevron-up').addClass('fa fa-chevron-down');
    //    }
    //}
});
//jqCheckAddPeriodo = () => {

//    const date = new Date();
//    var day = date.getDate();
//    var month = date.getMonth();
//    var year = date.getFullYear();
//    var currentDate = day + "/" + month + "/" + year;

//    var d1 = dateFrom.slit("/");
//    var d1 = dateTo.slit("/");
//    var c = dateCheck.slit("/");

//    var from = new Date(d1);
//    var to = new Date(d2);
//    var check = new Date(c);

//    if (check > from && check < to) {
//        $("#addUser").show();
//    }
//    else {
//        $("#addUser").hide();
//    }
//}


//if (window.PaginadorPrincipal == undefined) {
//    $('#RegistrosPaginaActual').val($('#idBodyTable tr').length);
//    window.PaginadorPrincipal = jQuery('#idPaginadorPrincipal').paginador({
//        Formulario: 'id_Form',
//        PaginaActual: 'PaginaActual',
//        RegistrosPorPagina: 'RegistrosPorPagina',
//        RegistrosPaginaActual: 'RegistrosPaginaActual',
//        ClassButtons: 'visually-hidden'
//    });
//}




/* Actualizacion detalles curso */
//jqAJAXGet('EjecucionEdiciones/ActualizarDetallesCurso', { 'IdCurso': Curso.IdCurso, IdEdicion: Curso.IdEdicion }
//    , { not_show_popup: true },
//    (data) => {
//        // Cursos
//        $("#DetallesCursoEjecutado").empty();
//        $("#DetallesCursoEjecutado").append(data);
//    },
//    (data) => {
//        deferred.reject('Error al actualizar los datos');
//    },
//    (res) => {
//        deferred.reject('Error al conectar con el servidor');
//    }
//);
