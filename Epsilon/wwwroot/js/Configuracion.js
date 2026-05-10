jQuery(function () {

    jqGetModalCorreoElectronico = () => {

        $.ajax({
            type: 'GET',
            url: 'Configuracion/ModalConfigurarCorreo',

            success: function (response) {

                // Colocamos el contenido del modal
                $('#correoModal .modal-body').html(response.data);

                // Preparamos el combo AHORA que el HTML ya existe
                const modeloCorreo = document.getElementById("ModeloCorreo");

                if (modeloCorreo) {
                    modeloCorreo.addEventListener("change", function () {

                        const idCorreo = this.value;
                        if (!idCorreo) return;

                        // Segunda llamada AJAX normal
                        $.ajax({
                            type: 'GET',
                            url: 'Configuracion/GetCorreoInfo',
                            data: { idCorreo: idCorreo },

                            success: function (response) {
                                const datos = JSON.parse(response.data); 
                                document.getElementById("asunto").value = datos.Asunto;
                                document.getElementById("cuerpoMensaje").value = datos.CuerpoMensaje;
                                document.getElementById("IdCorreo").value = datos.IdCorreo;
                                document.getElementById("botonEliminarCorreo").style.display = "inline-block";
                            },

                            error: function (xhr, status, error) {
                                console.error("Error obteniendo correo info:", error);
                            }
                        });

                    });
                }

                // Mostrar el modal
                let modal = new bootstrap.Modal(document.getElementById('correoModal'));
                modal.show();
            },

            error: function (xhr, status, error) {
                console.error("Error cargando modal:", error);
            }
        });

    };


   //$.ajax({
        //    type: 'GET',
        //    url: 'Configuracion/ModalConfigurarCorreo',
        //    data: new FormData(form),
        //    contentType:false,
            
        //    success: function (response) {

        //        $('#correoModal .modal-body').html(response.data);
        //        let modal = new bootstrap.Modal(document.getElementById('correoModal'));
        //        modal.show();
        //    },
        //    error: function (xhr, status, error) {
        //        console.error("Error al cargar el modal de correo electrónico:", error);
        //    }
        //});



    

    jqPostConfiguracionCorreo = () => {
        //let modal = new bootstrap.Modal(document.getElementById('correoModal'));
        //modal.show();


        alert("REFRESCAR!!!")

    }

    jqPostEliminarModeloCorreo = () => {
        $.ajax({
            type: 'POST',
            url: 'Configuracion/EliminarModeloCorreo',
            data: { idCorreo: $("ModeloCorreo").val() }, /*{ idCorreo: idCorreo }*/
            contentType:false,

            success: function (response) {

                $('#correoModal .modal-body').html(response.data);
                //let modal = new bootstrap.Modal(document.getElementById('correoModal'));
                //modal.hide();
            },
            error: function (xhr, status, error) {
                console.error("Error al cargar el modal de correo electrónico:", error);
            }
        });
    }

    jqAddModeloCorreo = () => {

        const nombre = $("#NombreCorreoNuevo").val();
        const asunto = $("#asunto").val();
        const cuerpo = $("#cuerpoMensaje").val();

        $.ajax({
            type: 'POST',
            url: 'Pacientes/AddModeloCorreo',
            data: {
                nombreCorreo: nombre,
                asunto: asunto,
                cuerpoMensaje: cuerpo
            },

            success: function (response) {

                // Recargar modal con nueva info
                $('#modalEnvioCorreo .modal-body').html(response.data);

            },

            error: function (xhr, status, error) {
                console.error("Error al añadir modelo de correo:", error);
            }
        });
    };


});



