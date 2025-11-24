
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

    // Lo MIO

    /*Funcion para expandir y contraer menu principal*/
    mostrarOcultarMenu = () => {
        const sidebarStateKey = 'sidebarState';
        let savedState = JSON.parse(localStorage.getItem(sidebarStateKey) || '{}');
        $('.nav-group').each(function (index) {
            const $group = $(this);
            const $title = $group.find('.nav-title');
            const $items = $group.find('.nav-items');

            // Restaurar estado guardado al cargar (sin animación)
            if (savedState[index]) {
                $group.addClass('expanded');
                // no slideDown, CSS manejará visualmente
            } else {
                $group.removeClass('expanded');
                // no slideUp
            }

            // Toggle en tiempo real (solo toggle class)
            $title.off('click').on('click', function () {
                const isExpanded = $group.hasClass('expanded');
                $group.toggleClass('expanded');

                // Guardar estado actualizado
                savedState[index] = !isExpanded;
                localStorage.setItem(sidebarStateKey, JSON.stringify(savedState));
            });
        });
    }

    mostrarOcultarMenu();



});