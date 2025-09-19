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
            context.Database.


            //Console.WriteLine("Seeded playlists:");
            //var seeded = context.Playlists.Include(p => p.Category).ToList();
            //foreach (var p in seeded)
            //{
            //    Console.WriteLine($"{p.Id}: {p.Name} [{p.Category?.Name}]");
            //}

            
            //var newPlaylist = new Playlist { Name = "My Custom List", CategoryId = 4 };
            //context.Playlists.Add(newPlaylist);
            //context.SaveChanges();
            //Console.WriteLine($"Created playlist with Id = {newPlaylist.Id}");

            
            //var firstTwo = context.Tracks.Take(2).ToList();
            //foreach (var t in firstTwo)
            //{
            //    context.PlaylistTracks.Add(new PlaylistTrack { PlaylistId = newPlaylist.Id, TrackId = t.Id });
            //}
            //context.SaveChanges();
            //Console.WriteLine("Added tracks to the new playlist.");

            
            //var loaded = context.Playlists
            //    .Where(p => p.Id == newPlaylist.Id)
            //    .Include(p => p.PlaylistTracks).ThenInclude(pt => pt.Track)
            //    .FirstOrDefault();

            //Console.WriteLine($"Playlist: {loaded.Name}");
            //foreach (var pt in loaded.PlaylistTracks)
            //{
            //    Console.WriteLine($" - {pt.Track.Title} ({pt.Track.DurationSeconds}s)");
            //}
        }
    }
}
