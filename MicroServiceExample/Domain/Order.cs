using Common.Enums;

namespace Domain
{
    public class Order
    {
        public int Id { get; set; }

        public int StockId { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public OrderStatusType Status { get; set; }

        public int? PaymentId { get; set; }

        public virtual Payment? Payment { get; set; }
    }
}