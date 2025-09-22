using _05_Entity_Framework.Entytis;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace _05_Entity_Framework
{

    
    internal class Program
    {
        static void Main(string[] args)
        {
            using var context = new MusicDbContext();





            var rnd = new Random();

            
            var tracks1 = context.Tracks.ToList();
            foreach (var track in tracks1)
            {
                track.Listens = rnd.Next(100, 10000);
                track.Rating = rnd.Next(1, 6);       
            }

            
            var albums = context.Albums.ToList();
            foreach (var album in albums)
            {
                album.Rating = rnd.Next(1, 6);        
            }

            context.SaveChanges();







            int albumId = 1; 
            var avgListens = context.Tracks
                .Where(t => t.AlbumId == albumId)
                .Average(t => t.Listens);

            var tracksAboveAvg = context.Tracks
                .Where(t => t.AlbumId == albumId && t.Listens > avgListens)
                .ToList();

            Console.WriteLine("Треки альбома з кількістю прослуховувань більше середнього:");
            foreach (var track in tracksAboveAvg)
            {
                Console.WriteLine($"- {track.Title} ({track.Listens} прослуховувань)");
            }
            Console.WriteLine();

            
            int artistId = 1; 

            var topTracks = context.Tracks
                .Where(t => t.Album.ArtistId == artistId)
                .OrderByDescending(t => t.Rating)
                .Take(3)
                .ToList();

            Console.WriteLine("ТОП-3 треки артиста за рейтингом:");
            foreach (var track in topTracks)
            {
                Console.WriteLine($"- {track.Title} (Рейтинг: {track.Rating})");
            }
            Console.WriteLine();

            var topAlbums = context.Albums
                .Where(a => a.ArtistId == artistId)
                .OrderByDescending(a => a.Rating)
                .Take(3)
                .ToList();

            Console.WriteLine("ТОП-3 альбоми артиста за рейтингом:");
            foreach (var album in topAlbums)
            {
                Console.WriteLine($"- {album.Title} (Рейтинг: {album.Rating})");
            }
            Console.WriteLine();

            
            string search = "love"; 

            var foundTracks = context.Tracks
                .Where(t => t.Title.Contains(search) || (t.Lyrics != null && t.Lyrics.Contains(search)))
                .ToList();

            Console.WriteLine($"Результати пошуку треків за запитом \"{search}\":");
            foreach (var track in foundTracks)
            {
                Console.WriteLine($"- {track.Title} (Lyrics: {track.Lyrics})");
            }
        }
    }
}
