
# .NET Suscriptor (RabbitMQ a WebSocket)

Este proyecto es un servicio backend creado en .NET que se suscribe a un broker RabbitMQ para recibir mensajes y luego distribuirlos a clientes conectados mediante un WebSocket.

## Requisitos

- .NET SDK 6.0 o superior
- RabbitMQ en funcionamiento
- Un archivo `appsettings.json` configurado para RabbitMQ y WebSocket

## Comandos

### Ejecutar la aplicación con Docker Compose

Para ejecutar la aplicación, utiliza el siguiente comando:

```bash
docker-compose up
```

Este comando levantará los servicios necesarios definidos en el archivo `docker-compose.yml` para ejecutar la aplicación.

## Interacción con RabbitMQ y WebSocket

- La aplicación se suscribe a un broker RabbitMQ para recibir mensajes.
- Estos mensajes se distribuyen mediante WebSocket a los clientes conectados en tiempo real.
