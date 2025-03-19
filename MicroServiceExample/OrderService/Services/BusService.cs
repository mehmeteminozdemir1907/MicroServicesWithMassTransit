using MassTransit;

namespace OrderService.Services
{
    public class BusService(IPublishEndpoint publishEndpoint)
    {
        public async Task PublishAsync<TEvent>(TEvent message)
        {
            CancellationTokenSource cancellationTokenSource = new();
            cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(30)); // 30 sn boyunca çalıştırmayı dener ve sonra exception fırlatır.

            await publishEndpoint.Publish(message!, pipe =>
            {
                pipe.Headers.Set("version", "1.0.0");
                pipe.SetAwaitAck(true); // SetAwaitAck kullanıldığında başarılı işlem yapıldığında dönüş olur. İşlem başarısızsa exception fırlatır.
                pipe.Durable = true;
                pipe.MessageId = Guid.NewGuid(); // MessageId requestin headerında gider.
            }, cancellationTokenSource.Token);
        }
    }
}
