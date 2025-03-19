using Common.BusEvents;
using Common.Enums;
using Domain;
using Microsoft.EntityFrameworkCore;
using OrderService.Context;

namespace OrderService.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly MainDbContext mainDbContext;
        private readonly BusService busService;

        public OrdersService(MainDbContext mainDbContext, BusService busService)
        {
            this.mainDbContext = mainDbContext;
            this.busService = busService;
        }

        public async Task CreateAsync()
        {
            var randomStock = await mainDbContext.Stocks
                .OrderBy(x => Guid.NewGuid())
                .FirstOrDefaultAsync();
            
            var order = new Order
            {
                StockId = randomStock.Id,
                Name = $"{randomStock.Name} Order",
                Quantity = 10,
                Status = OrderStatusType.Suspend,
            };

            await this.mainDbContext.Orders.AddAsync(order).ConfigureAwait(false);
            await this.mainDbContext.SaveChangesAsync().ConfigureAwait(false);

            var orderEvent = new OrderCreatedEvent(order.StockId, order.Name, order.Quantity);

            await this.busService.PublishAsync(orderEvent).ConfigureAwait(false);
        }
    }
}
