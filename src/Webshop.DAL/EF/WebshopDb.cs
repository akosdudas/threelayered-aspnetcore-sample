using Microsoft.EntityFrameworkCore;

namespace Webshop.DAL.EF
{
    public partial class WebshopDb : DbContext
    {
        public WebshopDb()
        {
        }

        public WebshopDb(DbContextOptions<WebshopDb> options)
            : base(options)
        {
        }

        public virtual DbSet<CustomerSite> CustomerSites { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerSite>(entity =>
            {
                entity.ToTable("CustomerSite");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Fax)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.ZipCode)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Tel)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Street).HasMaxLength(50);

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CustomerSites)
                    .HasForeignKey(d => d.CustomerId);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.Login).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.BankAccount)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MainCustomerSiteId).HasColumnName("MainCustomerSiteID");

                entity.HasOne(d => d.MainCustomerSite)
                    .WithOne()
                    .HasForeignKey<Customer>(d => d.MainCustomerSiteId);
            });
        }
    }
}
