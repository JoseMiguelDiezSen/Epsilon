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

    //-------- ((GESTION PACIENTES)) ---------//

    /* Abrir Detalle Paciente */
    $(document).on('click', '.js-toggle-detail', function (e) {
        e.preventDefault();

        // ID del paciente desde el botón
        var id = $(this).data('id');
        if (!id) return;

        // 🔥 guardar estado en sesión del navegador
        sessionStorage.setItem("detallePacienteAbierto", id);

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

    // Guardado en sesion del detalle abierto
    $(function () {

        var id = sessionStorage.getItem("detallePacienteAbierto");

        if (!id) return;

        var $row = $('.js-toggle-detail[data-id="' + id + '"]');

        if ($row.length) {
            $row.trigger('click');
        }
    });

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
                }
            })
            return false;
        } catch (ex) {
            concole.log(ex);
        }
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
                url: 'Pacientes/GetModalModificarPaciente',
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
            url: 'Pacientes/EliminarPaciente',
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
                    alert("Ha habido un error al eliminar el paciente");
                }
            })
            return false;
        } catch (ex) {
            console.log(ex);
        }
    }

    //-------- ((FUNCIONES DE DETALLE PACIENTE)) ---------//

    // Funcion para la apertura del historial de un paciente
    jqAbrirHistorialPaciente = (idPaciente) => {
        window.location.href = '/Pacientes/HistorialPaciente?idPaciente=' + idPaciente;
    }

    // Funcion para la apertura de las radiografias de un paciente
    jqAbrirRadiografiasPaciente = (idPaciente) => {
        window.location.href = '/Pacientes/RadiologiaPaciente?idPaciente=' + idPaciente;
    }

    // Funcion para generar el informe de un paciente
    jqGenerarInformePaciente = (idPaciente) => {
        window.open('/Pacientes/GenerarInformePaciente?idPaciente=' + idPaciente, '_blank');
    }

    //-------- ((ENVIO CORREO)) ---------//

    jqGetModalCorreoElectronico = (idPaciente) => {

        var i = idPaciente;

        $.ajax({
            type: 'GET',
            url: 'Pacientes/ModalEnvioCorreoPaciente',
            data: { idPaciente: i },

            success: function (response) {

                // Colocamos el contenido del modal
                $('#modalEnvioCorreo .modal-body').html(response.data);

                // Preparamos el combo AHORA que el HTML ya existe
                const modeloCorreo = document.getElementById("ModeloCorreo");

                if (modeloCorreo) {
                    modeloCorreo.addEventListener("change", function () {

                        const idCorreo = this.value;
                        if (!idCorreo) return;

                        // Segunda llamada AJAX normal
                        $.ajax({
                            type: 'GET',
                            url: 'Configuracion/GetCorreoInfo',
                            data: { idCorreo: idCorreo },

                            success: function (response) {
                                const datos = JSON.parse(response.data);

                                // Cargamos los datos del modelo seleccionado
                                document.getElementById("asunto").value = datos.Asunto;
                                document.getElementById("cuerpoMensaje").value = datos.CuerpoMensaje;
                                document.getElementById("IdCorreo").value = datos.IdCorreo;
                                document.getElementById("botonEliminarCorreo").style.display = "inline-block";


                                /////////////////////////////////////////////////////////////////////////////

                                const chkAdjuntos = document.getElementById("chkAdjuntos");
                                const chkRespuesta = document.getElementById("chkRespuesta");
                                const bloque = document.getElementById("bloqueAdjuntos");

                                function actualizarEstadoAdjuntos() {

                                    if (!bloque) return;

                                    if (chkAdjuntos && chkAdjuntos.checked) {
                                        bloque.style.display = "block";
                                    } else {
                                        bloque.style.display = "none";
                                    }
                                }

                                if (chkAdjuntos) {
                                    chkAdjuntos.addEventListener("change", actualizarEstadoAdjuntos);
                                }

                                if (chkRespuesta) {
                                    chkRespuesta.addEventListener("change", function () {
                                        // aquí luego meteremos lógica de respuesta si hace falta
                                        actualizarEstadoAdjuntos();
                                    });
                                }

                                // estado inicial limpio
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

                // Mostrar el modal
                let modal = new bootstrap.Modal(document.getElementById('modalEnvioCorreo'));
                modal.show();
            },

            error: function (xhr, status, error) {
                console.error("Error cargando modal:", error);
            }
        });

    };

    // ENVIO CORREO
    jqPostEnviarCorreo = (form) => {

        try {
            $.ajax({
                type: 'POST',
                url: 'Pacientes/EnviarCorreoPaciente',
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (response) {
                    //let modal = new bootstrap.Modal(document.getElementById('addPacienteModal'));
                    //modal.hide();
                    alert("Coreo enviado correctamente");
                },

                error: function (response) {
                    alert("Ha ocurrido un error al enviar el correo");
                }
            })
            return false;
        } catch (ex) {
            console.log(ex);
        }
    }

    // ADD MODELO CORREO
    jqAddModeloCorreo = () => {

        const btn = $("#botonAñadirCorreo");
        const nombre = $("#NombreCorreoNuevo").val();
        const asunto = $("#asunto").val();
        const cuerpo = $("#cuerpoMensaje").val();

        $.ajax({
            type: 'POST',
            url: 'Pacientes/AddModeloCorreo',
            data: {
                nombreCorreo: nombre,
                asunto: asunto,
                cuerpoMensaje: cuerpo
            },
            success: function (response) {

                // Recargar modal con nueva info
                //$('#modalEnvioCorreo .modal-body').html(response.data);
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
            url: 'Pacientes/EliminarModeloCorreo',
            data: { idCorreo: id },

            success: function (response) {

                // 👇 si NO devuelves HTML, no lo uses
                // $('#modalEnvioCorreo .modal-body').html(response.data);

                // 👉 solución simple: limpiar UI
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
    }

    //-------- ((EXCEL PACIENTES)) ---------//

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

    //--------------- ((MODALES ARRASTRABLES)) ------------------//
    $('#modalEnvioCorreo').dragablito({ handle: ".modal-header" });
    $('#updatePacienteModal').dragablito({ handle: ".modal-header" });
    $('#deletePacienteModal').dragablito({ handle: ".modal-header" });
});
