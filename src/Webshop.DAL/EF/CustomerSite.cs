namespace Webshop.DAL.EF
{
    public partial class CustomerSite
    {
        public int Id { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public int? CustomerId { get; set; }

        public Customer Customer { get; set; }
    }
}
