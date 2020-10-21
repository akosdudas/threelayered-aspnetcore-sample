using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Webshop.DAL;

namespace Webshop.BL
{
    // responsible for business workflows
    public class CustomerManager
    {
        // interface instead of implementation to allow testing
        private readonly ICustomerRepository customerRepository;
        private readonly IOrderRepository orderRepository;

        public CustomerManager(ICustomerRepository customerRepository, IOrderRepository orderRepository)
        {
            this.customerRepository = customerRepository;
            this.orderRepository = orderRepository;
        }

        // return value is not a list or array but a read-only collection
        // this shows that the values can be accessed, but not modified (the instances themselves are immutable too)
        public async Task<IReadOnlyCollection<Customer>> ListCustomers(string nameToSearchFor = null)
            => await customerRepository.ListCustomers(nameToSearchFor);

        public async Task<Customer> GetCustomerOrNull(int customerId)
            => await customerRepository.GetCustomerOrNull(customerId);

        public async Task<bool> TryDeleteCustomer(int customerId)
        {
            // transaction to prohibit adding orders while deleting
            // translation is not maintained by the data access layer as this complex process uses two repositories
            // (business workflow -> transactional boundary)
            using (var tran = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions() { IsolationLevel = IsolationLevel.RepeatableRead },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                // does the customer exist at all?
                // additionally: due to repetable read locks the record
                var customer = await customerRepository.GetCustomerOrNull(customerId);
                if (customer == null)
                    return false;

                // does the user have orders?
                // additionally: due to repetable read locks the records
                bool hasOrders = (await orderRepository.ListCustomerOrders(customerId)).Any();
                if (hasOrders)
                    return false; // cannot delete

                // ok, can procees
                await customerRepository.DeleteCustomer(customerId);

                // transaction must be finished explicitly
                tran.Complete();
                return true;
            }
        }
    }
}
