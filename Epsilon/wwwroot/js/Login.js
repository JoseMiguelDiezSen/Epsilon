$(document).ready(function () {

    // Opción: enviar con botón
    jqPostLoginUser = () => { 
        var usuario = $("#usuario").val();
        var contraseña = $("#contraseña").val();

        $.ajax({
            url: '/Login/LoginUser',
            type: 'POST',
            data: { usuario: nombre, contraseña: password },
            success: function (response) {
                if (response.success) {
                    window.location.href = response.redirectUrl;
                } else {
                    $("#mensaje").text(response.message);
                }
            },
            error: function () {
                $("#mensaje").text("Error al comunicarse con el servidor");
            }
        });
    }


    // Opción: enviar con Enter
    $("#loginForm input").keypress(function (e) {
        if (e.which == 13) {
            jqPostLoginUser();
        }
    });

});
