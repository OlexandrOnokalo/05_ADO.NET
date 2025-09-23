using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _05_Entity_Framework.Entytis
{
    [Table("Countries")]
    class Country
    {
        
        public int Id { get; set; }
                
        public string Name { get; set; }

        public ICollection<Artist> Artists { get; set; }
    }
}
