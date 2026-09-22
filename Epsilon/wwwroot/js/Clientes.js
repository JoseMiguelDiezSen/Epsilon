jQuery(function () {

    // Paginador tabla
    if (window.PaginadorPrincipal == undefined) {
        $('#RegistrosPaginaActual').val($('#idBodyTable tr').length);
        window.PaginadorPrincipal = jQuery('#idPaginadorPrincipal').paginador({
            Formulario: 'id_Form1',
            PaginaActual: 'PaginaActual',
            RegistrosPorPagina: 'RegistrosPorPagina',
            RegistrosPaginaActual: 'RegistrosPaginaActual',
            ClassButtons: 'visually-hidden'
        });
    }

    //-------- ((GESTION CLIENTES)) ---------//

    /* Abrir Detalle Cliente */
    $(document).on('click', '.js-toggle-detail', function (e) {
        e.preventDefault();

        // ID del cliente desde el botón
        var id = $(this).data('id');
        if (!id) return;

        // Guardar estado en sesión del navegador
        sessionStorage.setItem("detalleClienteAbierto", id);

        // Selector de la fila detalle (si ya existe)
        var detailSelector = '#detail-' + id;
        var $existing = $(detailSelector);

        // Fila actual (cliente)
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
            url: '/Clientes/DetalleCliente',
            type: 'GET',
            data: { idCliente: id },
            success: function (res) { $content.html(res); },
            error: function () { $content.html('Error al cargar detalles.'); }
        });
    });

    // Guardado en sesion del detalle abierto
    $(function () {
        var id = sessionStorage.getItem("detalleClienteAbierto");
        if (!id) return;

        var $row = $('.js-toggle-detail[data-id="' + id + '"]');
        if ($row.length) {
            $row.trigger('click');
        }
    });

    /* Funcion para el filtrado de clientes */
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
                    $('#idMsgError').html(error.message == null ? "No se pudo realizar la operación" : error.message);
                }
            });
            return false;
        } catch (ex) {
            console.log(ex);
        }
    };

    /* GET: Añadir cliente */
    jqGetModalAddCliente = () => {
        $.ajax({
            type: 'GET',
            url: '/Clientes/ModalAgregarCliente',
            contentType: false,
            processData: false,
            success: function (response) {
                $('#addClienteModal .modal-body').html(response.data);

                let modal = new bootstrap.Modal(document.getElementById('addClienteModal'));
                modal.show();

                $("#addCliente").validate({
                    rules: {
                        Email: { required: true, email: true },
                        NombreCliente: { required: true }
                    },
                    messages: {
                        Email: { required: "Se debe introducir un email válido.", email: "Formato incorrecto." },
                        NombreCliente: { required: "Se debe introducir el nombre del cliente." }
                    },
                    errorPlacement: function (error, element) {
                        error.addClass('text-danger');
                        error.insertAfter(element);
                    }
                });
            },
            error: function () {
                alert("No se pudo realizar la operación");
            }
        });
    };

    /* POST: Añadir cliente */
    jqPostAddCliente = (form) => {
        try {
            $.ajax({
                type: 'POST',
                url: '/Clientes/AgregarCliente',
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (response) {
                    let modal = bootstrap.Modal.getInstance(document.getElementById('addClienteModal'));
                    if (modal) modal.hide();
                    alert("Cliente creado correctamente");
                    $('#id_Form1').submit();
                },
                error: function () {
                    alert("Ha ocurrido un error al crear el cliente");
                }
            });
            return false;
        } catch (ex) {
            console.log(ex);
        }
    };

    /* GET : Actualizar un cliente */
    jqGetModalUpdateCliente = (idCliente) => {
        try {
            $.ajax({
                type: 'GET',
                url: '/Clientes/GetModalModificarCliente',
                data: { idCliente: idCliente },
                success: function (response) {
                    $('#updateClienteModal .modal-body').html(response.data);
                    let modal = new bootstrap.Modal(document.getElementById('updateClienteModal'));
                    modal.show();
                },
                error: function () {
                    console.error('Error al obtener el modal de modificación');
                }
            });
        }
        catch (ex) {
            console.error(ex);
        }
    };

    /* POST : Actualizar un cliente */
    jqPostUpdateCliente = (form) => {
        try {
            $.ajax({
                type: "POST",
                url: '/Clientes/ModificarCliente',
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (response) {
                    let modal = bootstrap.Modal.getInstance(document.getElementById('updateClienteModal'));
                    if (modal) modal.hide();
                    alert("Cliente modificado correctamente");
                    $('#id_Form1').submit();
                },
                error: function () {
                    alert("No se han podido modificar los datos del cliente");
                }
            });
            return false;
        }
        catch (ex) {
            console.log(ex);
        }
    };

    /* GET : Eliminar un cliente */
    jqGetModalDeleteCliente = (idCliente) => {
        $.ajax({
            type: "GET",
            url: '/Clientes/EliminarCliente',
            data: { idCliente: idCliente },
            success: function (response) {
                $('#deleteClienteModal .modal-body').html(response.data);
                let modal = new bootstrap.Modal(document.getElementById('deleteClienteModal'));
                modal.show();
            },
            error: function () {
                alert("No se pudo realizar la operación");
            }
        });
    };

    /* POST : Eliminar un cliente */
    jqPostDeleteCliente = (idCliente) => {
        try {
            $.ajax({
                type: 'POST',
                url: "/Clientes/EliminarCliente",
                data: { idCliente: idCliente },
                success: function () {
                    let modal = bootstrap.Modal.getInstance(document.getElementById('deleteClienteModal'));
                    if (modal) modal.hide();
                    alert("Cliente correctamente eliminado");
                    $('#id_Form1').submit();
                },
                error: function () {
                    let modal = bootstrap.Modal.getInstance(document.getElementById('deleteClienteModal'));
                    if (modal) modal.hide();
                    alert("Ha habido un error al eliminar el cliente");
                }
            });
            return false;
        } catch (ex) {
            console.log(ex);
        }
    };

    //-------- ((FUNCIONES DE DETALLE CLIENTE)) ---------//

    jqAbrirHistorialCliente = (idCliente) => {
        window.location.href = '/Clientes/HistorialCliente?idCliente=' + idCliente;
    };

    jqGenerarInformeCliente = (idCliente) => {
        window.open('/Clientes/GenerarInformeCliente?idCliente=' + idCliente, '_blank');
    };

    //-------- ((ENVIO CORREO)) ---------//

    jqGetModalCorreoElectronico = (idCliente) => {
        $.ajax({
            type: 'GET',
            url: '/Clientes/ModalEnvioCorreoCliente',
            data: { idCliente: idCliente },
            success: function (response) {
                $('#modalEnvioCorreo .modal-body').html(response.data);

                const modeloCorreo = document.getElementById("ModeloCorreo");
                if (modeloCorreo) {
                    modeloCorreo.addEventListener("change", function () {
                        const idCorreo = this.value;
                        if (!idCorreo) return;

                        $.ajax({
                            type: 'GET',
                            url: '/Configuracion/GetCorreoInfo',
                            data: { idCorreo: idCorreo },
                            success: function (response) {
                                const datos = JSON.parse(response.data);
                                document.getElementById("asunto").value = datos.Asunto;
                                document.getElementById("cuerpoMensaje").value = datos.CuerpoMensaje;
                                document.getElementById("IdCorreo").value = datos.IdCorreo;
                                document.getElementById("botonEliminarCorreo").style.display = "inline-block";

                                const chkAdjuntos = document.getElementById("chkAdjuntos");
                                const chkRespuesta = document.getElementById("chkRespuesta");
                                const bloque = document.getElementById("bloqueAdjuntos");

                                function actualizarEstadoAdjuntos() {
                                    if (!bloque) return;
                                    bloque.style.display = (chkAdjuntos && chkAdjuntos.checked) ? "block" : "none";
                                }

                                if (chkAdjuntos) {
                                    chkAdjuntos.addEventListener("change", actualizarEstadoAdjuntos);
                                }
                                if (chkRespuesta) {
                                    chkRespuesta.addEventListener("change", function () {
                                        actualizarEstadoAdjuntos();
                                    });
                                }

                                if (chkAdjuntos) chkAdjuntos.checked = false;
                                if (chkRespuesta) chkRespuesta.checked = false;
                                actualizarEstadoAdjuntos();
                            },
                            error: function (xhr, status, error) {
                                console.error("Error obteniendo correo info:", error);
                            }
                        });
                    });
                }

                let modal = new bootstrap.Modal(document.getElementById('modalEnvioCorreo'));
                modal.show();
            },
            error: function (xhr, status, error) {
                console.error("Error cargando modal de correo:", error);
            }
        });
    };

    // ENVIO CORREO
    jqPostEnviarCorreo = (form) => {
        try {
            $.ajax({
                type: 'POST',
                url: '/Clientes/EnviarCorreoCliente',
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function () {
                    alert("Correo enviado correctamente");
                },
                error: function () {
                    alert("Ha ocurrido un error al enviar el correo");
                }
            });
            return false;
        } catch (ex) {
            console.log(ex);
        }
    };

    // ADD MODELO CORREO
    jqAddModeloCorreo = () => {
        const btn = $("#botonAñadirCorreo");
        const nombre = $("#NombreCorreoNuevo").val();
        const asunto = $("#asunto").val();
        const cuerpo = $("#cuerpoMensaje").val();

        $.ajax({
            type: 'POST',
            url: '/Clientes/AddModeloCorreo',
            data: {
                nombreCorreo: nombre,
                asunto: asunto,
                cuerpoMensaje: cuerpo
            },
            success: function () {
                btn.hide();
            },
            error: function (xhr, status, error) {
                console.error("Error al añadir modelo de correo:", error);
            }
        });
    };

    // ELIMINAR MODELO CORREO
    jqPostEliminarModeloCorreo = () => {
        const btn = $("#botonEliminarCorreo");
        const id = $("#ModeloCorreo").val();
        if (!id) return;

        $.ajax({
            type: 'POST',
            url: '/Clientes/EliminarModeloCorreo',
            data: { idCorreo: id },
            success: function () {
                $("#ModeloCorreo option:selected").remove();
                $("#ModeloCorreo").val("");
                $("#asunto").val("");
                $("#cuerpoMensaje").val("");
                btn.hide();
            },
            error: function () {
                console.error("Error al eliminar modelo de correo");
                btn.prop("disabled", false);
            }
        });
    };

    //-------- ((EXCEL CLIENTES)) ---------//

    jqGetModalExportarExcel = () => {
        $.ajax({
            url: "/Clientes/ModalImportarExcel",
            type: "GET",
            contentType: false,
            processData: false,
            success: function (response) {
                $('#importarExcel .modal-body').html(response.data);
                let modal = new bootstrap.Modal(document.getElementById('importarExcel'));
                modal.show();
            },
            error: function () {
                alert("Error al abrir modal de importación.");
            }
        });
    };

    jqPostModalImportarExcel = () => {
        let formData = new FormData();
        let fileInput = document.getElementById("file").files[0];

        if (!fileInput) {
            alert("Selecciona un archivo primero.");
            return;
        }

        formData.append("fileExcel", fileInput);

        $.ajax({
            url: "/Clientes/ImportarExcel",
            type: "POST",
            data: formData,
            contentType: false,
            processData: false,
            success: function () {
                alert("Importación exitosa!");
                $('#id_Form1').submit();
            },
            error: function () {
                alert("Error al importar el archivo.");
            }
        });
    };

    //--------------- ((MODALES ARRASTRABLES)) ------------------//
    if ($.fn.dragablito) {
        $('#modalEnvioCorreo').dragablito({ handle: ".modal-header" });
        $('#updateClienteModal').dragablito({ handle: ".modal-header" });
        $('#deleteClienteModal').dragablito({ handle: ".modal-header" });
    }
});
