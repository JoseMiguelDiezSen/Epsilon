$(document).ready(function () {

    jqGetModalAddUser = (form) => {
        $.ajax({
            type: 'GET',
            url: 'Usuarios/ModalAgregarUsuario',
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (response) {
                console.log(document.querySelector("#addUserModal .modal-body"));
                // Inserta la vista en el modal como HTML
                $('#addUserModal .modal-body').html(response.data);

                // Abre el modal (Bootstrap 5)
                let modal = new bootstrap.Modal(document.getElementById('addUserModal'));
                modal.show();

                $("#addUser1").validate({
                    rules: {
                        EMail: { required: true, email: true },
                        Password: { required: true, minlength: 6 }
                    },
                    messages: {
                        EMail: { required: "Se debe introducir un email válido.", email: "Formato incorrecto." },
                        Password: { required: "Se debe introducir una contraseña.", minlength: "Debe tener al menos 6 caracteres." }
                    }
                });
            },

            error: function (response) {
                alert("No se pudo realizar la operacion");
            }
        });
    }

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

    /* Funcion para el filtrado de usuarios */
    //jqPostFiltrar = (ev, form) => {
    //    try {
    //        $.ajax({
    //            type: "POST",
    //            url: form.action,
    //            data: new FormData(form),
    //            contentType: false,
    //            processData: false,
    //            success: function (response) {
    //                $("#idResultadosFiltro").html(response.data === "undefined" ? response : response.data);
    //                $('#RegistrosPaginaActual').val($('#idBodyTable tr').length);

    //                if (window.PaginadorPrincipal == undefined) {
    //                    window.PaginadorPrincipal = $('#idPaginadorPrincipal').paginador({
    //                        Formulario: 'id_Form',
    //                        PaginaActual: 'PaginaActual',
    //                        RegistrosPorPagina: 'RegistrosPorPagina',
    //                        RegistrosPaginaActual: 'RegistrosPaginaActual',
    //                        ClassButtons: 'visually-hidden'
    //                    });
    //                } else {
    //                    window.PaginadorPrincipal.actualiza();
    //                }
    //            },
    //            error: function (error) {
    //                $('#idMsgError').html(err.message == null ? "No se pudo realizar la operacion" : error.message);
    //                OcultarElemento('idDivMsg');
    //                MostrarElemento('idDivMsgError');
    //            }
    //        })
    //        return false;
    //    } catch (ex) {
    //        concole.log(ex);
    //    }
    //}

    ///*Funcion de apertura del modal agregar usuario*/
    

    jqPostAddUser = (form) => {
        try {
            $.ajax({
                type: 'POST',
                url: "Usuarios/AgregarUsuario",
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (response) {
                    //$('#idMsg').html(response.data);
                    //OcultarElemento('idDivMsgError');
                    //MostrarElemento('idDivMsg');

                    //$('#add-user-modal').modal('hide');
                    //$('#idFiltros_Form').submit();

                    //var pagina = $('#PaginaActual').val();
                    //PaginadorPrincipal.irPagina(pagina);
                },

                error: function (response) {
                    //$('#idMsgError').html(response.data);
                    //OcultarElemento('idDivMsg');
                    //MostrarElemento('idDivMsgError');

                    $('id_add_user_form').modal('hide');

                    //var pagina = $('#PaginaActual').val();
                    //PaginadorPrincipal.irPagina(pagina);

                    //idResultadosFiltro
                }
            })
            return false;
        } catch (ex) {
            console.log(ex);
        }
    }

    ///* Funcion GET para actualizar un usuario */
    jqGetModalUpdateUser = (idUsuario) => {
        try {
            $.ajax({
                type: "GET",
                url: "Usuarios/ModalModificarUsuario",
                data: { idUsuario: idUsuario },
                contenType: false,
                proccessData: false,
                success: function (response) {
                    //document.querySelector(".modal-title").textContent = "Modificar Usuario";
                    // Inserta la vista en el modal como HTML
                    $('#updateUserModal .modal-body').html(response.data);
                    // Abre el modal (Bootstrap 4)
                    //$('#updateUserModal').modal('show');

                    // Abre el modal (Bootstrap 5)
                    let modal = new bootstrap.Modal(document.getElementById('updateUserModal'));
                    modal.show();
                },
                error: function () {

                }
            })
        }
        catch (ex) {
        }
    }

    ///* Funcion para eliminar un usuario */
    jqGetModalDeleteUser = (idUsuario) => {

        $.ajax({
            type: "GET",
            url: 'Usuarios/EliminarUsuario',
            data: { idUsuario: idUsuario },
            contenType: false,
            proccessData: false,
            success: function (response) {
                document.querySelector(".modal-title").textContent = "Eliminar Usuario";
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
