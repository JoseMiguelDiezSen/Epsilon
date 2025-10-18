(function ($) {
    $.fn.popupLoader = function (options) {
        var defaults = {
            delay: 1000,
            title_ajax_error: 'Error de conexión',
            title_response_error: 'Error en la operación',
            idPopupAlert: 'main-popup-alert',
        };
        var settings = $.extend({}, defaults, options);

        var $Loader = $(this[0]);
        if ($Loader.length < 1) {
            throw 'No se pudo asociar el plugin al elemento HTML.';
        } else if (!$Loader.is('div')) {
            console.log('Este plugin está diseñado para funcionar sólo con elementos DIV.');
        }

        var $PopupAlert = $('#' + settings.idPopupAlert);
        var idTimeout = 0;
        var showRequired = 0;

        var show = function () {
            if (showRequired < 1) {
                idTimeout = delayedShow(settings.delay);
            }
            showRequired++;
        }

        var hide = function () {
            showRequired--;
            if (showRequired < 1) {
                clearTimeout(idTimeout);
                $Loader.hide();
            }
        }

        var hideAll = function () {
            showRequired = 0;
            clearTimeout(idTimeout);
            $Loader.hide();
        }

        var delayedShow = function (delay) {
            return setTimeout(function () {
                if (showRequired > 0) {
                    $Loader.show();
                }
            }, delay);
        }

        var showError = function (title, error) {
            hideError();
            $('#idTitleAlertError').html(title);
            $('#idMsgAlertError').html(error);
            $PopupAlert.show();
        }

        var hideError = function (title, error) {
            $PopupAlert.hide();
        }

        var GetAjaxContext = function (ajaxOptions) {
            return ajaxOptions.context === undefined ? { not_show_popup: false } : ajaxOptions.context;
        }

        this.onAjaxSend = function (event, jqXHR, ajaxOptions) {
            let context = GetAjaxContext(ajaxOptions);
            context = context ? context : {};
            if (!context.not_show_popup) {
                show();
            }
        }

        this.onAjaxComplete = function (event, jqXHR, ajaxOptions) {
            let context = GetAjaxContext(ajaxOptions);
            context = context ? context : {};
            if (!context.not_show_popup) {
                hide();
                let datosJSON = { status: '200', data: 'Ok' };
                if (jqXHR.responseJSON !== undefined) {
                    datosJSON.status = jqXHR.responseJSON.status != undefined ? jqXHR.responseJSON.status : datosJSON.status;
                    datosJSON.data = jqXHR.responseJSON.data != undefined ? jqXHR.responseJSON.data : datosJSON.data;
                }

                if (datosJSON.status != '200') {
                    showError(settings.title_response_error, datosJSON.data);
                }
            }
        }

        this.onAjaxError = function (event, jqXHR, ajaxSettings, thrownError) {
            let context = GetAjaxContext(ajaxSettings);
            context = context ? context : {};
            if (!context.not_show_popup) {
                hide();
                showError(settings.title_ajax_error, 'La llamada a "' + ajaxSettings.url + '" falló con el mensaje: ' + thrownError);
            }
        }

        this.onAjaxStop = function () {
            hideAll();
        }

        this.prepara = function () {
            $(document).on('ajaxSend', this.onAjaxSend);
            $(document).on('ajaxComplete', this.onAjaxComplete);
            $(document).on('ajaxError', this.onAjaxError);
            $(document).on('ajaxStop', this.onAjaxStop);
            return this;
        }

        return this.prepara();
    }
}(jQuery));