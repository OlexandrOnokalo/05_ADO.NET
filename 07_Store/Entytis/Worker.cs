namespace _07_Store.Entytis
{
    internal class Worker
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public decimal Salary { get; set; }
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public int PositionId { get; set; }
        public Position? Position { get; set; }
        public int ShopId { get; set; }
        public Shop? Shop { get; set; }
    }
}
