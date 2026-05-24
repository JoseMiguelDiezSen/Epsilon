jQuery(function () {

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
            return false;

        } catch (ex) {

            console.log(ex);
        }
    }

});
