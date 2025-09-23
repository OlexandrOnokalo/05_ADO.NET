using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _05_Entity_Framework.Entytis
{
    [Table("Tracks")]
    class Track
    {
        
        public int Id { get; set; }

        
        public string Title { get; set; }

        public int AlbumId { get; set; }
        public Album Album { get; set; }


        public int DurationSeconds { get; set; }

        public int Rating { get; set; }

        public int Listens { get; set; }
        
        public string? Lyrics { get; set; }

        public ICollection<PlaylistTrack> PlaylistTracks { get; set; }
    }
}
