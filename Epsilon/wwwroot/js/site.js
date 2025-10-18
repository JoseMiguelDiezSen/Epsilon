
$(document).ready(function () {
    //    $('#modal-loading').PopupLoader();



    MostrarElemento = (idElement) => {
        $('#' + idElement).removeClass('visually-hidden');
    }

    OcultarElemento = (idElement) => {
        $('#' + idElement).addClass('visually-hidden');
    }

    $('#idDivMsg').on('click', function () {
        $(this).addClass('visually-hidden');
    });

    $('#idDivMsgError').on('click', function () {
        $(this).addClass('visually-hidden');
    });

});