using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _05_Entity_Framework.Entytis
{
    [Table("Playlists")]
    class Playlist
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; }

        public int? CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<PlaylistTrack> PlaylistTracks { get; set; }
    }
}
