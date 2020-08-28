using System.Collections.Generic;

namespace Webshop.DAL.EF
{
    public partial class Customer
    {
        public Customer()
        {
            CustomerSites = new HashSet<CustomerSite>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string BankAccount { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int? MainCustomerSiteId { get; set; }

        public CustomerSite MainCustomerSite { get; set; }
        public ICollection<CustomerSite> CustomerSites { get; set; }
    }
}
