
class Examples {

    // 1️ Propiedades / campos
    // Variables internas de la clase (pueden ser públicas o privadas)
    #campoPrivado;
    campoPublico;

    // 2️ Constructor
    // Inicializa la clase, recibe parámetros necesarios
    constructor(param1, param2) {
        this.campoPublico = param1;
        this.#campoPrivado = param2;
    }

    // 3️ Métodos privados
    // Funciones auxiliares internas que no se usan fuera de la clase
    #metodoPrivado() {
        // ...
    }

    // 4️ Métodos públicos
    // Funciones que interactúan con la clase desde fuera
    metodoPublico() {
        // ...
        this.#metodoPrivado();
    }

    // 5️ Getters y setters
    // Acceso controlado a propiedades privadas
    get propiedad() {
        return this.#campoPrivado;
    }

    set propiedad(valor) {
        this.#campoPrivado = valor;
    }

    // 6️ Métodos estáticos (opcionales)
    // Funciones relacionadas con la clase, no con instancias
    static metodoEstatico() {
        // ...
    }
}