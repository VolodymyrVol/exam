using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exam
{
    internal class ApplicationContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<BookDiscount> BooksDiscounts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<BookCustomer> BooksCustomers { get; set; }

        public ApplicationContext()
        {
            Database.EnsureDeleted(); 
            Database.EnsureCreated(); 
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .Build();

            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookDiscount>().HasOne(p => p.Discount).WithMany().HasForeignKey(w => w.DiscountId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<BookDiscount>().HasOne(p => p.Book).WithMany().HasForeignKey(w => w.BookId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<BookCustomer>().HasOne(p => p.Customer).WithMany().HasForeignKey(w => w.CustomerId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<BookCustomer>().HasOne(p => p.Book).WithMany().HasForeignKey(w => w.BookId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Customer>().HasOne(p => p.Book).WithOne().HasForeignKey(w => w.LeftBookId)





        }
    }
}
