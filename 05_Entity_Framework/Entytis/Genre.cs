using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _05_Entity_Framework.Entytis
{
    [Table("Genres")]
    class Genre
    {
        
        public int Id { get; set; }

        
        public string Name { get; set; }

        public ICollection<Album> Albums { get; set; }
    }
}
