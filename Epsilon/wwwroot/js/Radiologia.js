jQuery(function () {

    /*Script para la lectura de radiografias*/
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

    const imageId = "wadouri:https://localhost:7176/Media/Radiografias/1.dcm";

    cornerstone.loadImage(imageId).then(function (image) {
        cornerstone.displayImage(element, image);
    });


    /*Funcion que pretende abrir fullScreen con radiografia, no se ejecuta aun */
    abrirVisorFullScreen = () => {
        alert("Visor abriendo");
    }
});