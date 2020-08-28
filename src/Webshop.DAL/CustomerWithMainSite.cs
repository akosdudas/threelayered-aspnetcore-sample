namespace Webshop.DAL
{
    // extends the base entity with further information
    public class CustomerWithMainSite : Customer
    {
        public readonly CustomerSite MainSite;

        public CustomerWithMainSite(string name, string email, CustomerSite mainSite)
            : base(name, email)
        {
            MainSite = mainSite;
        }

    }
}
