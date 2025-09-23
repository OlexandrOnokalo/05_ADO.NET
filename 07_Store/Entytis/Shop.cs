namespace _07_Store.Entytis
{
    internal class Shop
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public int CityId { get; set; }
        public City? City { get; set; }
        public int? ParkingArea { get; set; }
        public ICollection<Worker> Workers { get; set; } = new List<Worker>();
        public ICollection<ShopProduct> ShopProducts { get; set; } = new List<ShopProduct>();
    }
}
