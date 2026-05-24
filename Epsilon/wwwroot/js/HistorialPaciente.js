jQuery(function () {

    /* POST: Añadir paciente */
    jqGenerarZip = () => {

        alert("Llega");

        //try {
        //    $.ajax({
        //        type: 'POST',
        //        url: 'Pacientes/GenerarZipHistorial',
        //        data: new FormData(form),
        //        contentType: false,
        //        processData: false,
        //        success: function (response) {

        //            alert("Zip generado correctamente");
        //        },

        //        error: function (response) {

        //            alert("Ha ocurrido un error");
        //        }
        //    })
        //    return false;
        //} catch (ex) {
        //    console.log(ex);
        //}
    }

    jqExportarHistorial = (idPaciente) => {

        try {

            const tipoExportacion = $("#modelosExportacion").val();

            let url = "";

            switch (tipoExportacion) {

                case "txt":
                    url = `/Pacientes/GenerarTxtHistorial?idPaciente=${idPaciente}`;
                    break;

                case "pdfListado":
                    url = `/Pacientes/GenerarListadoPDFHistorial?idPaciente=${idPaciente}`;
                    break;

                case "pdfTabla":
                    url = `/Pacientes/GenerarPDFTablaHistorial?idPaciente=${idPaciente}`;
                    break;

                case "pdfListaZip":
                    url = `/Pacientes/GenerarListadoEnZip?idPaciente=${idPaciente}`;
                    break;

                /*Este de momento lo dejamos aparte*/
                case "informe":
                    url = `/Pacientes/InformeHistorial?idPaciente=${idPaciente}`;
                    break;

                default:
                    alert("Seleccione una exportación válida");
                    return false;
            }

            window.location.href = url;
            alert("Todo correcto");
            return false;

        } catch (ex) {

            console.log(ex);
        }
    }

});
