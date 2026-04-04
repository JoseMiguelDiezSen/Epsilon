const viewer = document.querySelector('model-viewer');
const arButton = document.querySelector('#ar-button');

arButton.addEventListener('click', () => {
    if (viewer.canActivateAR) {
        viewer.activateAR();
    } else {
        alert('La realidad aumentada no está disponible en este dispositivo.');
    }
});







const viewer = document.getElementById("visor3d");

function abrirModelo(glb, hdr) {
    viewer.src = glb;
    viewer.environmentImage = hdr;

    document.getElementById("modal3d").style.display = "block";
}

function cerrarModal() {
    document.getElementById("modal3d").style.display = "none";
}
