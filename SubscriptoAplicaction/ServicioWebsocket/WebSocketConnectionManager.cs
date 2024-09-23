using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

public class WebSocketConnectionManager
{
    private ConcurrentDictionary<string, (WebSocket Socket, string Topic)> _sockets = new ConcurrentDictionary<string, (WebSocket, string)>();

    // Agregar un nuevo WebSocket con su tópico
    public string AddSocket(WebSocket socket, string topic)
    {
        string connectionId = Guid.NewGuid().ToString();
        _sockets.TryAdd(connectionId, (socket, topic));
        return connectionId;
    }

    // Remover un WebSocket del diccionario
    public async Task RemoveSocketAsync(string connectionId)
    {
        if (_sockets.TryRemove(connectionId, out var socketInfo))
        {
            await socketInfo.Socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed", CancellationToken.None);
            socketInfo.Socket.Dispose();
        }
    }

    // Enviar un mensaje solo a los WebSockets suscritos a un tópico específico
    public async Task SendMessageToTopicAsync(string topic, string message)
    {
        var buffer = Encoding.UTF8.GetBytes(message);
        foreach (var socketInfo in _sockets.Values)
        {
            if (socketInfo.Socket.State == WebSocketState.Open && socketInfo.Topic == topic)
            {
                await socketInfo.Socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}
