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

    //if (window.PaginadorPrincipal == undefined) {
    //    $("#RegistrosPaginaActual").val($('idBodyTable tr').length);
    //    window.PaginadorPrincipal = $('#idPaginadorPrincipal').paginador({
    //        Formulario: 'id_Form',
    //        PaginaActual: 'PaginaActual',
    //        RegistrosPorPagina: 'RegistrosPorPagina',
    //        RegistrosPaginaActual: 'RegistrosPaginaActual',
    //        ClassButtons: 'visually-hidden'
    //    });
    //}

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
                document.querySelector(".modal-title").textContent = "Alta Usuario";
                // Inserta la vista en el modal como HTML
                $('#addUserModal .modal-body').html(response.data);

             
                
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

    $('#addUser').validate({
        rules: {
            Nombre: {
                required: true,
                maxLenght: 60,
                minLenght: 2,
            },
            Password: {
                required: true,
                maxLenght: 100,
                minLenght: 6,
            }
        },
        messages: {
            Nombre: {
                required: "El nomnbre es obligatorio",
                max: "El tamaño maximo del nombre son 10 caracteres"
            },
            Password: {
                required: "El nomnbre es obligatorio",
                max: "El tamaño maximo del nombre son 10 caracteres"
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
                }
            })
            return false;
        } catch (ex) {
            console.log(ex);
        }
    }

    /* Funcion para actualizar un usuario */
    jqUpdateUser = (form) => {
        try {
            $.ajax({
                type: "GET",
                url: url,
                data: id,
                contenType: false,
                proccessData: false,

            })

        }

        catch (ex) {


        }

    }

    /* Funcion para eliminar un usuario */
    jqRemoveUser = (form) => {
        if (window.confirm("¿Desea realmente eliminar este uruario?")) {

            var idUsuario = $('#idUsuario').val();

            $.ajax({
                type: "POST",
                url: 'Usuarios/EliminarUsuario',
                data: idUsuario,
                contenType: false,
                proccessData: false,
                success: function (response) {



                },
                error: function (response) {
                    alert("No se pudo realizar la operacion");
                }
            })

        }
    }

});