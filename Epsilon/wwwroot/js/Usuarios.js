$(document).ready(function () {

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

    /* GET: Añadir usuario OK */
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

                $("#addUser").validate({
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

                //$('#idMsg').html(response.data);
            },
            error: function (response) {
                alert("No se pudo realizar la operacion");
            }
        });
    }

    /* POST: Añadir usuario OK */
    jqPostAddUser = (form) => {
        try {
            $.ajax({
                type: 'POST',
                url: 'Usuarios/AgregarUsuario',
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
                    // Abre el modal (Bootstrap 5)
                    let modal = new bootstrap.Modal(document.getElementById('addUserModal'));
                    modal.hide();
                    alert("Usuario creado correctamente");
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

    /* GET : Actualizar un usuario */
    jqGetModalUpdateUser = (idUsuario) => {
        try {

            var id = idUsuario;


            $.ajax({
                type: 'GET',
                url: 'Usuarios/ModalModificarUsuario',
                data: { idUsuario : idUsuario },
         
                success: function (response) {
              
                    // Inserta la vista en el modal como HTML
                    $('#updateUserModal .modal-body').html(response.data);
                    // Abre el modal (Bootstrap 4)
                    //$('#updateUserModal').modal('show');

                    // Abre el modal (Bootstrap 5)
                    let modal = new bootstrap.Modal(document.getElementById('updateUserModal'));
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

    /* POST : Actualizar un usuario */
    jqPostUpdateUser = (form) => {
        try {
            $.ajax({
                type: "POST",
                url: "Usuarios/ModificarUsuario",
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
        try {
            $.ajax({
                type: 'POST',
                url: "Usuarios/EliminarUsuario",
                data: { idUsuario : idUsuario },

                success: function (response) {
                    //$('#idMsg').html(response.data);
                    //OcultarElemento('idDivMsgError');
                    //MostrarElemento('idDivMsg');

                    //$('#add-user-modal').modal('hide');
                    //$('#idFiltros_Form').submit();

                    //var pagina = $('#PaginaActual').val();
                    //PaginadorPrincipal.irPagina(pagina);
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
