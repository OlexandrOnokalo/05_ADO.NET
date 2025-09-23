using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _05_Entity_Framework.Entytis
{
    [Table("Artists")]
    class Artist
    {
        [Key]
        public int Id { get; set; }

        
        public string FirstName { get; set; }

        
        public string LastName { get; set; }

        
        public int CountryId { get; set; }
        public Country Country { get; set; }

        public ICollection<Album> Albums { get; set; }
    }
}
