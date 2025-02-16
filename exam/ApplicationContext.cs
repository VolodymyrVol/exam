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
                            .AddJsonFile("C:\\Users\\vovan\\source\\repos\\exam\\exam\\appsettings.json")
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .Build();

            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookDiscount>().HasOne(p => p.Discount).WithMany().HasForeignKey(w => w.DiscountId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<BookDiscount>().HasOne(p => p.Book).WithMany().HasForeignKey(w => w.BookId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<BookCustomer>().HasOne(p => p.Customer).WithMany().HasForeignKey(w => w.CustomerId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<BookCustomer>().HasOne(p => p.Book).WithMany().HasForeignKey(w => w.BookId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Book>().HasOne(p => p.Customer).WithOne(p => p.Book).HasForeignKey<Customer>(p => p.LeftBookId).IsRequired(false).OnDelete(DeleteBehavior.NoAction);
            base.OnModelCreating(modelBuilder);
        }
    }
}
