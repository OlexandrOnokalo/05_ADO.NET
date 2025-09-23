namespace _07_Store.Entytis
{
    internal class ShopProduct
    {
        public int ShopId { get; set; }
        public Shop? Shop { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        
    }
}
