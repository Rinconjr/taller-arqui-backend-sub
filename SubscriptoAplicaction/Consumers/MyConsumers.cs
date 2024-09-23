using MassTransit;
using CommunWork.Mensajes; // Asegúrate de importar el namespace correcto del mensaje
using System.Threading.Tasks;

namespace SubscriptoAplicacion.Consumers
{
    public class MyConsumer : IConsumer<MyMessage>
    {
        private readonly WebSocketConnectionManager _connectionManager;

        public MyConsumer(WebSocketConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        public async Task Consume(ConsumeContext<MyMessage> context)
        {
            // Lógica para procesar el mensaje recibido
            string topic = context.Message.Topic;
            string messageText = context.Message.Text;

            // Imprimir en consola
            Console.WriteLine($"Mensaje recibido: {messageText}");
            Console.WriteLine($"Topico: {topic}");

            // Lógica para enviar solo a los WebSockets suscritos al tópico correspondiente
            await _connectionManager.SendMessageToTopicAsync(topic, $"Mensaje recibido: {messageText}");
        }
    }
}
