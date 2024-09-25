using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SubscriptoAplicacion.Consumers;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configurar Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar MassTransit con RabbitMQ para SubscriptoAplicacion
builder.Services.AddMassTransit(x =>
{
    // Registrar el consumidor
    x.AddConsumer<MyConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("admin");
            h.Password("admin123");
        });

        // Configurar la cola y asociarla al consumidor
        cfg.ReceiveEndpoint("my_queue", e =>
        {
            e.ConfigureConsumer<MyConsumer>(context);
        });
    });
});

// A�adir el servicio de MassTransit Hosted
builder.Services.AddMassTransitHostedService();

// Configurar WebSockets
builder.Services.AddWebSockets(options =>
{
    options.KeepAliveInterval = TimeSpan.FromSeconds(120);
});

// Registrar el WebSocketConnectionManager
builder.Services.AddSingleton<WebSocketConnectionManager>();

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Configurar el middleware de WebSocket
app.UseWebSockets();

// Usar CORS
app.UseCors("AllowAll");

app.MapControllers();



app.Use(async (context, next) =>
{
    if (context.Request.Path == "/ws")
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            var connectionManager = app.Services.GetRequiredService<WebSocketConnectionManager>();

            // Obtener el t�pico de la solicitud
            var topic = context.Request.Query["topic"]; // Ejemplo: wss://localhost/ws?topic=topico1
            var connectionId = connectionManager.AddSocket(webSocket, topic);

            await HandleWebSocketConnection(webSocket, connectionManager, connectionId);
        }
        else
        {
            context.Response.StatusCode = 400;
        }
    }
    else
    {
        await next();
    }
});

app.Run();

// M�todo para manejar la comunicaci�n WebSocket
async Task HandleWebSocketConnection(WebSocket webSocket, WebSocketConnectionManager manager, string connectionId)
{
    var buffer = new byte[1024 * 4];
    WebSocketReceiveResult result;

    try
    {
        while ((result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None)).CloseStatus == null)
        {
            // Aqu� puedes manejar los mensajes entrantes si es necesario
            await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);
        }
    }
    catch (Exception ex)
    {
        // Manejar excepciones si es necesario
    }
    finally
    {
        await manager.RemoveSocketAsync(connectionId);
    }
}
