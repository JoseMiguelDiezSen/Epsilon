
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

    // Gestión del menú lateral - Persistencia en localStorage sin chispazo
    const sidebarStateKey = 'sidebarState';

    mostrarOcultarMenu = () => {
        $('.sidebar .nav-group .nav-title').off('click').on('click', function () {
            const $group = $(this).closest('.nav-group');
            $group.toggleClass('expanded');

            // Guardar persistencia del estado en localStorage
            const saved = {};
            $('.sidebar .nav-group').each(function (idx) {
                saved[idx] = $(this).hasClass('expanded');
            });
            try {
                localStorage.setItem(sidebarStateKey, JSON.stringify(saved));
            } catch (e) {}
        });
    };

    mostrarOcultarMenu();

});