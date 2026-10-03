/**
 * Gestión del Asistente Virtual Epsilon AI en el cliente.
 * Desarrollado con jQuery siguiendo la convención de scripts del proyecto Epsilon.
 */
$(document).ready(function () {

    // Referencias a los elementos HTML del asistente flotante
    const $wrapper = $('#epsilon-bot-wrapper');
    const $ventana = $('#epsilon-bot-window');
    const $mensajes = $('#epsilon-bot-messages');
    const $input = $('#epsilon-bot-input');
    const $btnAbrir = $('#epsilon-bot-toggle-btn');
    const $btnCerrar = $('#epsilon-bot-close-btn');
    const $btnEnviar = $('#epsilon-bot-send-btn');
    const $cabecera = $('#epsilon-bot-header');

    // Variables para el control de arrastre de la ventana
    let enArrastre = false;
    let difX = 0;
    let difY = 0;
    let seHaMovido = false;

    // 1. Cargar el mensaje de bienvenida al iniciar la interfaz
    const cargarMensajeBienvenida = function () {
        agregarBurbujaMensaje('bot', '¡Hola! Soy **Epsilon AI**, tu asistente de gestión empresarial. ¿En qué puedo ayudarte hoy?');
    };

    // 2. Control de apertura y cierre de la ventana flotante
    $btnAbrir.on('click', function () {
        if (!seHaMovido) {
            $ventana.toggleClass('open');
            if ($ventana.hasClass('open')) {
                $input.focus();
                desplazarAlFinal();
            }
        }
    });

    $btnCerrar.on('click', function (e) {
        e.stopPropagation();
        $ventana.removeClass('open');
    });

    // 3. Selección de sugerencias rápidas (chips)
    $(document).on('click', '.epsilon-bot-chip', function () {
        const consulta = $(this).data('query');
        if (consulta) {
            $input.val(consulta);
            enviarMensaje();
        }
    });

    // 4. Lógica de arrastre de la ventana por la pantalla con el ratón
    [$cabecera, $btnAbrir].forEach(function ($elem) {
        if ($elem && $elem.length) {
            $elem.on('mousedown', function (e) {
                if (e.which !== 1) return; // Solo responder a clic izquierdo

                // Excluir botones de control e input para no interferir con el clic
                if ($(e.target).closest('#epsilon-bot-close-btn, #epsilon-bot-input, #epsilon-bot-send-btn, .epsilon-bot-chip').length) {
                    return;
                }

                enArrastre = true;
                seHaMovido = false;
                const offset = $wrapper.offset();
                difX = e.pageX - offset.left;
                difY = e.pageY - offset.top;
            });
        }
    });

    $(document).on('mousemove', function (e) {
        if (!enArrastre) return;

        const deltaX = e.pageX - difX;
        const deltaY = e.pageY - difY;

        // Desactivar comportamiento de clic si el movimiento supera los 4 píxeles
        seHaMovido = true;

        // Calcular los límites de pantalla para evitar que se pierda el botón flotante
        let nuevaX = Math.max(10, Math.min($(window).width() - 70, deltaX));
        let nuevaY = Math.max(10, Math.min($(window).height() - 70, deltaY));

        $wrapper.css({
            right: 'auto',
            bottom: 'auto',
            left: nuevaX + 'px',
            top: nuevaY + 'px'
        });
    });

    $(document).on('mouseup', function () {
        if (enArrastre) {
            setTimeout(function () {
                seHaMovido = false;
            }, 100);
        }
        enArrastre = false;
    });

    // 5. Enviar mensaje del usuario al controlador C# (BotController)
    const enviarMensaje = function () {
        const texto = $input.val().trim();
        if (!texto) return;

        // Mostrar el mensaje del usuario en la pantalla
        agregarBurbujaMensaje('user', texto);
        $input.val('');

        const ubicacionActual = window.location.pathname || '';

        // Realizar la petición AJAX al backend
        $.ajax({
            url: '/api/bot/chat',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ message: texto, context: ubicacionActual }),
            success: function (respuesta) {
                agregarBurbujaMensaje('bot', respuesta.text);
            },
            error: function () {
                // Mensaje en caso de que la API de Gemini o el servidor no respondan
                agregarBurbujaMensaje('bot', 'El servicio de IA no está disponible temporalmente. Por favor, utilice las opciones del menú principal para consultar la información.');
            }
        });
    };

    $btnEnviar.on('click', enviarMensaje);

    $input.on('keydown', function (e) {
        if (e.key === 'Enter' && !e.shiftKey) {
            e.preventDefault();
            enviarMensaje();
        }
    });

    // 6. Función auxiliar para renderizar burbujas de texto con formato
    function agregarBurbujaMensaje(remitente, texto) {
        const claseRemitente = remitente === 'user' ? 'user' : 'bot';
        
        // Formatear el texto (convertir negritas y saltos de línea)
        let textoFormateado = texto
            .replace(/&/g, "&amp;")
            .replace(/</g, "&lt;")
            .replace(/>/g, "&gt;")
            .replace(/\*\*(.*?)\*\*/g, '<strong>$1</strong>')
            .replace(/\*(.*?)\*/g, '<em>$1</em>')
            .replace(/\n/g, '<br/>');

        const hora = new Date().toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
        const htmlBurbuja = `<div class="epsilon-bot-message ${claseRemitente}">${textoFormateado}<span class="epsilon-bot-message-time">${hora}</span></div>`;

        $mensajes.append(htmlBurbuja);
        desplazarAlFinal();
    }

    // 7. Auto-scroll al final del contenedor de mensajes
    function desplazarAlFinal() {
        if ($mensajes.length) {
            $mensajes.scrollTop($mensajes[0].scrollHeight);
        }
    }

    // Cargar mensaje inicial al iniciar
    cargarMensajeBienvenida();
});
