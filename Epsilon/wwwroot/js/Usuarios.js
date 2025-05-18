$(document).ready(function () {

    

    jqCheckAddPeriodo = () => {

        const date = new Date();
        var day = date.getDate();
        var month = date.getMonth();
        var year = date.getFullYear();
        var currentDate = day + "/" + month + "/" + year;

        var d1 = dateFrom.slit("/");
        var d1 = dateTo.slit("/");
        var c = dateCheck.slit("/");

        var from = new Date(d1);
        var to = new Date(d2);
        var check = new Date(c);

        if (check > from && check < to) {
            $("#addUser").show();
        }
        else {
            $("#addUser").hide();
        }
    }

  
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

    /* Funcion para el filtrado de usuarios */
    jqPostFiltrar = (ev, form) => {
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

    /*Funcion de apertura del modal agregar usuario*/
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

                // Esperar un pequeño tiempo para asegurar que el contenido se inserte completamente
                setTimeout(function () {
                    $("#addUserModal").validate();
                }, 200); 

                // Abre el modal (Bootstrap 4)
                //$('#addUserModal').modal('show');

                // Abre el modal (Bootstrap 5)
                let modal = new bootstrap.Modal(document.getElementById('addUserModal'));
                modal.show();

              
            },

            error: function (response) {
                alert("No se pudo realizar la operacion");
            }


        })

       
    }

    $("#addUserModal").validate({
        rules: {
            Nombre: {
                required: true,
                maxlength: 60, // Corrección
                minlength: 2   // Corrección
            },
            Password: {
                required: true,
                maxlength: 100, // Corrección
                minlength: 6   // Corrección
            }
        },
        messages: {
            Nombre: {
                required: "El nombre es obligatorio",
                maxlength: "El tamaño máximo del nombre es de 60 caracteres", // Corrección
                minlength: "El nombre debe tener al menos 2 caracteres" // Corrección
            },
            Password: {
                required: "La contraseña es obligatoria", // Corrección
                maxlength: "El tamaño máximo de la contraseña es de 100 caracteres", // Corrección
                minlength: "La contraseña debe tener al menos 6 caracteres" // Corrección
            }
        }
    });


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

    /* Funcion GET para actualizar un usuario */
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

    /* Funcion para eliminar un usuario */
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
});