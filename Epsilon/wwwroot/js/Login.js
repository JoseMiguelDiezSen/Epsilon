jQuery(function () {
    $(document).ready(function () {
        var image = document.getElementById('themesBtn');
        var inputFoto = document.getElementById('fotoUsuario');

        if (!image || !inputFoto) return; // 🛡️ protección anti-null

        var rutaFoto = "/media/defaultUserImage.jpg";
        var fotoBase64 = inputFoto.value;

        if (fotoBase64 && fotoBase64 !== "") {
            rutaFoto = "data:image/jpeg;base64," + fotoBase64;
        }

        image.src = rutaFoto;
    });

    // Se definen las reglas de validacion
    $("#LoginUsuarioForm").validate({
        ignore: [],
        rules: {
            Nombre: { required: true },
            Password: { required: true, minlength: 4 },
        },
        messages: {
            Nombre: { required: "(*) Debe introducir un nombre" },
            Password: { required: "(*) Debe introducir una contraseña.", minlength: "Debe tener al menos 4 caracteres." },
        },

        errorClass: "is-invalid",
        validClass: "is-valid",

        // Se posicionan debajo de los controles
        errorPlacement: function (error, element) {
            if (element.attr("name") === "Nombre") {
                error.insertAfter("#inputNombre");
            }
            if (element.attr("name") === "Password") {
                error.insertAfter("#inputPassword");
            }
            error.addClass('text-danger').addClass('font-size:6px;'); // Bootstrap style
        }
    });

    // Inicializar particleground sobre el contenedor de fondo
    $('#particles-container').particleground({
        dotColor: '#5cbdaa',
        lineColor: '#5cbdaa'
    });

    // Toggle password
    $('#togglePassword').click(function () {
        let passwordInput = $('#Password');
        let icon = $(this).find('i');

        if (passwordInput.attr('type') === 'password') {
            passwordInput.attr('type', 'text');
            icon.removeClass('fa-eye').addClass('fa-eye-slash');
        } else {
            passwordInput.attr('type', 'password');
            icon.removeClass('fa-eye-slash').addClass('fa-eye');
        }
    });

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    // Callback de Google Login
    //window.handleGoogleLogin = (response) => {
    //    const token = response.credential;

    //    $.ajax({
    //        url: '/Login/LoginGoogle',
    //        type: 'POST',
    //        data: { token: token },
    //        success: function (res) {
    //            if (res.success) {
    //                window.location.href = res.redirectUrl;
    //            } else {
    //                $("#mensaje").text(res.message);
    //            }
    //        },
    //        error: function () {
    //            $("#mensaje").text("Error en login con Google");
    //        }
    //    });
    //};

   
    // Opción: enviar con botón
    jqPostLoginUser = () => {
        if ($("#LoginUsuarioForm").valid()) {
            var nombre = $("#Nombre").val();
            var password = $("#Password").val();

            $.ajax({
                url: '/Login/LoginUser',
                type: 'POST',
                data: { nombre: nombre, password: password },
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
    }



});