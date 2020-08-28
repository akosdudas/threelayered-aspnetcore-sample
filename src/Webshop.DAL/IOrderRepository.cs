using System.Collections.Generic;
using System.Threading.Tasks;

namespace Webshop.DAL
{
    public interface IOrderRepository
    {
        Task<IReadOnlyCollection<object>> ListCustomerOrders(int customerId);
    }
}
