jQuery(function () {

    /* Paginador tabla */
    if (window.PaginadorPrincipal == undefined) {
        $('#RegistrosPaginaActual').val($('#idBodyTable tr').length);
        window.PaginadorPrincipal = jQuery('#idPaginadorPrincipal').paginador({
            Formulario: 'id_Form',
            PaginaActual: 'PaginaActual',
            RegistrosPorPagina: 'RegistrosPorPagina',
            RegistrosPaginaActual: 'RegistrosPaginaActual',
            ClassButtons: 'visually-hidden'
        });
    }

    //-------- ((GESTIÓN PERSONAL / EMPLEADOS)) ---------//

    /* Abrir Detalle Personal en la tabla */
    $(document).on('click', '.js-toggle-detail', function (e) {
        e.preventDefault();

        var id = $(this).data('id');
        if (!id) return;

        var detailSelector = '#detail-' + id;
        var $existing = $(detailSelector);
        var $row = $(this).closest('tr');

        if ($existing.length) {
            $existing.toggle();
            return;
        }

        var $content = $('<div class="detail-content">Cargando detalles...</div>');

        $('<tr id="detail-' + id + '" class="detail-row">' +
            '<td colspan="' + $row.children('td').length + '"></td></tr>')
            .insertAfter($row)
            .find('td')
            .append($content);

        $.ajax({
            url: '/Personal/DetallePersonal',
            type: 'GET',
            data: { idEmpleado: id },
            success: function (res) { $content.html(res); },
            error: function () { $content.html('Error al cargar los detalles del empleado.'); }
        });
    });

    /* Filtrar Personal */
    jqPostFiltrar = (ev, form) => {
        try {
            $.ajax({
                type: "POST",
                url: form.action,
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (response) {
                    $("#idResultadosFiltro").html(response.data === undefined ? response : response.data);
                    $('#RegistrosPaginaActual').val($('#idBodyTable tr').length);

                    if (window.PaginadorPrincipal == undefined) {
                        window.PaginadorPrincipal = $('#idPaginadorPrincipal').paginador({
                            Formulario: 'id_Form',
                            PaginaActual: 'PaginaActual',
                            RegistrosPorPagina: 'RegistrosPorPagina',
                            RegistrosPaginaActual: 'RegistrosPaginaActual',
                            ClassButtons: 'visually-hidden'
                        });
                    } else {
                        window.PaginadorPrincipal.actualiza();
                    }
                },
                error: function () {
                    alert("No se pudo realizar el filtrado de personal.");
                }
            });
            return false;
        } catch (ex) {
            console.error(ex);
        }
    };

    /* GET: Modal Añadir Empleado */
    jqGetModalAddPersonal = () => {
        $.ajax({
            type: 'GET',
            url: '/Personal/ModalAgregarPersonal',
            contentType: false,
            processData: false,
            success: function (response) {
                $('#addPersonalModal .modal-body').html(response.data);

                let modal = new bootstrap.Modal(document.getElementById('addPersonalModal'));
                modal.show();

                $("#addPersonal").validate({
                    ignore: "input[readonly]",
                    rules: {
                        NombreEmpleado: { required: true },
                        DNI: { required: true },
                        NumeroEmpleado: { required: true, digits: true },
                        Puesto: { required: true },
                        Telefono: { required: true },
                        EMail: { required: true, email: true }
                    },
                    messages: {
                        NombreEmpleado: { required: "(*) Debe introducir un nombre." },
                        DNI: { required: "(*) Debe introducir un DNI." },
                        NumeroEmpleado: { required: "(*) Debe introducir el nº de empleado.", digits: "(*) Solo números." },
                        Puesto: { required: "(*) Debe introducir un puesto." },
                        Telefono: { required: "(*) Debe introducir un teléfono." },
                        EMail: { required: "(*) Debe introducir un correo válido.", email: "(*) Formato de email inválido." }
                    },
                    errorClass: "is-invalid",
                    validClass: "is-valid",
                    errorPlacement: function (error, element) {
                        var inputGroup = element.closest('.input-group');
                        if (inputGroup.length) {
                            error.insertAfter(inputGroup);
                        } else {
                            error.insertAfter(element);
                        }
                        error.addClass('text-danger');
                    }
                });
            },
            error: function () {
                alert("No se pudo realizar la operación.");
            }
        });
    };

    /* POST: Añadir Empleado */
    jqPostAddPersonal = (form) => {
        if (!$(form).valid()) {
            return false;
        }
        try {
            $.ajax({
                type: 'POST',
                url: '/Personal/AgregarPersonal',
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (response) {
                    if (response.StatusCode === 200 || response.statusCode === 200) {
                        const modal = bootstrap.Modal.getInstance(document.getElementById('addPersonalModal'));
                        if (modal) modal.hide();
                        alert("Empleado agregado correctamente.");
                        $('#id_Form').submit();
                    } else {
                        alert(response.message || "No se pudo agregar el empleado.");
                    }
                },
                error: function () {
                    alert("Ha ocurrido un error al guardar el empleado.");
                }
            });
            return false;
        } catch (ex) {
            console.error(ex);
        }
    };

    /* GET: Modal Modificar Empleado */
    jqGetModalUpdatePersonal = (idEmpleado) => {
        try {
            $.ajax({
                type: 'GET',
                url: '/Personal/GetModalModificarPersonal',
                data: { idEmpleado: idEmpleado },
                success: function (response) {
                    $('#updatePersonalModal .modal-body').html(response.data);

                    let modal = new bootstrap.Modal(document.getElementById('updatePersonalModal'));
                    modal.show();

                    $("#updatePersonal").validate({
                        ignore: "input[readonly]",
                        rules: {
                            NombreEmpleado: { required: true },
                            DNI: { required: true },
                            NumeroEmpleado: { required: true, digits: true },
                            Puesto: { required: true },
                            Telefono: { required: true },
                            EMail: { required: true, email: true }
                        },
                        messages: {
                            NombreEmpleado: { required: "(*) Debe introducir un nombre." },
                            DNI: { required: "(*) Debe introducir un DNI." },
                            NumeroEmpleado: { required: "(*) Debe introducir el nº de empleado.", digits: "(*) Solo números." },
                            Puesto: { required: "(*) Debe introducir un puesto." },
                            Telefono: { required: "(*) Debe introducir un teléfono." },
                            EMail: { required: "(*) Debe introducir un correo válido.", email: "(*) Formato de email inválido." }
                        },
                        errorClass: "is-invalid",
                        validClass: "is-valid",
                        errorPlacement: function (error, element) {
                            var inputGroup = element.closest('.input-group');
                            if (inputGroup.length) {
                                error.insertAfter(inputGroup);
                            } else {
                                error.insertAfter(element);
                            }
                            error.addClass('text-danger');
                        }
                    });
                },
                error: function () {
                    alert("Error al obtener los datos del empleado.");
                }
            });
        } catch (ex) {
            console.error(ex);
        }
    };

    /* POST: Modificar Empleado */
    jqPostUpdatePersonal = (form) => {
        if (!$(form).valid()) {
            return false;
        }
        try {
            $.ajax({
                type: 'POST',
                url: '/Personal/ModificarPersonal',
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (response) {
                    if (response.StatusCode === 200 || response.statusCode === 200) {
                        const modal = bootstrap.Modal.getInstance(document.getElementById('updatePersonalModal'));
                        if (modal) modal.hide();
                        alert("Empleado modificado correctamente.");
                        $('#id_Form').submit();
                    } else {
                        alert(response.message || "No se pudieron guardar las modificaciones del empleado.");
                    }
                },
                error: function () {
                    alert("No se han podido modificar los datos del empleado.");
                }
            });
            return false;
        } catch (ex) {
            console.error(ex);
        }
    };

    /* GET : Eliminar un empleado */
    jqGetModalDeletePersonal = (idEmpleado) => {
        $.ajax({
            type: "GET",
            url: '/Personal/EliminarPersonalAsync',
            data: { idEmpleado: idEmpleado },
            success: function (response) {
                $('#deletePersonalModal .modal-body').html(response.data);
                let modal = new bootstrap.Modal(document.getElementById('deletePersonalModal'));
                modal.show();
            },
            error: function () {
                alert("No se pudo realizar la operación.");
            }
        });
    };

    /* POST : Eliminar un empleado */
    jqPostDeletePersonal = (idEmpleado) => {
        var id = $('#IdEmpleado').val() || idEmpleado;

        try {
            $.ajax({
                type: 'POST',
                url: "/Personal/ConfirmarEliminarPersonal",
                data: { idEmpleado: id },
                success: function (response) {
                    let modal = bootstrap.Modal.getInstance(document.getElementById('deletePersonalModal')) || new bootstrap.Modal(document.getElementById('deletePersonalModal'));
                    modal.hide();
                    alert("Empleado correctamente eliminado.");
                    $('#id_Form').submit();
                },
                error: function () {
                    $('#deletePersonalModal').modal('hide');
                    alert("Ha habido un error al eliminar el empleado.");
                }
            });
            return false;
        } catch (ex) {
            console.log(ex);
        }
    };

    // Previsualización dinámica de foto al seleccionar archivo en cualquier modal
    document.addEventListener("change", function (e) {
        if (e.target.matches(".inputFoto")) {
            const file = e.target.files[0];
            if (!file) return;
            const form = e.target.closest("form");
            const preview = form.querySelector(".previewFoto");
            if (preview) {
                preview.src = URL.createObjectURL(file);
            }
        }
    });

    if ($.fn.dragablito) {
        $('#updatePersonalModal').dragablito({ handle: ".modal-header" });
        $('#deletePersonalModal').dragablito({ handle: ".modal-header" });
    }
});
