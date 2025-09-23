namespace _07_Store.Entytis
{
    internal class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public double Discount { get; set; }
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        public int Quantity { get; set; }
        public bool IsInStock { get; set; }
        public ICollection<ShopProduct> ShopProducts { get; set; } = new List<ShopProduct>();
    }
}
