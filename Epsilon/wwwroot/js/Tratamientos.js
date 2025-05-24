$(document).ready(function () {

    jqGetModalAddUser = (form) => {
        $.ajax({
            type: 'GET',
            url: 'Tratamientos/ModalAgregarTratamiento',
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (response) {
                console.log(document.querySelector("#addTratamientoModal .modal-body"));
                // Inserta la vista en el modal como HTML
                $('#addTratamientoModal .modal-body').html(response.data);

                // Abre el modal (Bootstrap 5)
                let modal = new bootstrap.Modal(document.getElementById('addTratamientoModal'));
                modal.show();

                //$("#addUser1").validate({
                //    rules: {
                //        EMail: { required: true, email: true },
                //        Password: { required: true, minlength: 6 }
                //    },
                //    messages: {
                //        EMail: { required: "Se debe introducir un email válido.", email: "Formato incorrecto." },
                //        Password: { required: "Se debe introducir una contraseña.", minlength: "Debe tener al menos 6 caracteres." }
                //    }
                //});
            },

            error: function (response) {
                alert("No se pudo realizar la operacion");
            }
        });
    }

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


});

