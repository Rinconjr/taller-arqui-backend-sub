
# .NET Suscriptor (RabbitMQ a WebSocket)

Este proyecto es un servicio backend creado en .NET que se suscribe a un broker RabbitMQ para recibir mensajes y luego distribuirlos a clientes conectados mediante un WebSocket.

## Requisitos

- .NET SDK 6.0 o superior
- RabbitMQ en funcionamiento
- Un archivo `appsettings.json` configurado para RabbitMQ y WebSocket

## Estructura del Proyecto

- **Controllers**: Contiene los controladores que manejan las conexiones de WebSocket.
- **Mensajes**: Contiene las clases y lógica necesarias para el manejo de los mensajes recibidos de RabbitMQ.

## Configuración del Proyecto

1. Clona el repositorio:
    ```bash
    git clone <URL-del-repositorio>
    cd nombre-del-repositorio
    ```

2. Configura los detalles de conexión en el archivo `appsettings.json`:
    ```json
    {
      "RabbitMQ": {
        "Host": "localhost",
        "Port": 5672,
        "Username": "guest",
        "Password": "guest"
      },
      "WebSocket": {
        "Url": "ws://localhost:5000/ws"
      }
    }
    ```
    Asegúrate de actualizar los valores según la configuración de tu RabbitMQ y WebSocket.

## Comandos

### Restaurar dependencias

```bash
dotnet restore
```

Este comando restaurará todas las dependencias definidas en los archivos `.csproj`.

### Compilar la aplicación

```bash
dotnet build
```

Compila el proyecto y genera los archivos binarios necesarios.

### Ejecutar la aplicación en desarrollo

```bash
dotnet run --project <nombre-del-proyecto>
```

Este comando ejecuta la aplicación desde la carpeta correspondiente. Los mensajes de RabbitMQ se recibirán y distribuirán a través de WebSockets a los clientes conectados.

### Ejecutar pruebas

Si tienes un proyecto de pruebas, puedes ejecutar los tests con el siguiente comando:

```bash
dotnet test
```

### Publicar para producción

```bash
dotnet publish --configuration Release --output ./publish
```

Este comando genera los archivos necesarios para desplegar la aplicación en un servidor de producción.

## Interacción con RabbitMQ y WebSocket

- La aplicación se suscribe a un broker RabbitMQ para recibir mensajes.
- Estos mensajes se distribuyen mediante WebSocket a los clientes conectados en tiempo real.

## Despliegue

Para desplegar la aplicación en un entorno de producción:
1. Ejecuta el comando `dotnet publish` para generar los archivos optimizados.
2. Despliega los archivos en un servidor web compatible o usa Docker si es necesario.
3. Configura RabbitMQ y WebSocket para que estén accesibles desde tu servidor.

## Personalización

Puedes ajustar los tópicos de RabbitMQ o la configuración del WebSocket en el archivo `appsettings.json` para adaptarlo a tus necesidades específicas.
