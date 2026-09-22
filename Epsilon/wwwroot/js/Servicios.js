jQuery(function () {

    /* Modal Añadir Servicio */
    window.jqGetModalAddServicio = function () {
        $.ajax({
            type: 'GET',
            url: '/Servicios/ModalAgregarServicio',
            contentType: false,
            processData: false,
            success: function (response) {
                $('#addServicioModal .modal-body').html(response.data);
                let modal = new bootstrap.Modal(document.getElementById('addServicioModal'));
                modal.show();
            },
            error: function () {
                alert("No se pudo cargar el formulario de servicio.");
            }
        });
    };

    /* POST Añadir Servicio */
    window.jqPostAddServicio = function (form) {
        try {
            $.ajax({
                type: 'POST',
                url: '/Servicios/AgregarServicio',
                data: $(form).serialize(),
                success: function (response) {
                    if (response.StatusCode === 200 || response.statusCode === 200) {
                        const modal = bootstrap.Modal.getInstance(document.getElementById('addServicioModal'));
                        if (modal) modal.hide();
                        alert("Servicio creado con éxito.");
                        location.reload();
                    } else {
                        alert(response.message || "Error al crear el servicio.");
                    }
                },
                error: function () {
                    alert("Error en el servidor al guardar el servicio.");
                }
            });
            return false;
        } catch (ex) {
            console.error(ex);
        }
    };

});

    window.jqGetModalUpdateServicio = function (idServicio) {
        $.ajax({
            type: 'GET',
            url: '/Servicios/ModalModificarServicio',
            data: { idServicio: idServicio },
            success: function (response) {
                #updateServicioModal .modal-body.html(response.data);
                let modal = new bootstrap.Modal(document.getElementById('updateServicioModal'));
                modal.show();
            }
        });
    };

    window.jqPostUpdateServicio = function (form) {
        $.ajax({
            type: 'POST',
            url: '/Servicios/ModificarServicio',
            data: .serialize(),
            success: function (response) {
                let modal = bootstrap.Modal.getInstance(document.getElementById('updateServicioModal'));
                if (modal) modal.hide();
                location.reload();
            }
        });
        return false;
    };

    window.jqGetModalDeleteServicio = function (idServicio) {
        $.ajax({
            type: 'GET',
            url: '/Servicios/ModalEliminarServicio',
            data: { idServicio: idServicio },
            success: function (response) {
                #deleteServicioModal .modal-body.html(response.data);
                let modal = new bootstrap.Modal(document.getElementById('deleteServicioModal'));
                modal.show();
            }
        });
    };

    window.jqPostDeleteServicio = function (idServicio) {
        $.ajax({
            type: 'POST',
            url: '/Servicios/EliminarServicio',
            data: { idServicio: idServicio },
            success: function (response) {
                let modal = bootstrap.Modal.getInstance(document.getElementById('deleteServicioModal'));
                if (modal) modal.hide();
                location.reload();
            }
        });
        return false;
    };

    window.jqGetModalUpdateServicio = function (idServicio) {
        $.ajax({
            type: 'GET',
            url: '/Servicios/ModalModificarServicio',
            data: { idServicio: idServicio },
            success: function (response) {
                $('#updateServicioModal .modal-body').html(response.data);
                let modal = new bootstrap.Modal(document.getElementById('updateServicioModal'));
                modal.show();
            }
        });
    };

    window.jqPostUpdateServicio = function (form) {
        $.ajax({
            type: 'POST',
            url: '/Servicios/ModificarServicio',
            data: $(form).serialize(),
            success: function (response) {
                let modal = bootstrap.Modal.getInstance(document.getElementById('updateServicioModal'));
                if (modal) modal.hide();
                location.reload();
            }
        });
        return false;
    };

    window.jqGetModalDeleteServicio = function (idServicio) {
        $.ajax({
            type: 'GET',
            url: '/Servicios/ModalEliminarServicio',
            data: { idServicio: idServicio },
            success: function (response) {
                $('#deleteServicioModal .modal-body').html(response.data);
                let modal = new bootstrap.Modal(document.getElementById('deleteServicioModal'));
                modal.show();
            }
        });
    };

    window.jqPostDeleteServicio = function (idServicio) {
        $.ajax({
            type: 'POST',
            url: '/Servicios/EliminarServicio',
            data: { idServicio: idServicio },
            success: function (response) {
                let modal = bootstrap.Modal.getInstance(document.getElementById('deleteServicioModal'));
                if (modal) modal.hide();
                location.reload();
            }
        });
        return false;
    };
