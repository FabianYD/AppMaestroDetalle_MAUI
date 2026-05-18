# App Gestión de Pedidos (Maestro/Detalle) con SQLite en .NET MAUI

Este proyecto es un ejemplo práctico de cómo implementar una interfaz **Maestro/Detalle** en una sola pantalla utilizando **.NET MAUI** y **SQLite** (a través de Dapper).

El diseño está fuertemente inspirado en una interfaz de gestión rápida, donde puedes construir el detalle de forma temporal en memoria antes de afectar la base de datos.

## Características

*   **Patrón Maestro/Detalle**: Gestión de dos tablas relacionadas (`Pedidos` y `LineasPedido`).
*   **Construcción en Memoria**: Vas añadiendo productos a una lista visual y solo cuando presionas "Crear" o "Actualizar", se refleja en la base de datos de una sola vez.
*   **Base de datos Local**: Uso de `Microsoft.Data.Sqlite` para almacenamiento local.
*   **Micro ORM**: Uso de `Dapper` para ejecutar consultas SQL de forma rápida.
*   **Diseño Limpio**: Uso de un diseño con tarjetas (Frames) esquinas redondeadas e iconos o emojis para una vista más amigable.

## Capturas de Pantalla

*(Reemplaza las siguientes imágenes con las capturas de tu aplicación en ejecución)*

### Pantalla Principal
![Pantalla Principal](ruta/a/tu/captura_principal.png)
> *Vista de la aplicación lista para empezar a llenar un pedido.*

### Añadiendo y Gestionando
![Agregando Datos](ruta/a/tu/captura_agregando.png)
> *Demostración de cómo se agrega líneas, y el uso del CRUD inferior.*

## Cómo ejecutar

1. Clona o descarga este repositorio.
2. Abre la solución en Visual Studio.
3. Restaura los paquetes NuGet.
4. Ejecuta la aplicación en tu emulador Android o en Windows Machine.
