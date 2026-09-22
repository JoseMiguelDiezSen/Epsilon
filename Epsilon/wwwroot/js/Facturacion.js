window.onload = function () {
    const ctx = document.getElementById('myChart').getContext('2d');

    const labels = ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'];

    const datos = (typeof datosChartIniciales !== 'undefined') ? datosChartIniciales : []; // Datos reales por mes (vienen del modelo en la vista)

    const data = {
        labels: labels,
        datasets: [
            {
                label: 'Ingresos',
                data: datos.map(d => d.totalFacturacion), // Ingresos = facturación real de cada mes
                borderColor: 'rgba(75, 192, 192, 1)',
                backgroundColor: 'rgba(75, 192, 192, 0.2)',
                tension: 0.4,
                fill: true,
                pointStyle: 'rectRot',
                pointRadius: 6,
                pointHoverRadius: 8
            },
            {
                label: 'Gastos',
                data: datos.map(d => 0), // No hay tabla de gastos, se pone a 0
                borderColor: 'rgba(255, 99, 132, 1)',
                backgroundColor: 'rgba(255, 99, 132, 0.2)',
                tension: 0.4,
                fill: true,
                pointStyle: 'circle',
                pointRadius: 6,
                pointHoverRadius: 8
            },


            // Esto deberia ser autocalculado en funcion de ingrsos y gastos
            {
                label: 'Beneficio',
                data: datos.map(d => d.totalFacturacion), // Beneficio = facturación real (Ingresos - Gastos)
                borderColor: 'rgba(54, 162, 235, 1)',
                backgroundColor: 'rgba(54, 162, 235, 0.2)',
                tension: 0.4,
                fill: true,
                pointStyle: 'triangle',
                pointRadius: 6,
                pointHoverRadius: 8
            }
        ]
    };

    const config = {
        type: 'line',
        data: data,
        options: {
            responsive: true,
            plugins: {
                legend: {
                    position: 'top',
                    labels: { font: { size: 14, weight: 'bold' } }
                },
                title: {
                    display: true,
                    text: 'Informe de Facturación',
                    font: { size: 22 }
                },
                tooltip: {
                    mode: 'index',
                    intersect: false,
                    backgroundColor: 'rgba(0,0,0,0.7)',
                    titleFont: { size: 16, weight: 'bold' },
                    bodyFont: { size: 14 }
                }
            },
            interaction: {
                mode: 'nearest',
                axis: 'x',
                intersect: false
            },
            scales: {
                x: { display: true, title: { display: true, text: 'Meses' } },
                y: { display: true, title: { display: true, text: 'Euros' }, beginAtZero: true }
            }
        }
    };

    // Aquí solo una vez, dentro del window.onload
    window.myChart = new Chart(ctx, config); // Se guarda en window para poder actualizarlo después por SignalR

    // Conectar con SignalR para actualizaciones en tiempo real
    conectarSignalR();
};

// Función para conectar con SignalR
function conectarSignalR() {
    if (typeof signalR === 'undefined') {
        console.error("❌ SignalR no está cargado. Revisa los scripts.");
        return;
    }

    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/facturacionHub")
        .withAutomaticReconnect()
        .build();

    connection.on("ActualizacionFacturacion", function () {
        console.log("📢 Recibida notificación de actualización de facturación");
        cargarDatosFacturacion();
    });

    connection.start().then(function () {
        console.log("✅ Conectado a SignalR para actualizaciones de facturación");
    }).catch(function (err) {
        console.error("❌ Error al conectar con SignalR: " + err);
    });
}

// Función para cargar los datos de facturación desde el servidor
function cargarDatosFacturacion() {
    $.ajax({
        type: 'GET',
        url: 'Facturacion/ObtenerDatosFacturacion',
        success: function (response) {
            console.log("✅ Datos recibidos del servidor:", response);
            actualizarTotales(response.totalFacturacion, response.totalCitas);
            actualizarChartFacturacion(response.datosChart);
        },
        error: function (response) {
            console.error("❌ Error al cargar los datos de facturación:", response);
        }
    });
}

// Función para actualizar los totales en pantalla
function actualizarTotales(totalFacturacion, totalCitas) {
    document.getElementById('totalFacturacion').textContent = new Intl.NumberFormat('es-ES', { style: 'currency', currency: 'EUR' }).format(totalFacturacion);
    document.getElementById('totalCitas').textContent = totalCitas;
}

// Función para actualizar el chart con los datos de facturación manteniendo su formato
function actualizarChartFacturacion(datosChart) {
    if (!window.myChart) return;

    window.myChart.data.datasets[0].data = datosChart.map(d => d.totalFacturacion); // Ingresos
    window.myChart.data.datasets[1].data = datosChart.map(d => 0); // Gastos
    window.myChart.data.datasets[2].data = datosChart.map(d => d.totalFacturacion); // Beneficio

    window.myChart.update(); // Redibuja el chart sin cambiar su formato
}

/* GET: Calcular factura */
jqGetModalCalcularFactura = () => {
    $.ajax({
        type: 'GET',
        url: 'Facturacion/ModalCalcularFactura',

        contentType: false,
        processData: false,
        success: function (response) {
            // Inserta la vista en el modal como HTML
            $('#addPacienteModal .modal-body').html(response.data);

            // Abre el modal (Bootstrap 5)
            let modal = new bootstrap.Modal(document.getElementById('addPacienteModal'));
            modal.show();

            $('#importeHora, #numeroHoras').on('input', function () {
                $('#totalImporte').val(
                    $('#importeHora').val() * $('#numeroHoras').val()
                );
            });
        },
        error: function (response) {
            alert("No se pudo realizar la operacion");
        }
    });
}

/* POST: Calcular factura */
jqPostCalcularFactura = (form) => {
    try {
        $.ajax({
            type: 'POST',
            url: 'Facturacion/CalcularFactura',
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (response) {
                let modal = new bootstrap.Modal(document.getElementById('addPacienteModal'));
                modal.hide();
                alert("Factura calculada correctamente");
            },

            error: function (response) {
                //$('#idMsgError').html(response.data);
                //OcultarElemento('idDivMsg');
                //MostrarElemento('idDivMsgError');
                //$('id_add_user_form').modal('hide');
                //var pagina = $('#PaginaActual').val();
                //PaginadorPrincipal.irPagina(pagina);
                //idResultadosFiltro
                alert("Ha ocurrido un error");
            }
        })
        return false;
    } catch (ex) {
        console.log(ex);
    }
}