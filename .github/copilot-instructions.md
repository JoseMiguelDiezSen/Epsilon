# Copilot Instructions

## General Guidelines
- Preferir mover JavaScript inline de vistas Razor a `wwwroot/js/Usuarios.js`.
- Usar delegación de eventos y evitar `onclick` inline para prevenir conflictos entre procesos.

## Code Style
- Manejar casos en los que la fila de detalle no exista creando la fila dinámicamente y comprobando null antes de acceder a `style`.