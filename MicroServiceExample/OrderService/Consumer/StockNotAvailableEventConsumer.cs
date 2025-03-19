using Common.BusEvents;
using Common.Enums;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderService.Context;

namespace OrderService.Consumer
{
    public class StockNotAvailableEventConsumer(MainDbContext mainDbContext) : IConsumer<StockNotAvailableEvent>
    {
        public async Task Consume(ConsumeContext<StockNotAvailableEvent> context)
        {
            var version = context.Headers.Get<string>("version");
            var message = context.Message;
            var messageId = context.MessageId;

            var order = await mainDbContext.Orders.FirstOrDefaultAsync(x => x.StockId == message.StockId);

            if (order != null)
            {
                order.Status = OrderStatusType.LowStock;
                mainDbContext.Orders.Update(order);
                await mainDbContext.SaveChangesAsync();
            }
        }
    }
}