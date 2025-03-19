using Common.Enums;

namespace Common.BusEvents
{
    public record OrderCreatedEvent(int StockId, string Name, int Quantity);
}