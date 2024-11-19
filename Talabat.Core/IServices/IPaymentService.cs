using Talabat.Core.Entities;

namespace Talabat.Core.IServices
{
    public interface IPaymentService
    {
        Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string basketId);
    }
}
