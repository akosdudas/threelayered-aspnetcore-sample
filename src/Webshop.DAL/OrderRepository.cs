using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Webshop.DAL
{
    // dummy class
    public class OrderRepository : IOrderRepository
    {
        public Task<IReadOnlyCollection<object>> ListCustomerOrders(int customerId)
            => Task.FromResult((IReadOnlyCollection<object>)Array.Empty<object>());
    }
}
