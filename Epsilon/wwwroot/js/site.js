
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

    /*Funcion para expandir y contraer menu principal*/
    mostrarOcultarMenu = () => {
        const sidebarStateKey = 'sidebarState';
        // Recuperar estado guardado
        let savedState = JSON.parse(localStorage.getItem(sidebarStateKey) || '{}');
        $('.nav-group').each(function (index) {
            const $group = $(this);
            const $title = $group.find('.nav-title');
            const $items = $group.find('.nav-items');

            // Restaurar estado guardado al cargar
            if (savedState[index]) {
                $group.addClass('expanded');
                $items.stop(true, true).slideDown(0); // altura fijada sin animación
            } else {
                $group.removeClass('expanded');
                $items.stop(true, true).slideUp(0);   // altura cero, invisible
            }

            // Toggle en tiempo real
            $title.off('click').on('click', function () {
                const isExpanded = $group.hasClass('expanded');
                $group.toggleClass('expanded');
                if (isExpanded) {
                    $items.stop(true, true).slideUp(2000); // animación suave al cerrar
                } else {
                    $items.stop(true, true).slideDown(2000); // animación suave al abrir
                }

                // Guardar estado actualizado
                savedState[index] = !isExpanded;
                localStorage.setItem(sidebarStateKey, JSON.stringify(savedState));
            });
        });
    }

    mostrarOcultarMenu();
});