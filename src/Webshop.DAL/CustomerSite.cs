namespace Webshop.DAL
{
    // custom entity for the business layer
    // immutable
    public class CustomerSite
    {
        public readonly string City;
        public readonly string Street;

        public CustomerSite(string city, string street)
        {
            City = city;
            Street = street;
        }
    }
}
