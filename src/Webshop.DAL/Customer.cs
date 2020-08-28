namespace Webshop.DAL
{
    // representation of the Customer for the business logic
    // immutable
    public class Customer
    {
        public readonly string Name;
        public readonly string Email;

        public Customer(string name, string email)
        {
            Name = name;
            Email = email;
        }
    }
}
