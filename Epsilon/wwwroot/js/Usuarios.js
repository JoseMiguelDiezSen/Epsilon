$(document).ready(function () {

    // Paginador tabla
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
                ////$("#addUser").validate({
                ////    ignore: [],

                ////    // Omite los campos readonly
                ////    ignore : "input[readonly]", 
                ////    rules: {

                ////        Nombre: { required: true },
                ////        Email: { required: true, email: true, gmailValido: true },
                ////        Password: { required: true, minlength: 6 },
                ////        Telefono: { required: true, minlength: 9, maxlength: 9, soloNumeros: true }
                ////    },
                ////    messages: {
                ////        Nombre: { required: "(*) Debe introducir un nombre" },
                ////        Email: { required: "(*) Debe introducir un email.", gmailValido: "Introduce un correo válido de Gmail" },
                ////        Password: { required: "(*) Debe introducir una contraseña.", minlength: "Debe tener al menos 6 caracteres." },
                ////        Telefono: { required: "(*) Debe introducir un telefomo.", minlength: "(*) Debe introducir un numero superior (9 cifras)", maxlength: "(*) Debe introducir un numero inferior (9 cifras)", soloNumeros: "(*) Debe introducir solo numeros" },
                ////    },

                ////    errorClass: "is-invalid",
                ////    validClass: "is-valid",


                ////    // Se posicionan debajo de los controles
                ////    errorPlacement: function (error, element) {
                ////        if (element.attr("name") === "Nombre") {
                ////            error.insertAfter("#inputNombre");
                ////        }
                ////        if (element.attr("name") === "Password") {
                ////            error.insertAfter("#inputPassword");
                ////        }
                ////        if (element.attr("name") === "Email") {
                ////            error.insertAfter("#inputCorreo");
                ////        }
                ////        if (element.attr("name") === "Telefono") {
                ////            error.insertAfter("#inputTelefono");
                ////        }

                ////        error.addClass('text-danger').addClass('font-size:6px;'); // Bootstrap style
                ////    }
                ////});

                ////// Regla de validacion personalizada pare el Email
                ////$.validator.addMethod("gmailValido", function (value, element) {
                ////    // Nombre de usuario: 6-20 caracteres, letras, números, . _ -
                ////    return this.optional(element) || /^[a-zA-Z0-9._-]{6,20}@gmail\.com$/.test(value);
                ////}, "Introduce un correo válido de Gmail");

                ////// Validacion personalizada telefono
                ////$.validator.addMethod("soloNumeros", function (value, element) {
                ////    return this.optional(element) || /^[0-9\s\-()+]+$/.test(value);
                ////}, "(*) Introduce solo números");

                $('#idMsg').html(response.data);
            },
            error: function (response) {
                alert("No se pudo realizar la operacion");
            }
        });
    }

    /* POST : Añadir usuario */
    jqPostAddUser = (form) => {

        //if ($('#addUser').valid()) {

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
                            const mensaje = response.errors || "Error desconocido en el servidor.";
                            $("#msgErrores").html(mensaje);
                            $("#divErrores").fadeIn(200);

                            $("#btnCerrarError").off("click").on("click", function () {
                                $("#divErrores").fadeOut(200);
                            });


                            $('#idMsgError').html("No se pudo realizar la operacion");
                            OcultarElemento('idDivMsg');
                            MostrarElemento('idDivMsgError');


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
        //}
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
                    // Abre el modal (Bootstrap 4)
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



    /* Funcion que muestra la snackbar */
    showMessage = () => {
        // Get the snackbar DIV
        var x = document.getElementById("snackbar");

        // Add the "show" class to DIV
        x.className = "show";

        // After 4.5 seconds, remove the show class from DIV
        setTimeout(function () {
            x.className = x.className.replace("show", "");
        }, 4500);
    }



    $('#addUserModal').dragablito({ handle: ".modal-header" });
    $('#updateUserModal').dragablito({ handle: ".modal-header" });
    $("#deleteUserModal").dragablito({ handle: ".modal-header" });
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





///*Funcion de apertura del modal agregar usuario*/







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
