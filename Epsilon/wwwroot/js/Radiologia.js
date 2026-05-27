jQuery(function () {

    /* Script para la lectura de radiografias */
    cornerstoneWADOImageLoader.external.cornerstone = cornerstone;
    cornerstoneWADOImageLoader.external.dicomParser = dicomParser;

    cornerstone.registerImageLoader(
        'wadouri',
        cornerstoneWADOImageLoader.wadouri.loadImage
    );

    cornerstoneWADOImageLoader.webWorkerManager.initialize({
        maxWebWorkers: 1,
        startWebWorkersOnDemand: true,
        decodeConfig: {
            useWebWorkers: true
        },
        taskConfiguration: {
            decodeTask: {
                initializeCodecsOnStartup: false
            }
        }
    });

    const element = document.getElementById("dicomViewer");
    cornerstone.enable(element);

    /* Muestra el texto de estado del visor */
    mostrarTexto = (texto) => {
        document.getElementById("textoVisor").innerText = texto;
    }

    /* Cambio de radiografia */
    $("#listadoRadiografias").change(function () {

        mostrarTexto("Cargando visor...");

        const rutaDicom = $(this).val();
        const imageId = "wadouri:https://localhost:7176" + rutaDicom;

        cornerstone.loadImage(imageId).then(function (image) {
            cornerstone.displayImage(element, image);
            mostrarTexto("");
        });

    });

    /* FullScreen */
    abrirVisorFullScreen = () => {

        const visor = document.getElementById("dicomViewer");

        if (visor.requestFullscreen) {

            visor.requestFullscreen();

            setTimeout(function () {
                cornerstone.resize(visor, true);
            }, 200);

        }

    }

});