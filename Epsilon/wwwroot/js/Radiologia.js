jQuery(function () {

    const MIN_SCALE = 0.3;
    const MAX_SCALE = 5;

    let points = [];
    let currentImage = null;

    let rectStart = null;
    let rectEnd = null;
    let rectFinished = false;
    let rectState = 0; // 0 = nada, 1 = inicio, 2 = cerrado

    let measurePoints = [];
    // Indica si las herramientas de interacción están bloqueadas
    // true  = bloqueadas
    // false = activas
    let locked = false;

    let pixelSpacing = [1, 1];

    // Configuracion del tamaño mininmo de la radiografia para evitar que desaparezca
    const clampScale = (value) => {
        return Math.min(MAX_SCALE, Math.max(MIN_SCALE, value));
    };

    // INICIALIZACIÓN DE CORNERSTONE Y DEPENDENCIAS
    
    // Se enlazan las librerías externas que utiliza CornerstoneTools
    cornerstoneTools.external.cornerstone = cornerstone;
    cornerstoneTools.external.Hammer = Hammer;
    cornerstoneTools.external.cornerstoneMath = cornerstoneMath;

    // Inicializa CornerstoneTools
    cornerstoneTools.init();

    // Se enlazan las dependencias necesarias para cargar imágenes DICOM
    cornerstoneWADOImageLoader.external.cornerstone = cornerstone;
    cornerstoneWADOImageLoader.external.dicomParser = dicomParser;

    // Registra el cargador de imágenes DICOM mediante protocolo WADO-URI
    cornerstone.registerImageLoader('wadouri',cornerstoneWADOImageLoader.wadouri.loadImage);

    // Configuración de WebWorkers para procesar imágenes DICOM
    cornerstoneWADOImageLoader.webWorkerManager.initialize({

        // Número máximo de workers simultáneos
        maxWebWorkers: 1,

        // Los workers se crean únicamente cuando son necesarios
        startWebWorkersOnDemand: true,

        // Permite utilizar WebWorkers para decodificar imágenes
        decodeConfig: {
            useWebWorkers: true
        },

        taskConfiguration: {

            // Configuración de la tarea de decodificación DICOM
            decodeTask: {

                // Evita cargar todos los códecs al arrancar la página
                initializeCodecsOnStartup: false
            }
        }
    });

    // Obtiene la referencia al contenedor HTML donde se mostrará la radiografía
    const element = document.getElementById("dicomViewer");

    // Habilita el contenedor para que Cornerstone pueda renderizar imágenes DICOM
    cornerstone.enable(element);

    // Herramienta de desplazamiento (Pan)
    cornerstoneTools.addTool(cornerstoneTools.PanTool);

    // Herramienta de zoom mediante rueda del ratón
    cornerstoneTools.addTool(cornerstoneTools.ZoomMouseWheelTool);

    // Control de bloqueo
    const applyToolState = () => {

        if (locked) {
            // Deshabilita las herramientas de Cornerstone
            cornerstoneTools.setToolDisabled("Pan");
            cornerstoneTools.setToolDisabled("ZoomMouseWheel");

            // Deshabilita controles de la interfaz
            $("#zoomIn, #zoomOut, #tamanio, #brightness, #contrast").prop("disabled", true);

        } else {
            // Activa desplazamiento con botón izquierdo
            cornerstoneTools.setToolActive("Pan", {mouseButtonMask: 1});

            // Activa zoom con rueda del ratón
            cornerstoneTools.setToolActive("ZoomMouseWheel", { mouseButtonMask: 0 });

            // Habilita controles de la interfaz
            $("#zoomIn, #zoomOut, #tamanio, #brightness, #contrast").prop("disabled", false);
        }
    }

    applyToolState();

    // Actualiza en el visor los valores actuales de los sliders de brillo y contraste tomando como referencia el VOI original de la imagen cargada.
    const updateVOI = () => {

        // Obtiene el viewport actual del visor
        const viewport = cornerstone.getViewport(element);

        // Si no existe viewport, VOI o referencia inicial,
        // no es posible aplicar ajustes.
        if (!viewport || !viewport.voi || !baseVOI) return;

        // Obtiene los valores actuales de los sliders
        const brightness = parseInt($("#brightness").val() || 0);
        const contrast = parseInt($("#contrast").val() || 0);

        // Ajusta brillo (Window Center)
        viewport.voi.windowCenter = baseVOI.windowCenter - brightness;

        // Ajusta contraste (Window Width)
        // Se fuerza un mínimo de 1 para evitar valores inválidos.
        viewport.voi.windowWidth = Math.max(1, baseVOI.windowWidth + contrast);

        // Aplica los cambios al visor
        cornerstone.setViewport(element, viewport);
    }

    // -------------------- CONTROLES ---------------//

    // Sincroniza los sliders de brillo y contraste con el visor DICOM. Cada modificación ejecuta updateVOI() y actualiza la imagen en tiempo real.
    $("#brightness, #contrast").on("input", updateVOI);

    // Incrementa el nivel de zoom del visor.
    $("#zoomIn").click(function () {

        const viewport = cornerstone.getViewport(element);

        // Si no hay imagen cargada no se realiza ninguna acción
        if (!viewport) return;

        viewport.scale = clampScale(viewport.scale + 0.1);

        cornerstone.setViewport(element, viewport);
    });

    // Reduce el nivel de zoom del visor.
    $("#zoomOut").click(function () {

        const viewport = cornerstone.getViewport(element);

        // Si no hay imagen cargada no se realiza ninguna acción
        if (!viewport) return;

        viewport.scale = clampScale(viewport.scale - 0.1);

        cornerstone.setViewport(element, viewport);
    });

    // Boton de reset
    $("#reset").click(function () {

        //if (!currentImage || !baseVOI) return;

        //cornerstone.displayImage(element, currentImage);

        //requestAnimationFrame(() => {

        //    const viewport = cornerstone.getViewport(element);
        //    if (!viewport) return;

        //    viewport.voi.windowCenter = baseVOI.windowCenter;
        //    viewport.voi.windowWidth = baseVOI.windowWidth;
        //    viewport.scale = baseVOI.scale || 1;

        //    cornerstone.setViewport(element, viewport);

        //    $("#brightness").val(0);
        //    $("#contrast").val(0);
        //    $("#tamanio").val(50);
        //});
    });

    // Ajusta directamente la escala del visor a partir del valor seleccionado en el slider.
    $("#tamanio").on("input", function () {

        const viewport = cornerstone.getViewport(element);
        if (!viewport) return;

        const value = $(this).val();

        viewport.scale = clampScale(value / 50);

        cornerstone.setViewport(element, viewport);
    });

    // Bloqueo de DCM
    $("#toggleBtn").change(function () {

        locked = this.checked;

        $("#toggleState").text(locked ? "🔒" : "🔓");

        applyToolState();
    });

    /// ----------------------------- FIN CONTROLES----------------------------------------//
    abrirVisorFullScreen = function () {

        const visor = document.getElementById("dicomViewer");

        visor.requestFullscreen();

        setTimeout(function () {
            cornerstone.resize(visor, true);
        }, 150);
    };

    mostrarTexto = (texto) => {
        document.getElementById("textoVisor").innerText = texto;
    }

    // Se ejecuta cuando el usuario selecciona una radiografía distinta en el desplegable.
    $("#listadoRadiografias").change(function () {

        // Muestra mensaje temporal mientras se carga la imagen
        mostrarTexto("Cargando visor...");

        // Obtiene la ruta DICOM seleccionada
        const rutaDicom = $(this).val();

        // Construye la URL completa que utilizará Cornerstone
        const imageId = "wadouri:https://localhost:7176" + rutaDicom;

        // Carga la imagen DICOM seleccionada
        cornerstone.loadImage(imageId).then(function (image) {

            // Renderiza la imagen en el visor
            cornerstone.displayImage(element, image);
            toolMode = "draw";
            setTimeout(drawPoints, 0);

            const spacingTag = image.data.string('x00280030');

            pixelSpacing = spacingTag
                ? spacingTag.split("\\").map(Number)
                : [1, 1];

            currentImage = image;

            // Obtiene el viewport actual de la imagen recién cargada
            const viewport = cornerstone.getViewport(element);

            // Guarda los valores VOI originales de la imagen
            // para poder utilizarlos posteriormente como referencia
            // en ajustes de brillo, contraste o reset.
            baseVOI = {
                windowCenter: viewport.voi.windowCenter,
                windowWidth: viewport.voi.windowWidth
            };

            // Oculta el mensaje de carga
            mostrarTexto("");
        });

    });


    // DIBUJO _ MEDICION
    //element.addEventListener("mousedown", function (e) {

    //    const coords = cornerstone.pageToPixel(element, e.clientX, e.clientY);

    //    if (measurePoints.length === 2) {
    //        measurePoints = [];
    //    }

    //    measurePoints.push(coords);

    //    cornerstone.updateImage(element);
    //});

    // DIBUJO - Linea de Medicion
    //element.addEventListener("cornerstoneimagerendered", function (e) {

    //    const ctx = e.detail.canvasContext;
    //    if (!ctx) return;

    //    ctx.save();
    //    ctx.strokeStyle = "red";
    //    ctx.fillStyle = "red";

    //    if (measurePoints.length === 2) {

    //        const p1 = measurePoints[0];
    //        const p2 = measurePoints[1];

    //        ctx.beginPath();
    //        ctx.moveTo(p1.x, p1.y);
    //        ctx.lineTo(p2.x, p2.y);
    //        ctx.stroke();

    //        const dist = getDistanceMM(p1, p2).toFixed(2);
    //        ctx.fillText(dist + " mm", p2.x + 5, p2.y + 5);
    //    }

    //    ctx.restore();
    //});
    element.addEventListener("mousedown", function (e) {

        if (toolMode === "none") return;

        const coords = cornerstone.pageToPixel(element, e.clientX, e.clientY);

        if (toolMode === "point") {

            measurePoints.push(coords);
        }

        if (toolMode === "line") {

            if (measurePoints.length === 2) {
                measurePoints = [];
            }

            measurePoints.push(coords);
        }

        if (toolMode === "rectangle") {

            const coords = cornerstone.pageToPixel(element, e.clientX, e.clientY);

            // 0 -> inicio
            if (!rectStart) {
                rectStart = coords;
                rectEnd = null;
                return;
            }

            // 1 -> cierre
            if (!rectEnd) {
                rectEnd = coords;
                cornerstone.updateImage(element);
                return;
            }

            // 2 -> reset + nuevo inicio
            rectStart = coords;
            rectEnd = null;
        }

        cornerstone.updateImage(element);
    });




    element.addEventListener("cornerstoneimagerendered", function (e) {

        const ctx = e.detail.canvasContext;
        if (!ctx) return;

        ctx.save();
        ctx.strokeStyle = "red";
        ctx.fillStyle = "red";

        // PUNTOS (modo point o line)
        if (toolMode === "point" || toolMode === "line") {

            measurePoints.forEach(p => {
                ctx.beginPath();
                ctx.arc(p.x, p.y, 4, 0, Math.PI * 2);
                ctx.fill();
            });
        }

        // LÍNEA
        if (toolMode === "line" && measurePoints.length === 2) {

            const p1 = measurePoints[0];
            const p2 = measurePoints[1];

            ctx.beginPath();
            ctx.moveTo(p1.x, p1.y);
            ctx.lineTo(p2.x, p2.y);
            ctx.stroke();

            const dist = getDistanceMM(p1, p2).toFixed(2);
            ctx.fillText(dist + " mm", p2.x + 5, p2.y + 5);
        }

        // RECTÁNGULO
        if (toolMode === "rectangle" && rectStart && rectEnd) {

            const p1 = rectStart;
            const p2 = rectEnd || rectStart;

            const x = Math.min(p1.x, p2.x);
            const y = Math.min(p1.y, p2.y);
            const w = Math.abs(p1.x - p2.x);
            const h = Math.abs(p1.y - p2.y);

            ctx.strokeStyle = "yellow";
            ctx.strokeRect(x, y, w, h);
        }

        ctx.restore();
    });

    // Calcular Distancia
    function getDistanceMM(p1, p2) {
        const dx = (p2.x - p1.x) * pixelSpacing[1];
        const dy = (p2.y - p1.y) * pixelSpacing[0];
        return Math.sqrt(dx * dx + dy * dy);
    }

    // DIBUJO _ PUNTOS
    //element.addEventListener("mousedown", function (e) {
  
    //    const coords = cornerstone.pageToPixel(element, e.pageX, e.pageY);

    //    points.push(coords);

    //    cornerstone.updateImage(element);
    //});


    // DIBUJO - Puntos
    //element.addEventListener("cornerstoneimagerendered", function (e) {
    //    console.log("RENDER OK");
    //    const enabledElement = cornerstone.getEnabledElement(element);
    //    const context = enabledElement.canvas.getContext("2d");

    //    context.save();
    //    context.fillStyle = "red";

    //    points.forEach(p => {
    //        context.beginPath();
    //        context.arc(p.x, p.y, 5, 0, Math.PI * 2);
    //        context.fill();
    //    });

    //    context.restore();
    //});


    // Dibujar Puntos
    function drawPoints() {

        const enabledElement = cornerstone.getEnabledElement(element);
        if (!enabledElement || !enabledElement.canvas) return;

        const ctx = enabledElement.canvas.getContext("2d");

        ctx.save();
        ctx.fillStyle = "red";

        points.forEach(p => {
            ctx.beginPath();
            ctx.arc(p.x, p.y, 5, 0, Math.PI * 2);
            ctx.fill();
        });
        ctx.restore();
    }

    $("#toolMode").change(function () {
        toolMode = $(this).val();
        measurePoints = [];
        cornerstone.updateImage(element);
    });


    //////////////////////////////////////////////////////////////////

    // Cuando el visor sale de pantalla completa,se debe recalcular el tamaño para adaptarse al nuevo espacio disponible.
    document.addEventListener("fullscreenchange", function () {
        setTimeout(function () {
            cornerstone.resize(element, true);
        }, 50);
    });

    // =========================
    // RESIZE OBSERVER (AJUSTE AUTOMÁTICO)
    // =========================
    //
    // Observa cambios en el tamaño del contenedor del visor
    // (dicomViewer) y ajusta automáticamente el render de Cornerstone.
    //
    // Esto evita problemas de escalado cuando:
    // - cambia el tamaño de la ventana
    // - se activa/desactiva fullscreen
    // - cambia el layout de la página
    // - se redimensionan paneles o contenedores
    const observer = new ResizeObserver(() => {

        // Recalcula el tamaño del visor DICOM
        cornerstone.resize(element, true);

    });

    // Activa la observación del elemento del visor
    observer.observe(element);
});