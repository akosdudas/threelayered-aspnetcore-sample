using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Webshop.DAL.EF;

namespace Webshop.DAL
{
    // repository pattern for the persistence of Customer entitites
    public class CustomerRepository : ICustomerRepository
    {
        private readonly WebshopDb db;

        public CustomerRepository(WebshopDb db)
            => this.db = db;

        public async Task<Customer> GetCustomerOrNull(int customerId)
        {
            var dbCustomer = await db.Customers
                                     .GetByIdOrNull(customerId);
            return dbCustomer?.GetCustomer();
            // ?. null propagating operator: if there is no such record GetById returns null and further calls would throw
            //    when using ?. if the left side is null the result is null without calling the method on the right side
        }

        // parameter default value: no need to specify when not used
        public async Task<IReadOnlyCollection<Customer>> ListCustomers(string customerName = null)
        {
            return await db.Customers
                           .SearchByName(customerName)
                           .GetCustomers();
        }

        public async Task<IReadOnlyCollection<CustomerWithMainSite>> ListCustomersWithSites(string customerName = null)
        {
            // "Fluent API" stle query with the technical details implemented in the extension methods
            return await db.Customers
                           .WithSites()
                           .SearchByName(customerName)
                           .GetCustomersWithMainSite();
        }

        public async Task DeleteCustomer(int customerId)
        {
            int retries = 3;
            while (true) // retry due to concurrency issues, but not indefinitely
            {
                var dbRecord = await db.Customers
                                       .WithSites()
                                       .GetByIdOrNull(customerId);
                if (dbRecord == null) // deleted already (by concurrent operation)
                    return;

                db.Customers.Remove(dbRecord);

                try
                {
                    await db.SaveChangesAsync();
                    return; // successful delete, stop the retry
                }
                catch (DbUpdateConcurrencyException ex) // catch only optimistic concurrency errors, not all errors!
                {
                    if (--retries < 0) // no endless loops
                        throw;

                    // update from the database and retry again
                    foreach (var e in ex.Entries)
                        await e.ReloadAsync();
                }
            }
        }
    }

    // helper extension methods
    internal static class CustomerRepositoryExtensions
    {
        public static async Task<IReadOnlyCollection<Customer>> GetCustomers(this IQueryable<EF.Customer> customers)
        {
            return await customers.Select(dbRec => dbRec.GetCustomer())
                                  .ToArrayAsync();
            // ToArrayAsync() evaluates the query now
            // otherwise it would be possible to return with an IEnumerable that would be evaluated later in the BLL without any DbContext available (and would throw an error)
        }

        public static Customer GetCustomer(this EF.Customer dbRecord) => new Customer(dbRecord.Name, dbRecord.Email);

        public static IQueryable<EF.Customer> SearchByName(this IQueryable<EF.Customer> customers, string nameToSearchFor)
        {
            // no search criteri yields the entire "table"
            if (string.IsNullOrEmpty(nameToSearchFor))
                return customers;
            else // otherwise add filtering for the name
                return customers.Where(v => v.Name.Contains(nameToSearchFor));
        }


        public static IQueryable<EF.Customer> WithSites(this IQueryable<EF.Customer> customers) => customers.Include(v => v.CustomerSites);

        public static async Task<IReadOnlyCollection<CustomerWithMainSite>> GetCustomersWithMainSite(this IQueryable<EF.Customer> customers)
        {
            return await customers.Select(dbRec => GetCustomerWithMainSite(dbRec))
                                  .ToArrayAsync();
        }

        public static CustomerSite GetCustomerSite(EF.CustomerSite dbRec) => new CustomerSite(dbRec.City, dbRec.Street);

        public static CustomerWithMainSite GetCustomerWithMainSite(EF.Customer dbRec)
            => new CustomerWithMainSite(dbRec.Name, dbRec.Email, GetCustomerSite(dbRec.MainCustomerSite));

        public static Task<EF.Customer> GetByIdOrNull(this IQueryable<EF.Customer> customers, int customerId) => customers.FirstOrDefaultAsync(v => v.Id == customerId);
    }
}
