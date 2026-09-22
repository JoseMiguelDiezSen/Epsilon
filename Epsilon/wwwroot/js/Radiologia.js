jQuery(function () {

    const MIN_SCALE = 0.2;
    const MAX_SCALE = 5;

    // Estado del visor y de las herramientas
    let currentImage = null;
    let baseVOI = null;
    let locked = false;
    let toolMode = "none";
    let pixelSpacing = [1, 1];

    // Herramienta Puntos
    let points = [];

    // Herramienta Línea (Medición)
    let linePoints = [];
    let linePreviewPoint = null;

    // Herramienta Rectángulo
    let isDrawingRect = false;
    let rectStart = null;
    let rectEnd = null;
    let rectPreview = null;

    // Limitador de escala de zoom
    const clampScale = (value) => {
        return Math.min(MAX_SCALE, Math.max(MIN_SCALE, value));
    };

    // ==========================================================
    // INICIALIZACIÓN DE CORNERSTONE Y DEPENDENCIAS
    // ==========================================================
    cornerstoneTools.external.cornerstone = cornerstone;
    cornerstoneTools.external.Hammer = Hammer;
    cornerstoneTools.external.cornerstoneMath = cornerstoneMath;
    cornerstoneTools.init();

    cornerstoneWADOImageLoader.external.cornerstone = cornerstone;
    cornerstoneWADOImageLoader.external.dicomParser = dicomParser;
    cornerstone.registerImageLoader('wadouri', cornerstoneWADOImageLoader.wadouri.loadImage);

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

    // Herramientas nativas de navegación
    cornerstoneTools.addTool(cornerstoneTools.PanTool);
    cornerstoneTools.addTool(cornerstoneTools.ZoomMouseWheelTool);

    // ==========================================================
    // ACTUALIZACIÓN DE HUD Y ESTADOS
    // ==========================================================
    const updateHUD = () => {
        if (!currentImage) return;
        const viewport = cornerstone.getViewport(element);
        if (!viewport) return;

        // Zoom y modo
        const zoomPercent = Math.round((viewport.scale || 1) * 100);
        let modoTexto = "PAN";
        if (toolMode === "point") modoTexto = "PUNTO";
        else if (toolMode === "line") modoTexto = "LÍNEA";
        else if (toolMode === "rectangle") modoTexto = isDrawingRect ? "DIBUJANDO..." : "RECTÁNGULO";

        $("#hudZoom").text(`ZOOM: ${zoomPercent}% | MODO: ${modoTexto}`);

        // Ventana (VOI)
        if (viewport.voi) {
            const wc = Math.round(viewport.voi.windowCenter);
            const ww = Math.round(viewport.voi.windowWidth);
            $("#hudVOI").text(`WC: ${wc} | WW: ${ww}`);
        }
    };

    const applyToolState = () => {
        if (locked) {
            cornerstoneTools.setToolDisabled("Pan");
            cornerstoneTools.setToolDisabled("ZoomMouseWheel");
            $("#zoomIn, #zoomOut, #tamanio, #fs_tamanio, #brightness, #fs_brightness, #contrast, #fs_contrast, #toolMode, #fs_toolMode, #reset, #fs_reset, #limpiarDibujos, #fs_limpiarDibujos").prop("disabled", true);
            $("#dicomViewer").removeClass("drawing-mode");
        } else {
            $("#zoomIn, #zoomOut, #tamanio, #fs_tamanio, #brightness, #fs_brightness, #contrast, #fs_contrast, #toolMode, #fs_toolMode, #reset, #fs_reset, #limpiarDibujos, #fs_limpiarDibujos").prop("disabled", false);
            cornerstoneTools.setToolActive("ZoomMouseWheel", { mouseButtonMask: 0 });

            if (toolMode === "none") {
                cornerstoneTools.setToolActive("Pan", { mouseButtonMask: 1 });
                $("#dicomViewer").removeClass("drawing-mode");
            } else {
                // Al estar dibujando, desactivamos Pan para que no interfiera el arrastre
                cornerstoneTools.setToolDisabled("Pan");
                $("#dicomViewer").addClass("drawing-mode");
            }
        }
        updateHUD();
    };

    applyToolState();

    // ==========================================================
    // AJUSTES VOI (BRILLO Y CONTRASTE) - SINCRONIZADOS
    // ==========================================================
    const updateVOI = (source) => {
        const viewport = cornerstone.getViewport(element);
        if (!viewport || !viewport.voi || !baseVOI) return;

        let brightness, contrast;

        if (source === "fullscreen") {
            brightness = parseInt($("#fs_brightness").val() || 0);
            contrast = parseInt($("#fs_contrast").val() || 0);
            $("#brightness").val(brightness);
            $("#contrast").val(contrast);
        } else {
            brightness = parseInt($("#brightness").val() || 0);
            contrast = parseInt($("#contrast").val() || 0);
            $("#fs_brightness").val(brightness);
            $("#fs_contrast").val(contrast);
        }

        viewport.voi.windowCenter = baseVOI.windowCenter - brightness;
        viewport.voi.windowWidth = Math.max(1, baseVOI.windowWidth + (contrast - 2000));

        cornerstone.setViewport(element, viewport);
        updateHUD();
    };

    $("#brightness, #contrast").on("input", () => updateVOI("base"));
    $("#fs_brightness, #fs_contrast").on("input", () => updateVOI("fullscreen"));

    // ==========================================================
    // CONTROLES DE ZOOM Y RESET - SINCRONIZADOS
    // ==========================================================
    $("#zoomIn").click(function () {
        const viewport = cornerstone.getViewport(element);
        if (!viewport) return;
        viewport.scale = clampScale(viewport.scale + 0.15);
        cornerstone.setViewport(element, viewport);
        const scaleVal = Math.round(viewport.scale * 50);
        $("#tamanio, #fs_tamanio").val(scaleVal);
        updateHUD();
    });

    $("#zoomOut").click(function () {
        const viewport = cornerstone.getViewport(element);
        if (!viewport) return;
        viewport.scale = clampScale(viewport.scale - 0.15);
        cornerstone.setViewport(element, viewport);
        const scaleVal = Math.round(viewport.scale * 50);
        $("#tamanio, #fs_tamanio").val(scaleVal);
        updateHUD();
    });

    $("#tamanio").on("input", function () {
        const viewport = cornerstone.getViewport(element);
        if (!viewport) return;
        const val = $(this).val();
        $("#fs_tamanio").val(val);
        viewport.scale = clampScale(val / 50);
        cornerstone.setViewport(element, viewport);
        updateHUD();
    });

    $("#fs_tamanio").on("input", function () {
        const viewport = cornerstone.getViewport(element);
        if (!viewport) return;
        const val = $(this).val();
        $("#tamanio").val(val);
        viewport.scale = clampScale(val / 50);
        cornerstone.setViewport(element, viewport);
        updateHUD();
    });

    // ==========================================================
    // LIMPIAR ANOTACIONES Y RESET
    // ==========================================================
    const limpiarDibujos = () => {
        points = [];
        linePoints = [];
        linePreviewPoint = null;
        rectStart = null;
        rectEnd = null;
        rectPreview = null;
        isDrawingRect = false;

        cornerstone.updateImage(element);
        updateHUD();
    };

    $("#limpiarDibujos, #fs_limpiarDibujos").click(limpiarDibujos);

    $("#reset, #fs_reset").click(function () {
        if (!currentImage || !baseVOI) return;

        const viewport = cornerstone.getViewport(element);
        if (viewport) {
            viewport.voi.windowCenter = baseVOI.windowCenter;
            viewport.voi.windowWidth = baseVOI.windowWidth;
            viewport.scale = baseVOI.scale || 1;
            viewport.translation.x = 0;
            viewport.translation.y = 0;
            cornerstone.setViewport(element, viewport);
        }

        $("#brightness, #fs_brightness").val(0);
        $("#contrast, #fs_contrast").val(2000);
        $("#tamanio, #fs_tamanio").val(50);
        $("#toolMode, #fs_toolMode").val("none");
        toolMode = "none";
        applyToolState();

        limpiarDibujos();
    });

    $("#toggleBtn").change(function () {
        locked = this.checked;
        $("#toggleState").text(locked ? "🔒" : "🔓");
        applyToolState();
    });

    const setModoHerramienta = (nuevoModo) => {
        toolMode = nuevoModo;
        $("#toolMode").val(toolMode);
        $("#fs_toolMode").val(toolMode);
        // Cancelar dibujos en curso si cambia de herramienta
        isDrawingRect = false;
        rectPreview = null;
        linePreviewPoint = null;
        applyToolState();
        cornerstone.updateImage(element);
    };

    $("#toolMode").change(function () {
        setModoHerramienta($(this).val());
    });

    $("#fs_toolMode").change(function () {
        setModoHerramienta($(this).val());
    });

    // Cancelar dibujo en curso con tecla Escape
    $(document).on("keydown", function (e) {
        if (e.key === "Escape") {
            if (isDrawingRect) {
                isDrawingRect = false;
                rectStart = null;
                rectPreview = null;
                cornerstone.updateImage(element);
                updateHUD();
            } else if (linePoints.length === 1) {
                linePoints = [];
                linePreviewPoint = null;
                cornerstone.updateImage(element);
                updateHUD();
            }
        }
    });

    // ==========================================================
    // CARGA DE RADIOGRAFÍAS (DICOM)
    // ==========================================================
    const mostrarCargador = (mostrar, texto) => {
        const $spinner = $("#textoVisor");
        if (mostrar) {
            $("#textoVisorMsg").text(texto || "Cargando estudio DICOM...");
            $spinner.addClass("show");
        } else {
            $spinner.removeClass("show");
        }
    };

    const cargarRadiografia = (rutaDicom, studyTitle) => {
        if (!rutaDicom) return;

        mostrarCargador(true, "Cargando radiografía...");

        // Construir URL absoluta dinámica según el origen actual
        const origin = window.location.origin;
        const imageId = "wadouri:" + origin + (rutaDicom.startsWith("/") ? "" : "/") + rutaDicom;

        cornerstone.loadImage(imageId).then(function (image) {
            currentImage = image;
            cornerstone.displayImage(element, image);

            // Leer espaciado de píxel de los metadatos DICOM (tag 0028,0030)
            const spacingTag = image.data ? image.data.string('x00280030') : null;
            pixelSpacing = spacingTag ? spacingTag.split("\\").map(Number) : [1, 1];
            if (isNaN(pixelSpacing[0]) || isNaN(pixelSpacing[1])) {
                pixelSpacing = [1, 1];
            }

            const viewport = cornerstone.getViewport(element);
            baseVOI = {
                windowCenter: viewport.voi.windowCenter,
                windowWidth: viewport.voi.windowWidth,
                scale: viewport.scale || 1
            };

            // Sincronizar controles (base y pantalla completa)
            $("#brightness, #fs_brightness").val(0);
            $("#contrast, #fs_contrast").val(2000);
            const initialScale = Math.round(viewport.scale * 50);
            $("#tamanio, #fs_tamanio").val(initialScale);

            // Sincronizar HUD
            $("#hudStudy").text("ESTUDIO: " + (studyTitle || rutaDicom.split("/").pop()));
            updateHUD();

            // Sincronizar miniaturas y selectores
            $(".thumbnail-item").removeClass("active");
            $(`.thumbnail-item[data-dcm='${rutaDicom}']`).addClass("active");
            $("#listadoRadiografias").val(rutaDicom);

            mostrarCargador(false);
        }).catch(function (error) {
            console.error("Error al cargar la imagen DICOM:", error);
            mostrarCargador(true, "Error al cargar la imagen DICOM.");
            setTimeout(() => mostrarCargador(false), 3000);
        });
    };

    // Eventos de selección de radiografía
    $("#listadoRadiografias").change(function () {
        const rutaDicom = $(this).val();
        const texto = $(this).find("option:selected").text();
        cargarRadiografia(rutaDicom, texto);
    });

    $(".thumbnail-item").on("click", function () {
        const rutaDicom = $(this).data("dcm");
        const code = $(this).data("code");
        const date = $(this).data("date");
        cargarRadiografia(rutaDicom, `${code} (${date})`);
    });

    // Cargar automáticamente el primer estudio si existe
    const primerEstudio = $("#listadoRadiografias option:not([disabled])").first().val();
    if (primerEstudio) {
        const primerTexto = $("#listadoRadiografias option:not([disabled])").first().text();
        cargarRadiografia(primerEstudio, primerTexto);
    }

    // ==========================================================
    // INTERACCIÓN DE DIBUJO Y MEDICIÓN (MOUSEDOWN / MOUSEMOVE)
    // ==========================================================

    // Clavado e inicio de puntos / rectángulos
    element.addEventListener("mousedown", function (e) {
        if (e.button !== 0 || locked || !currentImage || toolMode === "none") return;

        const coords = cornerstone.pageToPixel(element, e.clientX, e.clientY);

        // 1. MODO PUNTO
        if (toolMode === "point") {
            points.push(coords);
            cornerstone.updateImage(element);
            return;
        }

        // 2. MODO LÍNEA (Medición en 2 clics)
        if (toolMode === "line") {
            if (linePoints.length === 0 || linePoints.length === 2) {
                linePoints = [coords];
                linePreviewPoint = coords;
            } else if (linePoints.length === 1) {
                linePoints.push(coords);
                linePreviewPoint = null;
            }
            cornerstone.updateImage(element);
            return;
        }

        // 3. MODO RECTÁNGULO (2 clics: 1º inicia y expande en tiempo real, 2º clava)
        if (toolMode === "rectangle") {
            if (!isDrawingRect) {
                // PRIMER CLIC: Fija inicio y empieza el modo interactivo
                rectStart = coords;
                rectPreview = coords;
                rectEnd = null;
                isDrawingRect = true;
            } else {
                // SEGUNDO CLIC: Fija el final y clava el rectángulo
                rectEnd = coords;
                rectPreview = null;
                isDrawingRect = false;
            }
            cornerstone.updateImage(element);
            updateHUD();
            return;
        }
    });

    // Desplazamiento dinámico (mousemove) para ver cómo se va ampliando el rectángulo
    element.addEventListener("mousemove", function (e) {
        if (locked || !currentImage || toolMode === "none") return;

        const coords = cornerstone.pageToPixel(element, e.clientX, e.clientY);

        if (toolMode === "rectangle" && isDrawingRect && rectStart) {
            rectPreview = coords;
            cornerstone.updateImage(element);
        } else if (toolMode === "line" && linePoints.length === 1) {
            linePreviewPoint = coords;
            cornerstone.updateImage(element);
        }
    });

    // ==========================================================
    // RENDERIZADO EN CANVAS (CORNERSTONEIMAGERENDERED)
    // ==========================================================
    element.addEventListener("cornerstoneimagerendered", function (e) {
        const ctx = e.detail.canvasContext;
        if (!ctx) return;

        ctx.save();

        // 1. RENDERIZAR PUNTOS
        if (points.length > 0) {
            points.forEach((p, idx) => {
                ctx.beginPath();
                ctx.arc(p.x, p.y, 4.5, 0, Math.PI * 2);
                ctx.fillStyle = "#ef4444";
                ctx.fill();
                ctx.strokeStyle = "#ffffff";
                ctx.lineWidth = 1.5;
                ctx.stroke();
            });
        }

        // 2. RENDERIZAR LÍNEA
        if (linePoints.length >= 1) {
            const p1 = linePoints[0];
            const p2 = linePoints.length === 2 ? linePoints[1] : linePreviewPoint;

            if (p1 && p2) {
                const isPreview = linePoints.length === 1;

                ctx.beginPath();
                ctx.moveTo(p1.x, p1.y);
                ctx.lineTo(p2.x, p2.y);
                ctx.strokeStyle = isPreview ? "#38bdf8" : "#22c55e";
                ctx.lineWidth = 2;
                if (isPreview) {
                    ctx.setLineDash([4, 4]);
                } else {
                    ctx.setLineDash([]);
                }
                ctx.stroke();
                ctx.setLineDash([]);

                // Puntos extremos
                [p1, p2].forEach(p => {
                    ctx.beginPath();
                    ctx.arc(p.x, p.y, 3.5, 0, Math.PI * 2);
                    ctx.fillStyle = isPreview ? "#38bdf8" : "#22c55e";
                    ctx.fill();
                });

                // Etiqueta de distancia
                const distMM = getDistanceMM(p1, p2).toFixed(2);
                const midX = (p1.x + p2.x) / 2;
                const midY = (p1.y + p2.y) / 2;
                drawLabelBadge(ctx, `${distMM} mm`, midX + 6, midY - 6);
            }
        }

        // 3. RENDERIZAR RECTÁNGULO
        if (rectStart && (rectEnd || (isDrawingRect && rectPreview))) {
            const p1 = rectStart;
            const p2 = isDrawingRect ? rectPreview : rectEnd;

            if (p1 && p2) {
                const x = Math.min(p1.x, p2.x);
                const y = Math.min(p1.y, p2.y);
                const w = Math.abs(p1.x - p2.x);
                const h = Math.abs(p1.y - p2.y);

                const widthMM = (w * pixelSpacing[1]).toFixed(1);
                const heightMM = (h * pixelSpacing[0]).toFixed(1);

                if (isDrawingRect) {
                    // MIENTRAS SE DESPLAZA: Trazo dinámico discontinuo con relleno sutil
                    ctx.setLineDash([6, 5]);
                    ctx.strokeStyle = "#38bdf8";
                    ctx.lineWidth = 2;
                    ctx.strokeRect(x, y, w, h);

                    ctx.fillStyle = "rgba(56, 189, 248, 0.15)";
                    ctx.fillRect(x, y, w, h);
                    ctx.setLineDash([]);

                    // Etiqueta de tamaño en tiempo real
                    drawLabelBadge(ctx, `${widthMM} x ${heightMM} mm`, x, y - 6);
                } else {
                    // CLAVADO (FIJO): Trazo sólido brillante con anclajes en las 4 esquinas
                    ctx.setLineDash([]);
                    ctx.strokeStyle = "#fbbf24";
                    ctx.lineWidth = 2.5;
                    ctx.strokeRect(x, y, w, h);

                    ctx.fillStyle = "rgba(251, 191, 36, 0.08)";
                    ctx.fillRect(x, y, w, h);

                    // Anclajes en esquinas
                    drawHandle(ctx, x, y);
                    drawHandle(ctx, x + w, y);
                    drawHandle(ctx, x, y + h);
                    drawHandle(ctx, x + w, y + h);

                    // Etiqueta permanente
                    drawLabelBadge(ctx, `ROI: ${widthMM} x ${heightMM} mm`, x, y - 6);
                }
            }
        }

        ctx.restore();
    });

    // ==========================================================
    // FUNCIONES AUXILIARES DE DIBUJO Y CÁLCULO
    // ==========================================================
    function getDistanceMM(p1, p2) {
        const dx = (p2.x - p1.x) * pixelSpacing[1];
        const dy = (p2.y - p1.y) * pixelSpacing[0];
        return Math.sqrt(dx * dx + dy * dy);
    }

    function drawLabelBadge(ctx, text, x, y) {
        ctx.save();
        ctx.font = "bold 11px ui-monospace, SFMono-Regular, Consolas, monospace";
        const metrics = ctx.measureText(text);
        const padding = 5;
        const boxHeight = 18;
        const boxWidth = metrics.width + padding * 2;

        const drawY = Math.max(boxHeight + 2, y);

        // Fondo oscuro del badge
        ctx.fillStyle = "rgba(11, 15, 25, 0.88)";
        ctx.fillRect(x, drawY - boxHeight, boxWidth, boxHeight);

        // Borde fino
        ctx.strokeStyle = "rgba(255, 255, 255, 0.25)";
        ctx.lineWidth = 1;
        ctx.strokeRect(x, drawY - boxHeight, boxWidth, boxHeight);

        // Texto blanco
        ctx.fillStyle = "#ffffff";
        ctx.fillText(text, x + padding, drawY - 5);
        ctx.restore();
    }

    function drawHandle(ctx, x, y) {
        ctx.save();
        ctx.fillStyle = "#fbbf24";
        ctx.fillRect(x - 3.5, y - 3.5, 7, 7);
        ctx.strokeStyle = "#1e293b";
        ctx.lineWidth = 1.5;
        ctx.strokeRect(x - 3.5, y - 3.5, 7, 7);
        ctx.restore();
    }

    // ==========================================================
    // PANTALLA COMPLETA Y RESIZE OBSERVER
    // ==========================================================
    abrirVisorFullScreen = function () {
        const visorWrapper = document.querySelector(".viewer-wrapper");
        if (visorWrapper.requestFullscreen) {
            visorWrapper.requestFullscreen();
        } else if (visorWrapper.webkitRequestFullscreen) {
            visorWrapper.webkitRequestFullscreen();
        }
        $(".viewer-wrapper").addClass("fullscreen-mode");

        setTimeout(function () {
            cornerstone.resize(element, true);
        }, 150);
    };

    salirVisorFullScreen = function () {
        if (document.exitFullscreen) {
            document.exitFullscreen();
        } else if (document.webkitExitFullscreen) {
            document.webkitExitFullscreen();
        }
        $(".viewer-wrapper").removeClass("fullscreen-mode");
    };

    document.addEventListener("fullscreenchange", function () {
        const isFs = !!(document.fullscreenElement || document.webkitFullscreenElement);
        $(".viewer-wrapper").toggleClass("fullscreen-mode", isFs);
        setTimeout(function () {
            cornerstone.resize(element, true);
        }, 80);
    });

    const observer = new ResizeObserver(() => {
        cornerstone.resize(element, true);
    });
    observer.observe(element);
});