(function (jQuery) {
    jQuery.fn.paginador = function (options) {
        var defaults = {
            Formulario: 'idForm',
            PaginaActual: 'idPagina',
            RegistrosPorPagina: 'RegistrosPorPagina',
            RegistrosPaginaActual: 'idRegistrosPP',
            ClassButtons: 'disabled'
        };
        var settings = $.extend({}, defaults, options);

        var $Paginador = $(this[0]);
        if ($Paginador.length < 1) {
            throw 'No se pudo asociar el Paginador.';
        } else if (!$Paginador.is('div')) {
            console.log('Este plugin está diseñado para funcionar sólo con elementos DIV.');
        }

        var $BtnSiguiente = $($Paginador.children('.PaginaSiguiente')[0]);
        if ($BtnSiguiente.length < 1) {
            console.log('No se encuentra el elemento con la clase PaginaSiguiente.');
        }

        var $BtnAnterior = $($Paginador.children('.PaginaAnterior')[0]);
        if ($BtnAnterior.length < 1) {
            console.log('No se encuentra el elemento con la clase PaginaAnterior.');
        }

        var $TextoPagina = $($Paginador.children('.TextoPaginacion')[0]);
        if ($TextoPagina.length < 1) {
            console.log('No se encuentra elemento con la clase TextoPaginacion.');
        }

        var $Formulario = $('#' + settings.Formulario);
        if ($Formulario.length < 1) {
            console.log('No se encuentra elemento con id:' + settings.Formulario);
        }

        var $Pagina = $('#' + settings.PaginaActual);
        if ($Pagina.length < 1) {
            console.log('No se encuentra elemento con id:' + settings.PaginaActual);
        }

        var $RegistrosPaginaActual = $('#' + settings.RegistrosPaginaActual);
        if ($RegistrosPaginaActual.length < 1) {
            console.log('No se encuentra elemento con id:' + settings.RegistrosPaginaActual);
        }

        var $RegistrosPorPagina = $('#' + settings.RegistrosPorPagina);
        if ($RegistrosPorPagina.length < 1) {
            console.log('No se encuentra elemento con id:' + settings.RegistrosPorPagina);
        }

        var classBtns = settings.ClassButtons;

        this.prepara = function () {
            this.actualiza();
            return this;
        }

        this.paginaAnterior = function () {
            try {
                var vPag = $Pagina.val();
                $Pagina.val(--vPag);
                $Formulario.submit();
            } catch (ex) {
                console.log(ex);
            }
            return this;
        }

        this.paginaSiguiente = function () {
            try {
                var vPag = $Pagina.val();
                $Pagina.val(++vPag);
                $Formulario.submit();
            } catch (ex) {
                console.log(ex);
            }
            return this;
        }

        this.irPagina = function (vPag) {
            try {
                $Pagina.val(vPag);
                $Formulario.submit();
            } catch (ex) {
                console.log(ex);
            }
            return this;
        }

        this.recarga = function () {
            try {
                $Formulario.submit();
            } catch (ex) {
                console.log(ex);
            }
            return this;
        }

        this.actualiza = function () {
            try {
                $Paginador.addClass('visually-hidden');
                $BtnSiguiente.addClass(classBtns);
                $BtnAnterior.addClass(classBtns);
                if ($Pagina.val() > 1) {
                    $BtnAnterior.removeClass(classBtns);
                    $Paginador.removeClass('visually-hidden');
                }

                if ($RegistrosPaginaActual.val() == $RegistrosPorPagina.val()) {
                    $BtnSiguiente.removeClass(classBtns);
                    $Paginador.removeClass('visually-hidden');
                }

                if ($TextoPagina.length > 0) {
                    $TextoPagina.text(/*'Página ' + */$Pagina.val());
                }

            } catch (ex) {
                console.log(ex);
            }
            return this;
        }
        this.destroy = function () {
            try {
                $Paginador.addClass('visually-hidden');
                $BtnSiguiente.addClass(classBtns);
                $BtnAnterior.addClass(classBtns);
                $Pagina.val(1);
                $RegistrosPaginaActual.val(0);
            } catch (ex) {
                console.log(ex);
            }
        }

        return this.prepara();
    }
})(jQuery);

