using System.Collections.Generic;
using System.Threading.Tasks;

namespace Webshop.DAL
{
    public interface ICustomerRepository
    {
        Task<IReadOnlyCollection<Customer>> ListCustomers(string customerName = null);
        Task<Customer> GetCustomerOrNull(int customerId);
        Task<IReadOnlyCollection<CustomerWithMainSite>> ListCustomersWithSites(string customerName = null);
        Task DeleteCustomer(int customerId);
    }
}
