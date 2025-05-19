using Ecommerce.Models;
using System.Net;
using Microsoft.EntityFrameworkCore;
namespace Ecommerce.AppDbContext
{
    public class AppdbContext : DbContext
    {
        public AppdbContext(DbContextOptions<AppdbContext> options) : base(options) { }
        public DbSet<User> users { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Cart> carts { get; set; }
        public DbSet<CartItems> cartItems { get; set; }
        public DbSet<WishList> wishList { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }
        public DbSet<Address> Addresses { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(x => x.Role)
                .HasDefaultValue("user");
            modelBuilder.Entity<User>()
                .Property(x => x.IsBlocked)
                .HasDefaultValue("false");
            modelBuilder.Entity<Category>()
                .HasMany(x => x.Products)
                .WithOne(r => r.Category)
                .HasForeignKey(x => x.CategoryId);
            modelBuilder.Entity<Product>()
                .Property(pr => pr.Price).
                HasPrecision(18, 2);
            modelBuilder.Entity<Product>().HasData(
      new Product
      {
          Id = 1,
          Title = "BMW 7 SERIES",
          Description = "The BMW 7 Series is a full-size luxury sedan, known for its blend of comfort, performance, and advanced technology, serving as BMW's flagship model",
          Price = 100,
          Image = "https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.amazon.in%2FHot-Wheels-Nissan-Skyline-Imports%2Fdp%2FB0DDHLX5B3&psig=AOvVaw3EoMvC6pCTZDa6eXw2gY_8&ust=1734499339117000&source=images&cd=vfe&opi=89978449&ved=0CBQQjRxqFwoTCKC409uHrooDFQAAAAAdAAAAABAE",
          Stock = 20,
          CategoryId = 1
      },
      new Product
      {
          Id = 2,
          Title = "BENZ S-CLASS",
          Description = "The Mercedes-Benz S-Class is the flagship, full-size luxury sedan and coupe series",
          Price = 200,
          Image = "https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.amazon.in%2FHot-Wheels-Nissan-Skyline-Imports%2Fdp%2FB0DDHLX5B3&psig=AOvVaw3EoMvC6pCTZDa6eXw2gY_8&ust=1734499339117000&source=images&cd=vfe&opi=89978449&ved=0CBQQjRxqFwoTCKC409uHrooDFQAAAAAdAAAAABAE",
          Stock = 20,
          CategoryId = 2
      }

      );
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Bmw" },
                new Category { Id = 2, Name = "Benz" }
                );

            modelBuilder.Entity<User>()
                .HasOne(x => x.Cart)
                .WithOne(y => y.User)
                .HasForeignKey<Cart>(x => x.UserId);
            modelBuilder.Entity<Cart>()
                .HasMany(q => q.CartItems)
                .WithOne(w => w.Cart)
                .HasForeignKey(i => i.CartId);
            modelBuilder.Entity<CartItems>()
                .HasOne(f => f.Product)
                .WithMany(o => o.CartItems)
                .HasForeignKey(i => i.Id);
            modelBuilder.Entity<WishList>()
                .HasOne(x => x.users)
                .WithMany(w => w.WishList)
                .HasForeignKey(e => e.UserId);
            modelBuilder.Entity<WishList>()
                .HasOne(x => x.products)
                .WithMany()
                .HasForeignKey(e => e.ProductId);
            modelBuilder.Entity<Order>()
                .HasOne(x => x.User)
                .WithMany(O => O.Orders)
                .HasForeignKey(e => e.UserId);
            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);
            modelBuilder.Entity<Order>()
            .Property(x => x.Status)
            .HasDefaultValue("placed");
            modelBuilder.Entity<OrderItems>()
                .HasOne(p => p.Order)
                .WithMany(c => c.OrderItems)
                .HasForeignKey(d => d.OrderId);
            modelBuilder.Entity<OrderItems>()
                .HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(p => p.ProductId);
            modelBuilder.Entity<OrderItems>()
                .Property(oi => oi.TotalPrice)
                .HasPrecision(18, 2);
            modelBuilder.Entity<Address>()
                .HasOne(a => a.User)
                .WithMany(u => u.Addresses)
                .HasForeignKey(u => u.UserId);
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Address)
                .WithMany(a => a.Orders)
                .HasForeignKey(u => u.AddressId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
