using System.Runtime.Serialization;

namespace Talabat.Core.Entities.OrderAggregate
{
    public enum OrderStatus
    {
        [EnumMember(Value = "Pending")]
        Pending,

        [EnumMember(Value = "Payment Succeeded")]
        PaymentSucceeded,

        [EnumMember(Value = "Payment Failed")]
        PaymentFailed
    }
}
