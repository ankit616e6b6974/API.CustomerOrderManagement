using API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Web.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Inventory> Inventories => Set<Inventory>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Customer
            modelBuilder.Entity<Customer>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Name).IsRequired().HasMaxLength(100);
                e.Property(x => x.Email).IsRequired().HasMaxLength(200);
                e.HasIndex(x => x.Email).IsUnique();
            });

            // Product
            modelBuilder.Entity<Product>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Price).HasColumnType("decimal(18,2)");
            });

            // Inventory
            modelBuilder.Entity<Inventory>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasOne(x => x.Product)
                 .WithOne(x => x.Inventory)
                 .HasForeignKey<Inventory>(x => x.ProductId);
            });

            // Order
            modelBuilder.Entity<Order>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.TotalAmount).HasColumnType("decimal(18,2)");
                e.Property(x => x.Status).HasConversion<string>();
                e.HasOne(x => x.Customer)
                 .WithMany(x => x.Orders)
                 .HasForeignKey(x => x.CustomerId);
            });

            // OrderItem
            modelBuilder.Entity<OrderItem>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.UnitPrice).HasColumnType("decimal(18,2)");
                e.HasOne(x => x.Order)
                 .WithMany(x => x.OrderItems)
                 .HasForeignKey(x => x.OrderId);
                e.HasOne(x => x.Product)
                 .WithMany(x => x.OrderItems)
                 .HasForeignKey(x => x.ProductId);
            });

            // Seed Data
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Laptop Pro 15", Description = "High-performance laptop", Price = 1299.99m },
                new Product { Id = 2, Name = "Wireless Mouse", Description = "Ergonomic wireless mouse", Price = 49.99m },
                new Product { Id = 3, Name = "Mechanical Keyboard", Description = "RGB mechanical keyboard", Price = 129.99m },
                new Product { Id = 4, Name = "4K Monitor", Description = "27-inch 4K display", Price = 699.99m },
                new Product { Id = 5, Name = "USB-C Hub", Description = "7-in-1 USB-C hub", Price = 79.99m },
                new Product { Id = 6, Name = "Webcam HD", Description = "1080p webcam", Price = 89.99m },
                new Product { Id = 7, Name = "Noise Cancelling Headphones", Description = "ANC headphones", Price = 349.99m },
                new Product { Id = 8, Name = "SSD 1TB", Description = "NVMe SSD", Price = 119.99m }
            );

            modelBuilder.Entity<Inventory>().HasData(
                new Inventory { Id = 1, ProductId = 1, QuantityAvailable = 20, UpdatedAtUtc = new DateTime(2026, 02, 28, 10, 10, 10) },
                new Inventory { Id = 2, ProductId = 2, QuantityAvailable = 100, UpdatedAtUtc = new DateTime(2026, 02, 28, 10, 10, 10) },
                new Inventory { Id = 3, ProductId = 3, QuantityAvailable = 50, UpdatedAtUtc = new DateTime(2026, 02, 28, 10, 10, 10) },
                new Inventory { Id = 4, ProductId = 4, QuantityAvailable = 15, UpdatedAtUtc = new DateTime(2026, 02, 28, 10, 10, 10) },
                new Inventory { Id = 5, ProductId = 5, QuantityAvailable = 75, UpdatedAtUtc = new DateTime(2026, 02, 28, 10, 10, 10) },
                new Inventory { Id = 6, ProductId = 6, QuantityAvailable = 40, UpdatedAtUtc = new DateTime(2026, 02, 28, 10, 10, 10) },
                new Inventory { Id = 7, ProductId = 7, QuantityAvailable = 30, UpdatedAtUtc = new DateTime(2026, 02, 28, 10, 10, 10) },
                new Inventory { Id = 8, ProductId = 8, QuantityAvailable = 60, UpdatedAtUtc = new DateTime(2026, 02, 28, 10, 10, 10) }
            );

            modelBuilder.Entity<Customer>().HasData(
                new Customer { Id = 1, Name = "Alice Johnson", Email = "alice@example.com", Phone = "555-0101", CreatedAtUtc = new DateTime(2026, 04, 01, 10, 10, 10) },
                new Customer { Id = 2, Name = "Bob Smith", Email = "bob@example.com", Phone = "555-0102", CreatedAtUtc = new DateTime(2026, 03, 01, 10, 10, 10) },
                new Customer { Id = 3, Name = "Carol White", Email = "carol@example.com", Phone = "555-0103", CreatedAtUtc = new DateTime(2026, 04, 01, 10, 10, 10) },
                new Customer { Id = 4, Name = "David Brown", Email = "david@example.com", Phone = "555-0104", CreatedAtUtc = new DateTime(2026, 04, 01, 10, 10, 10) },
                new Customer { Id = 5, Name = "Eva Martinez", Email = "eva@example.com", Phone = "555-0105", CreatedAtUtc = new DateTime(2026, 04, 01, 10, 10, 10) },
                new Customer { Id = 6, Name = "Kaali", Email = "kaali@example.com", Phone = "555-0104", CreatedAtUtc = new DateTime(2026, 04, 01, 10, 10, 10) },
                new Customer { Id = 7, Name = "Luffy", Email = "luffy@example.com", Phone = "555-0105", CreatedAtUtc = new DateTime(2026, 04, 01, 10, 10, 10) }
            );

            modelBuilder.Entity<Order>().HasData(
                new Order { Id = 1, CustomerId = 1, TotalAmount = 1349.98m, Status = 1, CreatedAtUtc = new DateTime(2026, 04, 10) },
                new Order { Id = 2, CustomerId = 2, TotalAmount = 179.98m, Status = 1, CreatedAtUtc = new DateTime(2026, 04, 11) },
                new Order { Id = 3, CustomerId = 3, TotalAmount = 699.99m, Status = 1, CreatedAtUtc = new DateTime(2026, 04, 12) },
                new Order { Id = 4, CustomerId = 1, TotalAmount = 144.98m, Status = 1, CreatedAtUtc = new DateTime(2026, 04, 13) },
                new Order { Id = 5, CustomerId = 5, TotalAmount = 349.99m, Status = 0, CreatedAtUtc = new DateTime(2026, 04, 14) },
                new Order { Id = 6, CustomerId = 6, TotalAmount = 1349.98m, Status = 1, CreatedAtUtc = new DateTime(2026, 04, 10) },
                new Order { Id = 7, CustomerId = 7, TotalAmount = 179.98m, Status = 1, CreatedAtUtc = new DateTime(2026, 04, 11) },
                new Order { Id = 8, CustomerId = 6, TotalAmount = 144.98m, Status = 1, CreatedAtUtc = new DateTime(2026, 04, 13) }
            );

            modelBuilder.Entity<OrderItem>().HasData(
                // Order 1: Laptop + Mouse
                new OrderItem { Id = 1, OrderId = 1, ProductId = 1, Quantity = 1, UnitPrice = 1299.99m },
                new OrderItem { Id = 2, OrderId = 1, ProductId = 2, Quantity = 1, UnitPrice = 49.99m },

                // Order 2: Mouse + Keyboard
                new OrderItem { Id = 3, OrderId = 2, ProductId = 2, Quantity = 1, UnitPrice = 49.99m },
                new OrderItem { Id = 4, OrderId = 2, ProductId = 3, Quantity = 1, UnitPrice = 129.99m },

                // Order 3: 4K Monitor
                new OrderItem { Id = 5, OrderId = 3, ProductId = 4, Quantity = 1, UnitPrice = 699.99m },

                // Order 4: USB-C Hub + Webcam
                new OrderItem { Id = 6, OrderId = 4, ProductId = 5, Quantity = 1, UnitPrice = 79.99m },
                new OrderItem { Id = 7, OrderId = 4, ProductId = 6, Quantity = 1, UnitPrice = 64.99m },

                // Order 5: Noise Cancelling Headphones
                new OrderItem { Id = 8, OrderId = 5, ProductId = 7, Quantity = 1, UnitPrice = 349.99m },

                // Order 6: Laptop + Mouse
                new OrderItem { Id = 9, OrderId = 6, ProductId = 1, Quantity = 1, UnitPrice = 1299.99m },
                new OrderItem { Id = 10, OrderId = 6, ProductId = 2, Quantity = 1, UnitPrice = 49.99m },

                // Order 7: USB-C Hub + Webcam
                new OrderItem { Id = 11, OrderId = 8, ProductId = 5, Quantity = 1, UnitPrice = 79.99m },
                new OrderItem { Id = 12, OrderId = 8, ProductId = 6, Quantity = 1, UnitPrice = 64.99m },

                // Order 8: Mouse + Keyboard
                new OrderItem { Id = 13, OrderId = 7, ProductId = 2, Quantity = 1, UnitPrice = 49.99m },
                new OrderItem { Id = 14, OrderId = 7, ProductId = 3, Quantity = 1, UnitPrice = 129.99m }
            );
        }
    }
}
