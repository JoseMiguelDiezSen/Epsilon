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

    //-------- ((GESTIÓN MÉDICOS)) ---------//

    /* Abrir Detalle Médico en la tabla */
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
            url: '/Medicos/DetalleMedico',
            type: 'GET',
            data: { idMedico: id },
            success: function (res) { $content.html(res); },
            error: function () { $content.html('Error al cargar los detalles del médico.'); }
        });
    });

    /* Filtrar Médicos */
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
                    alert("No se pudo realizar el filtrado de médicos.");
                }
            });
            return false;
        } catch (ex) {
            console.error(ex);
        }
    };

    /* GET: Modal Añadir Médico */
    jqGetModalAddMedico = () => {
        $.ajax({
            type: 'GET',
            url: 'Medicos/ModalAgregarMedico',
            contentType: false,
            processData: false,
            success: function (response) {
                $('#addMedicoModal .modal-body').html(response.data);

                let modal = new bootstrap.Modal(document.getElementById('addMedicoModal'));
                modal.show();

                // Validación con jQuery Validate
                $("#addMedico").validate({
                    ignore: "input[readonly]",
                    rules: {
                        NombreMedico: { required: true },
                        DNI: { required: true },
                        NumeroColegiado: { required: true, digits: true },
                        Especialidad: { required: true },
                        Telefono: { required: true },
                        EMail: { required: true, email: true }
                    },
                    messages: {
                        NombreMedico: { required: "(*) Debe introducir un nombre." },
                        DNI: { required: "(*) Debe introducir un DNI." },
                        NumeroColegiado: { required: "(*) Debe introducir el nº de colegiado.", digits: "(*) Solo números." },
                        Especialidad: { required: "(*) Debe introducir una especialidad." },
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
                alert("No se pudo realizar la operacion");
            }
        });
    };

    /* POST: Añadir Médico */
    jqPostAddMedico = (form) => {
        if (!$(form).valid()) {
            return false;
        }
        try {
            $.ajax({
                type: 'POST',
                url: 'Medicos/AgregarMedico',
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (response) {
                    if (response.StatusCode === 200 || response.statusCode === 200) {
                        const modal = bootstrap.Modal.getInstance(document.getElementById('addMedicoModal'));
                        if (modal) modal.hide();
                        alert("Médico agregado correctamente.");
                        $('#id_Form').submit();
                    } else {
                        alert(response.message || "No se pudo agregar el médico.");
                    }
                },
                error: function () {
                    alert("Ha ocurrido un error al guardar el médico.");
                }
            });
            return false;
        } catch (ex) {
            console.error(ex);
        }
    };

    /* GET: Modal Modificar Médico */
    jqGetModalUpdateMedico = (idMedico) => {
        try {
            $.ajax({
                type: 'GET',
                url: 'Medicos/GetModalModificarMedico',
                data: { idMedico: idMedico },
                success: function (response) {
                    $('#updateMedicoModal .modal-body').html(response.data);

                    let modal = new bootstrap.Modal(document.getElementById('updateMedicoModal'));
                    modal.show();

                    // Validación con jQuery Validate
                    $("#updateMedico").validate({
                        ignore: "input[readonly]",
                        rules: {
                            NombreMedico: { required: true },
                            DNI: { required: true },
                            NumeroColegiado: { required: true, digits: true },
                            Especialidad: { required: true },
                            Telefono: { required: true },
                            EMail: { required: true, email: true }
                        },
                        messages: {
                            NombreMedico: { required: "(*) Debe introducir un nombre." },
                            DNI: { required: "(*) Debe introducir un DNI." },
                            NumeroColegiado: { required: "(*) Debe introducir el nº de colegiado.", digits: "(*) Solo números." },
                            Especialidad: { required: "(*) Debe introducir una especialidad." },
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
                    alert("Error al obtener los datos del médico.");
                }
            });
        } catch (ex) {
            console.error(ex);
        }
    };

    /* POST: Modificar Médico */
    jqPostUpdateMedico = (form) => {
        if (!$(form).valid()) {
            return false;
        }
        try {
            $.ajax({
                type: 'POST',
                url: 'Medicos/ModificarMedico',
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (response) {
                    if (response.StatusCode === 200 || response.statusCode === 200) {
                        const modal = bootstrap.Modal.getInstance(document.getElementById('updateMedicoModal'));
                        if (modal) modal.hide();
                        alert("Médico modificado correctamente.");
                        $('#id_Form').submit();
                    } else {
                        alert(response.message || "No se pudieron guardar las modificaciones del médico.");
                    }
                },
                error: function () {
                    alert("No se han podido modificar los datos del médico.");
                }
            });
            return false;
        } catch (ex) {
            console.error(ex);
        }
    };

    /* GET : Eliminar un medico */
    jqGetModalDeleteMedico = (idMedico) => {
        $.ajax({
            type: "GET",
            url: 'Medicos/EliminarMedico',
            data: { idMedico: idMedico },
            success: function (response) {
                // Inserta la vista en el modal como HTML
                $('#deleteMedicoModal .modal-body').html(response.data);

                // Abre el modal (Bootstrap 5)
                let modal = new bootstrap.Modal(document.getElementById('deleteMedicoModal'));
                modal.show();
            },
            error: function (response) {
                alert("No se pudo realizar la operacion");
            }
        });
    };

    /* POST : Eliminar un medico */
    jqPostDeleteMedico = (idMedico) => {

        var idMedico = $('#IdMedico').val();

        try {
            $.ajax({
                type: 'POST',
                url: "Medicos/EliminarMedico",
                data: { idMedico: idMedico },

                success: function (response) {
                    // Abre el modal (Bootstrap 5)
                    let modal = bootstrap.Modal.getInstance(document.getElementById('deleteMedicoModal')) || new bootstrap.Modal(document.getElementById('deleteMedicoModal'));
                    modal.hide();
                    alert("Médico correctamente eliminado");
                    $('#id_Form').submit();
                },

                error: function (response) {
                    $('#deleteMedicoModal').modal('hide');
                    alert("Ha habido un error al eliminar el médico");
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

});
