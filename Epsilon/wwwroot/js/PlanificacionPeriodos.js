$(document).ready(function () {

    jqGetModalAddPeriodo = (url, title, modal) => {

        try {

            $.ajax({
                type: 'GET',
                url: url,
                contentType: false,
                proccessData: false,

                success: function (response) {

                    $('#' + modal + ' .modal-body').html(response.data);
                    $('#' + modal + ' .modal-body').html(title);
                    $('#' + modal).modal('show');

                    PreparaValidacion('idAddPeriodo_Form');
                },

                error: function (error) {
                    alert('Se ha producido un error al realizar la operacion.');
                    console.log(error);
                }
            })

            return false;
        } catch (ex) {
            console.log(ex)
        }
    }

    jqPostAddPeriodo = (form) => {
        try {

            $.ajax({
                type: 'POST',
                url: form.action,
                data: new FormData(form),
                contentType: false,
                processData: false,

                success: function (response) {
                    $('#idMsg').html(response.data);
                    OcultarElemento('idDivMsgError');
                    MostrarElemento('idDivMsg');

                    $('#add-user-modal').modal('hide');
                    $('#idFiltros_Form').submit();

                    var pagina = $('#PaginaActual').val();
                    PaginadorPrincipal.irPagina(pagina);
                },

                error: function (response) {
                    $('#idMsgError').html(response.data);
                    OcultarElemento('idDivMsg');
                    MostrarElemento('idDivMsgError');

                    $('id_add_user_form').modal('hide');

                    var pagina = $('#PaginaActual').val();
                    PaginadorPrincipal.irPagina(pagina);
                }
            })
            return false;
        } catch (ex) {
            console.log(ex);
        }
    }

});