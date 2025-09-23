using _07_Store.Entytis;
using Microsoft.EntityFrameworkCore;

namespace _07_Store.Helpers
{
    internal static class DbInitializer
    {
        public static void SeedCountries(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>().HasData(
                new Country { Id = 1, Name = "Ukraine" },
                new Country { Id = 2, Name = "Poland" }
            );
        }

        public static void SeedCities(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>().HasData(
                new City { Id = 1, Name = "Kyiv", CountryId = 1 },
                new City { Id = 2, Name = "Lviv", CountryId = 1 },
                new City { Id = 3, Name = "Warsaw", CountryId = 2 }
            );
        }

        public static void SeedCategories(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electronics" },
                new Category { Id = 2, Name = "Groceries" },
                new Category { Id = 3, Name = "Clothes" }
            );
        }

        public static void SeedProducts(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Smartphone X", Price = 799.99m, Discount = 0.1, CategoryId = 1, Quantity = 50, IsInStock = true },
                new Product { Id = 2, Name = "Milk 1L", Price = 1.49m, Discount = 0.0, CategoryId = 2, Quantity = 200, IsInStock = true },
                new Product { Id = 3, Name = "T-Shirt", Price = 19.99m, Discount = 0.0, CategoryId = 3, Quantity = 120, IsInStock = true }
            );
        }

        public static void SeedShops(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Shop>().HasData(
                new Shop { Id = 1, Name = "Central Shop", Address = "Khreshchatyk 1", CityId = 1, ParkingArea = 20 },
                new Shop { Id = 2, Name = "West Mall", Address = "Shevchenka 10", CityId = 2, ParkingArea = 50 }
            );
        }

        public static void SeedShopProducts(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShopProduct>().HasData(
                new ShopProduct { ShopId = 1, ProductId = 1,  },
                new ShopProduct { ShopId = 1, ProductId = 2, },
                new ShopProduct { ShopId = 2, ProductId = 3, },
                new ShopProduct { ShopId = 2, ProductId = 2, }
            );
        }

        public static void SeedPositions(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Position>().HasData(
                new Position { Id = 1, Name = "Manager" },
                new Position { Id = 2, Name = "Cashier" },
                new Position { Id = 3, Name = "Seller" }
            );
        }

        public static void SeedWorkers(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Worker>().HasData(
                new Worker { Id = 1, Name = "Ivan", Surname = "Petrov", Salary = 1200.00m, Email = "ivan.petrov@example.com", PhoneNumber = "+380501112233", PositionId = 1, ShopId = 1 },
                new Worker { Id = 2, Name = "Olena", Surname = "Shevchenko", Salary = 700.00m, Email = "olena.shev@example.com", PhoneNumber = "+380671112244", PositionId = 2, ShopId = 1 },
                new Worker { Id = 3, Name = "Piotr", Surname = "Nowak", Salary = 900.00m, Email = "piotr.nowak@example.com", PhoneNumber = "+48 501 11 22 33", PositionId = 3, ShopId = 2 }
            );
        }
    }
}
