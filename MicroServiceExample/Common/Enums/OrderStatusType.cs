using System.ComponentModel;

namespace Common.Enums
{
    public enum OrderStatusType
    {
        [Description("Askıda")]
        Suspend = 1,

        [Description("Tamamlandı")]
        Completed = 2,

        [Description("Stok Yetersiz")]
        LowStock = 3,

        [Description("Ödeme Hatalı")]
        PaymentError = 4,
    }
}