using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _05_Entity_Framework.Entytis
{
    [Table("Albums")]
    class Album
    {
        
        public int Id { get; set; }

        
        public string Title { get; set; }

        public int ArtistId { get; set; }
        public Artist Artist { get; set; }

        public int Year { get; set; }

        public int GenreId { get; set; }
        public Genre Genre { get; set; }
        public int Rating { get; set; }
        public ICollection<Track> Tracks { get; set; }
    }
}
