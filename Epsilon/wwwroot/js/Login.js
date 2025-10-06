jQuery(function () {


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

    // Regla de validacion personalizada pare el Email
    //$.validator.addMethod("gmailValido", function (value, element) {
    //    // Nombre de usuario: 6-20 caracteres, letras, números, . _ -
    //    return this.optional(element) || /^[a-zA-Z0-9._-]{6,20}@gmail\.com$/.test(value);
    //}, "Introduce un correo válido de Gmail");

    //// Validacion personalizada telefono
    //$.validator.addMethod("soloNumeros", function (value, element) {
    //    return this.optional(element) || /^[0-9\s\-()+]+$/.test(value);
    //}, "(*) Introduce solo números");

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
