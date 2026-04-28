$(document).ready(function () {

    /*Script de los colores*/
    $(".botonColores")
        .css("content", "''")
        .css("background", "linear-gradient(45deg,#ff0000,#ff7300,#fffb00,#48ff00,#00ffd5,#002bff,#7a00ff,#ff00c8,#ff0000)")

        /*.css("position", "absolute")*/
        //.css("top", "-2px")
        //.css("left", "-2px")

        .css("background-size", "400%")

        /* .css("z-index","-1")*/

        /* Efecto Difuminacion */
        /*.css("filter", "blur(0px)")*/
        .css("filter", "blur(5px)")
        //.css("filter", "blur(10px)")

        .css("width", "calc(100%+4px)")
        .css("height", "calc(100%+4px)")

        /* Velocidad de los colores*/
        //.css("animation", "glowing 10s linear infinite")
        //.css("animation", "glowing 20s linear infinite")

        .css("animation", "glowing 40s linear infinite")
        .css("transition", "opacity .3s ease-in-out")
        .css("color", "#000")
        .css("opacity", "1")
});