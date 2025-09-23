namespace _07_Store.Entytis
{
    internal class Position
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<Worker> Workers { get; set; } = new List<Worker>();
    }
}
