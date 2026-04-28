/* GET: Calcular factura */
jqGetModalCalcularFactura = () => {
    $.ajax({
        type: 'GET',
        url: 'Facturacion/ModalCalcularFactura',

        contentType: false,
        processData: false,
        success: function (response) {
            // Inserta la vista en el modal como HTML
            $('#addPacienteModal .modal-body').html(response.data);

            // Abre el modal (Bootstrap 5)
            let modal = new bootstrap.Modal(document.getElementById('addPacienteModal'));
            modal.show();

            $('#importeHora, #numeroHoras').on('input', function () {
                $('#totalImporte').val(
                    $('#importeHora').val() * $('#numeroHoras').val()
                );
            });
        },
        error: function (response) {
            alert("No se pudo realizar la operacion");
        }
    });
}

/* POST: Calcular factura */
jqPostCalcularFactura = (form) => {
    try {
        $.ajax({
            type: 'POST',
            url: 'Facturacion/CalcularFactura',
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (response) {
                let modal = new bootstrap.Modal(document.getElementById('addPacienteModal'));
                modal.hide();
                alert("Factura calculada correctamente");
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