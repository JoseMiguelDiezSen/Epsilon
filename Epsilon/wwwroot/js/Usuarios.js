jQuery(function () {

    /* Paginador tabla*/
    if (window.PaginadorPrincipal == undefined) {
        // Seguramente aqui este el fallo
        $('#RegistrosPaginaActual').val($('#idBodyTable tr').length);
        window.PaginadorPrincipal = jQuery('#idPaginadorPrincipal').paginador({
            Formulario: 'id_Form',
            PaginaActual: 'PaginaActual',
            RegistrosPorPagina: 'RegistrosPorPagina',
            RegistrosPaginaActual: 'RegistrosPaginaActual',
            ClassButtons: 'visually-hidden'
        });
    }


    /* GET : Añadir usuario */
    jqGetModalAddUser1 = (form) => {
        $.ajax({
            type: 'GET',
            url: 'Usuarios/ModalAgregarUsuario',
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (response) {
                // Inserta la vista en el modal como HTML
                $('#addUserModal .modal-body').html(response.data);

                // Abre el modal (Bootstrap 5)
                let modal = new bootstrap.Modal(document.getElementById('addUserModal'));
                modal.show();

                // --- Inserción mínima recomendada para reparsear validación unobtrusive ---
                const $addForm = $('#addUser');
                console.log('jqGetModalAddUser: form encontrado?', $addForm.length);
                if (typeof $.validator !== 'undefined' && typeof $.validator.unobtrusive !== 'undefined') {
                    try {
                        $.validator.unobtrusive.parse($addForm);
                    } catch (ex) {
                        console.warn('unobtrusive.parse fallo:', ex);
                    }
                }
                // ------------------------------------------------------------------------

                // ...el resto de tu inicialización de validación (si la deixas) sigue aquí...
                // (tu código original definía $("#addUser").validate({...}) y métodos personalizados)
                $('#idMsg').html(response.data);
            },
            error: function (response) {
                alert("No se pudo realizar la operacion");
            }
        });
    }

    /* GET : Añadir usuario */
    jqGetModalAddUser = (form) => {
        $.ajax({
            type: 'GET',
            url: 'Usuarios/ModalAgregarUsuario',
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (response) {
                // Inserta la vista en el modal como HTML
                $('#addUserModal .modal-body').html(response.data);

                // Abre el modal (Bootstrap 5)
                let modal = new bootstrap.Modal(document.getElementById('addUserModal'));
                modal.show();

                // Se definen las reglas de validacion
                $("#addUser").validate({
                /*    ignore: [],*/

                    // Omite los campos readonly
                    ignore : "input[readonly]", 
                    rules: {

                        Nombre: { required: true },
                        Email: { required: true, email: true, gmailValido: true },
                        Password: { required: true, minlength: 6 },
                        Telefono: { required: true, minlength: 9, maxlength: 9, soloNumeros: true }
                    },
                    messages: {
                        Nombre: { required: "(*) Debe introducir un nombre" },
                        Email: { required: "(*) Debe introducir un email.", gmailValido: "Introduce un correo válido de Gmail" },
                        Password: { required: "(*) Debe introducir una contraseña.", minlength: "Debe tener al menos 6 caracteres." },
                        Telefono: { required: "(*) Debe introducir un telefomo.", minlength: "(*) Debe introducir un numero superior (9 cifras)", maxlength: "(*) Debe introducir un numero inferior (9 cifras)", soloNumeros: "(*) Debe introducir solo numeros" },
                    },

                    errorClass: "is-invalid",
                    validClass: "is-valid",

                    errorPlacement: function (error, element) {
                        // Inserta después del input-group si existe, si no, después del elemento
                        var inputGroup = element.closest('.input-group');
                        if (inputGroup.length) {
                            error.insertAfter(inputGroup);
                        } else {
                            error.insertAfter(element);
                        }
                        error.addClass('text-danger');
                    }
                });

                // Regla de validacion personalizada pare el Email
                $.validator.addMethod("gmailValido", function (value, element) {
                    return this.optional(element) || /^[a-zA-Z0-9._-]{6,20}@gmail\.com$/.test(value);
                }, "Introduce un correo válido de Gmail");

                // Validacion personalizada telefono
                $.validator.addMethod("soloNumeros", function (value, element) {
                    return this.optional(element) || /^[0-9\s\-()+]+$/.test(value);
                }, "(*) Introduce solo números");

                $('#idMsg').html(response.data);
            },
            error: function (response) {
                alert("No se pudo realizar la operacion");
            }
        });
    }

    /* POST : Añadir usuario */
    jqPostAddUser = (form) => {

        if (!$(form).valid()) {
            return false;
        }
            try {
                $.ajax({
                    type: 'POST',
                    url: 'Usuarios/AgregarUsuario',
                    data: new FormData(form),
                    contentType: false,
                    processData: false,
                    success: function (response) {
                        console.log("Respuesta AJAX:", response);
                        //$('#idMsg').html(response.data);
                     
                        //$('#add-user-modal').modal('hide');
                        //$('#idFiltros_Form').submit();

                        if (response.StatusCode === 200) {
                            $('#divErrores').hide();
                            // Localiza el modal (Bootstrap 5)
                            const modal = bootstrap.Modal.getInstance(document.getElementById('addUserModal'));
                            if (modal) modal.hide();
                            alert("Usuario creado correctamente");
                            showMessage();
                            $('#idMsg').html(response.data);
                            OcultarElemento('idDivMsgError');
                            MostrarElemento('idDivMsg');
                            //var pagina = $('#PaginaActual').val();
                            //PaginadorPrincipal.irPagina(pagina);

                        } else {
                            //const mensaje = response.errors || "Error desconocido en el servidor.";
                            //$("#msgErrores").html(mensaje);
                            //$("#divErrores").fadeIn(200);

                            //$("#btnCerrarError").off("click").on("click", function () {
                            //    $("#divErrores").fadeOut(200);
                            //});


                            //$('#idMsgError').html("Ha ocurrido un error en la operacion de Agregar un usuario");
                            //OcultarElemento('idDivMsg');
                            //MostrarElemento('idDivMsgError');

                            const modal = bootstrap.Modal.getInstance(document.getElementById('addUserModal'));
                            if (modal) modal.hide();
                            alert("Usuario creado correctamente");
                            showMessage();
                            $('#idMsg').html(response.data);
                        }                
                    },
                    error: function (response) {
                        //$('#idMsgError').html(response.data);
                        //OcultarElemento('idDivMsg');
                        //MostrarElemento('idDivMsgError');
                        //$('id_add_user_form').modal('hide');
                        //var pagina = $('#PaginaActual').val();
                        //PaginadorPrincipal.irPagina(pagina);
                        //idResultadosFiltro
                        $('#idMsgError').html(response.data);
                        OcultarElemento('idDivMsg');
                        MostrarElemento('idDivMsgError');
                        alert("Ha ocurrido un error al crear el usuario");
                    }
                })
                return false;
            } catch (ex) {
                console.log(ex);
            }
        
    }

    /* GET : Actualizar un usuario */
    jqGetModalUpdateUser = (idUsuario) => {
        try {

            $.ajax({
                type: 'GET',
                url: 'Usuarios/ModalModificarUsuario',
                data: { idUsuario: idUsuario },
                success: function (response) {
                    // Inserta la vista en el modal como HTML
                    $('#updateUserModal .modal-body').html(response.data);
                    // (Bootstrap 4)
                    //$('#updateUserModal').modal('show');

                    // Abre el modal (Bootstrap 5)
                    let modal = new bootstrap.Modal(document.getElementById('updateUserModal'));
                    modal.show();

                    // Se definen las reglas de validacion
                    $("#updateUser").validate({
                        ignore: [],
                        // Omite los campos readonly
                        ignore: "input[readonly]",
                        rules: {
                            Nombre: { required: true },
                            Email: { required: true, email: true, gmailValido: true },
                            Password: { required: true, minlength: 6 },
                            Telefono: { required: true, minlength: 9, maxlength: 9, soloNumeros: true }
                        },
                        messages: {
                            Nombre: { required: "(*) Debe introducir un nombre" },
                            Email: { required: "(*) Debe introducir un email.", gmailValido: "Introduce un correo válido de Gmail" },
                            Password: { required: "(*) Debe introducir una contraseña.", minlength: "Debe tener al menos 6 caracteres." },
                            Telefono: { required: "(*) Debe introducir un telefomo.", minlength: "(*) Debe introducir un numero superior (9 cifras)", maxlength: "(*) Debe introducir un numero inferior (9 cifras)", soloNumeros: "(*) Debe introducir solo numeros" },
                        },

                        errorClass: "is-invalid",
                        validClass: "is-valid",

                        // Se posicionan debajo de los controles
                        errorPlacement: function (error, element) {
                            if (element.attr("name") === "Nombre") {
                                error.insertAfter("#inputNombre");
                            }
                            if (element.attr("name") === "Password") {
                                error.insertAfter("#inputPassword");
                            }
                            if (element.attr("name") === "Email") {
                                error.insertAfter("#inputCorreo");
                            }
                            if (element.attr("name") === "Telefono") {
                                error.insertAfter("#inputTelefono");
                            }
                            error.addClass('text-danger').addClass('font-size:6px;'); // Bootstrap style
                        }
                    });

                    // Regla de validacion personalizada pare el Email
                    $.validator.addMethod("gmailValido", function (value, element) {
                        // Nombre de usuario: 6-20 caracteres, letras, números, . _ -
                        return this.optional(element) || /^[a-zA-Z0-9._-]{6,20}@gmail\.com$/.test(value);
                    }, "Introduce un correo válido de Gmail");

                    // Validacion personalizada telefono
                    $.validator.addMethod("soloNumeros", function (value, element) {
                        return this.optional(element) || /^[0-9\s\-()+]+$/.test(value);
                    }, "(*) Introduce solo números");
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

    /* POST : Actualizar un usuario */
    jqPostUpdateUser = (form) => {
        if ($("#updateUser").valid()) {
            try {
                $.ajax({
                    type: 'POST',
                    url: '/Usuarios/ModificarUsuario',
                    data: new FormData(form),
                    contentType: false,
                    processData: false,
                    success: function (response) {

                        // Inserta la vista en el modal como HTML
                        $('#updateUserModal .modal-body').html(response.data);
                        // Abre el modal (Bootstrap 4)
                        //$('#updateUserModal').modal('show');

                        // Abre el modal (Bootstrap 5)
                        let modal = new bootstrap.Modal(document.getElementById('updateUserModal'));
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
    }

    /* GET : Eliminar un usuario */
    jqGetModalDeleteUser = (idUsuario) => {
        $.ajax({
            type: "GET",
            url: 'Usuarios/EliminarUsuario',
            data: { idUsuario: idUsuario },
            success: function (response) {
                //document.querySelector(".modal-title").textContent = "Eliminar Usuario";
                // Inserta la vista en el modal como HTML
                $('#deleteUserModal .modal-body').html(response.data);
                // Abre el modal (Bootstrap 4)
                //$('#updateUserModal').modal('show');


                // Abre el modal (Bootstrap 5)
                let modal = new bootstrap.Modal(document.getElementById('deleteUserModal'));
                modal.show();
            },
            error: function (response) {
                alert("No se pudo realizar la operacion");
            }
        })
    }

    /* POST : Eliminar un usuario */
    jqPostDeleteUser = (idUsuario) => {

        var idUsuario = $('#IdUsuario').val();

        try {
            $.ajax({
                type: 'POST',
                url: "Usuarios/EliminarUsuario",
                data: { idUsuario: idUsuario },

                success: function (response) {
                    //$('#idMsg').html(response.data);
                    //OcultarElemento('idDivMsgError');
                    //MostrarElemento('idDivMsg');
                    //$('#idFiltros_Form').submit();

                    //var pagina = $('#PaginaActual').val();
                    //PaginadorPrincipal.irPagina(pagina);

                    // Abre el modal (Bootstrap 5)
                    let modal = new bootstrap.Modal(document.getElementById('deleteUserModal'));
                    modal.hide();
                    alert("Usuario correctamente eliminado");
                },

                error: function (response) {
                    //$('#idMsgError').html(response.data);
                    //OcultarElemento('idDivMsg');
                    //MostrarElemento('idDivMsgError');

                    $('id_add_user_form').modal('hide');

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


    jqGetImportarUsuarios = (form) => {
        $.ajax({
            url: "/Usuarios/ModalImportarUsuarios",
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

    jqPostImportarUsuarios = (form) => {
        let formData = new FormData();
        let fileInput = document.getElementById("file").files[0];

        if (!fileInput) {
            alert("Selecciona un archivo primero.");
            return;
        }

        formData.append("fileExcel", fileInput);

        $.ajax({
            url: "/Usuarios/ImportarUsuarios",
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

    verDetalleUsuario = () => {

    }

    jqGetModalExportarExcel = () => {

        $.ajax({
            url: "/Pacientes/ModalImportarExcel",
            type: "GET",

            contentType: false,
            processData: false,
            success: function(response) {
                // Inserta la vista en el modal como HTML
                $('#importarExcel .modal-body').html(response.data);
                // Abre el modal (Bootstrap 5)
                let modal = new bootstrap.Modal(document.getElementById('importarExcel'));
                modal.show();
            },
            error: function() {
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
            success: function(response) {
                alert("Importación exitosa!");
            },
            error: function() {
                alert("Error al importar el archivo.");
            }
        })
    }




    // Funcion que contiene la logica para abrir el detalle de un usuario y mostrarlo debajo de la fila correspondiente
    $(document).on('click', '.js-toggle-detail', function (e) {
        e.preventDefault();

        var $btn = $(this);
        var id = $btn.data('id');
        if (id === undefined || id === null) {
            console.warn('Botón detalle sin data-id');
            return;
        }

        var detailId = 'detail-' + id;
        var $existing = $('#' + detailId);

        var $row = $btn.closest('tr');
        if ($existing.length) {
            // Alterna la visibilidad con un efecto suave
            $existing.toggle(); // si necesita animación, usar slideToggle()
            return;
        }

        // Crea una fila de detalle justo después de la fila actual
        var colspan = Math.max(1, $row.children('td').length);
        var $tr = $('<tr/>', { id: detailId, 'class': 'detail-row' });
        var $td = $('<td/>', { colspan: colspan });
        var $content = $('<div/>', { 'class': 'detail-content' }).text('Cargando detalles...');

        $td.append($content);
        $tr.append($td);
        $row.after($tr);

        // Intentar cargar contenido rich vía AJAX — endpoint opcional
        $.ajax({
            url: '/Usuarios/DetalleUsuario',
            type: 'GET',
            data: { idUsuario: id },
            success: function (response) {
                // Si el controlador devuelve `response.data` o HTML directo
                var html = response && response.data ? response.data : (response || '');
                $content.html(html);
            },
            error: function () {
                // Si no existe el endpoint o falla, mostrar un fallback
                $content.html('<div style="padding:8px;color:#f8f9fa;">No se pudieron cargar los detalles.</div>');
            }
        });
    });

    //Funcion que contiene la logica para mostrar el mensaje de exito al crear un usuario, se muestra durante 4.5 segundos y luego desaparece
    showMessage = () => {
        // Se obtiene el DIV que actúa como snackbar
        var x = document.getElementById("snackbar");

        // Se añade la clase "show" para mostrar el DIV
        x.className = "show";

        // Despues de 4.5 segundos, se elimina la clase "show" para ocultar el DIV
        setTimeout(function () {
            x.className = x.className.replace("show", "");
        }, 4500);
    }

    // Hacemos los modales arrastrables usando la librería Dragablito
    $('#addUserModal').dragablito({ handle: ".modal-header" });
    $('#updateUserModal').dragablito({ handle: ".modal-header" });
    $("#deleteUserModal").dragablito({ handle: ".modal-header" });

    // Funcion que contiene la logica para mostrar o ocultar la contraseña al hacer click en el icono del ojo
    document.addEventListener("click", function (e) {

        const btn = e.target.closest(".togglePassword");
        if (!btn) return;

        const inputGroup = btn.closest(".input-group");
        const passwordInput = inputGroup.querySelector("input");
        const icon = btn.querySelector("i");

        if (passwordInput.type === "password") {
            passwordInput.type = "text";
            icon.classList.replace("fa-eye", "fa-eye-slash");
        } else {
            passwordInput.type = "password";
            icon.classList.replace("fa-eye-slash", "fa-eye");
        }

    });

    // Funcion que contiene la logica para el cambio de foto en los modales
    document.addEventListener("change", function (e) {

        if (e.target.matches(".inputFoto")) {

            const file = e.target.files[0];

            if (!file) return;

            const form = e.target.closest("form");

            const preview = form.querySelector(".previewFoto");

            preview.src = URL.createObjectURL(file);
        }

    });

});


// Added safe binding of navigator.permissions.query to avoid Illegal invocation
(function(){
  if (typeof navigator !== 'undefined' && navigator.permissions && typeof navigator.permissions.query === 'function') {
    // ensure we always call with correct receiver
    window.permissionsQuery = navigator.permissions.query.bind(navigator.permissions);
  }
})();