using Common.BusEvents;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using StockService.Context;

namespace StockService.Consumer
{
    public class OrderCreatedEventConsumer(MainDbContext mainDbContext) : IConsumer<OrderCreatedEvent>
    {
        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var version = context.Headers.Get<string>("version");
            var message = context.Message;
            var messageId = context.MessageId;

            var stock = await mainDbContext.Stocks.FirstOrDefaultAsync(x => x.Id == message.StockId);

            if (stock is not null && stock.Quantity >= message.Quantity)
            {
                stock.Quantity -= message.Quantity;
                mainDbContext.Stocks.Update(stock);
                await mainDbContext.SaveChangesAsync();

                await context.Publish(new StockReservedEvent(stock.Id));
            }
            else
            {
                await context.Publish(new StockNotAvailableEvent(stock.Id));
            }
        }
    }
}
