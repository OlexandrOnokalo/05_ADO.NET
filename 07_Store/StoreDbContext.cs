using _07_Store.Entytis;
using _07_Store.Helpers;
using Microsoft.EntityFrameworkCore;

namespace _07_Store
{

    internal class StoreDbContext : DbContext
    {

        public StoreDbContext()
        {

            //this.Database.EnsureDeleted();
            //this.Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(@"Data Source=(localDB)\MSSQLLocalDb;
                                        Initial Catalog=StoreAppDb;
                                        Integrated Security=True;
                                        Connect Timeout=5;
                                        Encrypt=False;TrustServerCertificate=True;
                                        Application Intent=ReadWrite;Multi Subnet Failover=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Country>(b =>
            {
                b.ToTable("Countries");
                b.HasKey(x => x.Id);
                b.Property(x => x.Name).IsRequired().HasMaxLength(150);
                b.HasMany(x => x.Cities)
                 .WithOne(c => c.Country)
                 .HasForeignKey(c => c.CountryId)
                 .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<City>(b =>
            {
                b.ToTable("Cities");
                b.HasKey(x => x.Id);
                b.Property(x => x.Name).IsRequired().HasMaxLength(150);
                b.HasMany(x => x.Shops)
                 .WithOne(s => s.City)
                 .HasForeignKey(s => s.CityId)
                 .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<Shop>(b =>
            {
                b.ToTable("Shops");
                b.HasKey(x => x.Id);
                b.Property(x => x.Name).IsRequired().HasMaxLength(200);
                b.Property(x => x.Address).IsRequired().HasMaxLength(300);
                b.Property(x => x.ParkingArea).IsRequired(false);
                b.HasMany(x => x.Workers)
                 .WithOne(w => w.Shop)
                 .HasForeignKey(w => w.ShopId)
                 .OnDelete(DeleteBehavior.Cascade);
                b.HasMany(x => x.ShopProducts)
                 .WithOne(sp => sp.Shop)
                 .HasForeignKey(sp => sp.ShopId)
                 .OnDelete(DeleteBehavior.Cascade);
            });


            modelBuilder.Entity<Position>(b =>
            {
                b.ToTable("Positions");
                b.HasKey(x => x.Id);
                b.Property(x => x.Name).IsRequired().HasMaxLength(150);
                b.HasMany(x => x.Workers)
                 .WithOne(w => w.Position)
                 .HasForeignKey(w => w.PositionId)
                 .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<Worker>(b =>
            {
                b.ToTable("Workers");
                b.HasKey(x => x.Id);
                b.Property(x => x.Name).IsRequired().HasMaxLength(120);
                b.Property(x => x.Surname).IsRequired().HasMaxLength(120);
                b.Property(x => x.Salary).HasColumnType("decimal(18,2)");
                b.Property(x => x.Email).IsRequired().HasMaxLength(200);
                b.Property(x => x.PhoneNumber).IsRequired().HasMaxLength(50);
            });


            modelBuilder.Entity<Category>(b =>
            {
                b.ToTable("Categories");
                b.HasKey(x => x.Id);
                b.Property(x => x.Name).IsRequired().HasMaxLength(150);
                b.HasMany(x => x.Products)
                 .WithOne(p => p.Category)
                 .HasForeignKey(p => p.CategoryId)
                 .IsRequired(false)
                 .OnDelete(DeleteBehavior.SetNull);
            });


            modelBuilder.Entity<Product>(b =>
            {
                b.ToTable("Products");
                b.HasKey(x => x.Id);
                b.Property(x => x.Name).IsRequired().HasMaxLength(250);
                b.Property(x => x.Price).HasColumnType("decimal(18,2)");
                b.Property(x => x.Discount).HasDefaultValue(0.0);
                b.Property(x => x.Quantity).HasDefaultValue(0);
                b.Property(x => x.IsInStock).HasDefaultValue(true);
                b.HasMany(x => x.ShopProducts)
                 .WithOne(sp => sp.Product)
                 .HasForeignKey(sp => sp.ProductId)
                 .OnDelete(DeleteBehavior.Cascade);
            });


            modelBuilder.Entity<ShopProduct>(b =>
            {
                b.ToTable("ShopProducts");
                b.HasKey(x => new { x.ShopId, x.ProductId });
                b.HasOne(x => x.Shop)
                 .WithMany(s => s.ShopProducts)
                 .HasForeignKey(x => x.ShopId);
                b.HasOne(x => x.Product)
                 .WithMany(p => p.ShopProducts)
                 .HasForeignKey(x => x.ProductId);
            });

            modelBuilder.SeedCountries();
            modelBuilder.SeedCities();
            modelBuilder.SeedCategories();
            modelBuilder.SeedProducts();
            modelBuilder.SeedShops();
            modelBuilder.SeedShopProducts();
            modelBuilder.SeedPositions();
            modelBuilder.SeedWorkers();
        }
        public DbSet<Country> Countries { get; set; } 
        public DbSet<City> Cities { get; set; }
        public DbSet<Shop> Shops { get; set; } 
        public DbSet<Position> Positions { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ShopProduct> ShopProducts { get; set; }
    }
}
