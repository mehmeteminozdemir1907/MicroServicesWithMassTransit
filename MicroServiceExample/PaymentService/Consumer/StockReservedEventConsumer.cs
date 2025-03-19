using Common.BusEvents;
using Domain;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using PaymentService.Context;

namespace PaymentService.Consumer
{
    public class StockReservedEventConsumer(MainDbContext mainDbContext) : IConsumer<StockReservedEvent>
    {
        public async Task Consume(ConsumeContext<StockReservedEvent> context)
        {
            var version = context.Headers.Get<string>("version");
            var message = context.Message;
            var messageId = context.MessageId;

            using var transaction = await mainDbContext.Database.BeginTransactionAsync();

            try
            {
                var payment = new Payment
                {
                    Amount = 50,
                    TransactionDate = DateTime.Now,
                };

                await mainDbContext.Payments.AddAsync(payment);
                await mainDbContext.SaveChangesAsync();

                var order = await mainDbContext.Orders.FirstOrDefaultAsync(x => x.StockId == message.StockId);

                if (order != null)
                {
                    order.PaymentId = payment.Id;
                    mainDbContext.Orders.Update(order);
                }

                await mainDbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                await context.Publish(new PaymentCompletedEvent(message.StockId));
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                await context.Publish(new PaymentFailedEvent(message.StockId));
            }
        }
    }
}
