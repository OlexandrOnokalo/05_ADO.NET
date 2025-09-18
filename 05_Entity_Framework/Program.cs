using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Emit;

namespace _05_Entity_Framework
{
    
    internal class MusicDbContext : DbContext
    {
        public MusicDbContext()
        {
            
            this.Database.EnsureDeleted();
            this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(@"Data Source=PULSE\SQLEXPRESS;
                                        Initial Catalog=MusicAppDb;
                                        Integrated Security=True;
                                        Connect Timeout=5;
                                        Encrypt=False;TrustServerCertificate=True;
                                        Application Intent=ReadWrite;Multi Subnet Failover=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<PlaylistTrack>().HasKey(pt => new { pt.PlaylistId, pt.TrackId });

            
            modelBuilder.Entity<Country>().HasData(
                new Country { Id = 1, Name = "USA" },
                new Country { Id = 2, Name = "UK" },
                new Country { Id = 3, Name = "Sweden" },
                new Country { Id = 4, Name = "Ukraine" },
                new Country { Id = 5, Name = "Canada" }
            );

            modelBuilder.Entity<Genre>().HasData(
                new Genre { Id = 1, Name = "Rock" },
                new Genre { Id = 2, Name = "Pop" },
                new Genre { Id = 3, Name = "Jazz" },
                new Genre { Id = 4, Name = "Classical" },
                new Genre { Id = 5, Name = "Electronic" },
                new Genre { Id = 6, Name = "HipHop" }
            );

            modelBuilder.Entity<Artist>().HasData(
                new Artist { Id = 1, FirstName = "John", LastName = "Smith", CountryId = 1 },
                new Artist { Id = 2, FirstName = "Emma", LastName = "Brown", CountryId = 2 },
                new Artist { Id = 3, FirstName = "Max", LastName = "Johansson", CountryId = 3 },
                new Artist { Id = 4, FirstName = "Oleg", LastName = "Ivanov", CountryId = 4 },
                new Artist { Id = 5, FirstName = "Alice", LastName = "Green", CountryId = 5 }
            );

            modelBuilder.Entity<Album>().HasData(
                new Album { Id = 1, Title = "Greatest Hits", ArtistId = 1, Year = 2010, GenreId = 1 },
                new Album { Id = 2, Title = "Soft Sounds", ArtistId = 2, Year = 2015, GenreId = 2 },
                new Album { Id = 3, Title = "Nordic Beats", ArtistId = 3, Year = 2018, GenreId = 5 },
                new Album { Id = 4, Title = "Folk Tales", ArtistId = 4, Year = 2012, GenreId = 4 },
                new Album { Id = 5, Title = "Green Days", ArtistId = 5, Year = 2020, GenreId = 2 },
                new Album { Id = 6, Title = "Smooth Jazz", ArtistId = 2, Year = 2016, GenreId = 3 }
            );

            modelBuilder.Entity<Track>().HasData(
                new Track { Id = 1, Title = "Hit One", AlbumId = 1, DurationSeconds = 240 },
                new Track { Id = 2, Title = "Hit Two", AlbumId = 1, DurationSeconds = 200 },
                new Track { Id = 3, Title = "Soft Intro", AlbumId = 2, DurationSeconds = 180 },
                new Track { Id = 4, Title = "Soft Ballad", AlbumId = 2, DurationSeconds = 210 },
                new Track { Id = 5, Title = "Nordic Storm", AlbumId = 3, DurationSeconds = 300 },
                new Track { Id = 6, Title = "Nordic Calm", AlbumId = 3, DurationSeconds = 260 },
                new Track { Id = 7, Title = "Folk Song 1", AlbumId = 4, DurationSeconds = 220 },
                new Track { Id = 8, Title = "Folk Song 2", AlbumId = 4, DurationSeconds = 195 },
                new Track { Id = 9, Title = "Green Day 1", AlbumId = 5, DurationSeconds = 230 },
                new Track { Id = 10, Title = "Green Day 2", AlbumId = 5, DurationSeconds = 205 },
                new Track { Id = 11, Title = "Smooth Flow", AlbumId = 6, DurationSeconds = 250 },
                new Track { Id = 12, Title = "Jazz Night", AlbumId = 6, DurationSeconds = 275 }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Workout" },
                new Category { Id = 2, Name = "Chill" },
                new Category { Id = 3, Name = "Party" },
                new Category { Id = 4, Name = "Focus" }
            );

            modelBuilder.Entity<Playlist>().HasData(
                new Playlist { Id = 1, Name = "Morning Boost", CategoryId = 1 },
                new Playlist { Id = 2, Name = "Evening Chill", CategoryId = 2 },
                new Playlist { Id = 3, Name = "Party Hits", CategoryId = 3 }
            );

            modelBuilder.Entity<PlaylistTrack>().HasData(
                
                new PlaylistTrack { PlaylistId = 1, TrackId = 1 },
                new PlaylistTrack { PlaylistId = 1, TrackId = 5 },
                new PlaylistTrack { PlaylistId = 1, TrackId = 9 },
                
                new PlaylistTrack { PlaylistId = 2, TrackId = 3 },
                new PlaylistTrack { PlaylistId = 2, TrackId = 6 },
                new PlaylistTrack { PlaylistId = 2, TrackId = 11 },
                new PlaylistTrack { PlaylistId = 2, TrackId = 12 },
                
                new PlaylistTrack { PlaylistId = 3, TrackId = 2 },
                new PlaylistTrack { PlaylistId = 3, TrackId = 4 },
                new PlaylistTrack { PlaylistId = 3, TrackId = 10 },
                new PlaylistTrack { PlaylistId = 3, TrackId = 5 }
            );
        }

        
        public DbSet<Country> Countries { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<PlaylistTrack> PlaylistTracks { get; set; }
    }

    
    [Table("Countries")]
    class Country
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        public ICollection<Artist> Artists { get; set; }
    }

    
    [Table("Artists")]
    class Artist
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string FirstName { get; set; }

        [Required, MaxLength(100)]
        public string LastName { get; set; }

        
        public int CountryId { get; set; }
        public Country Country { get; set; }

        public ICollection<Album> Albums { get; set; }
    }

    
    [Table("Genres")]
    class Genre
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }
    }

    
    [Table("Albums")]
    class Album
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; }

        public int ArtistId { get; set; }
        public Artist Artist { get; set; }

        public int Year { get; set; }

        public int GenreId { get; set; }
        public Genre Genre { get; set; }

        public ICollection<Track> Tracks { get; set; }
    }

    
    [Table("Tracks")]
    class Track
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; }

        public int AlbumId { get; set; }
        public Album Album { get; set; }

        // Рус.: длительность трека в секундах
        public int DurationSeconds { get; set; }

        public ICollection<PlaylistTrack> PlaylistTracks { get; set; }
    }

    
    [Table("Categories")]
    class Category
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        public ICollection<Playlist> Playlists { get; set; }
    }

    
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

    
    [Table("PlaylistTracks")]
    class PlaylistTrack
    {
        public int PlaylistId { get; set; }
        public Playlist Playlist { get; set; }

        public int TrackId { get; set; }
        public Track Track { get; set; }
    }

    
    internal class Program
    {
        static void Main(string[] args)
        {
            using var context = new MusicDbContext();

            
            context.Database.EnsureCreated();

            Console.WriteLine("Seeded playlists:");
            var seeded = context.Playlists.Include(p => p.Category).ToList();
            foreach (var p in seeded)
            {
                Console.WriteLine($"{p.Id}: {p.Name} [{p.Category?.Name}]");
            }

            
            var newPlaylist = new Playlist { Name = "My Custom List", CategoryId = 4 };
            context.Playlists.Add(newPlaylist);
            context.SaveChanges();
            Console.WriteLine($"Created playlist with Id = {newPlaylist.Id}");

            
            var firstTwo = context.Tracks.Take(2).ToList();
            foreach (var t in firstTwo)
            {
                context.PlaylistTracks.Add(new PlaylistTrack { PlaylistId = newPlaylist.Id, TrackId = t.Id });
            }
            context.SaveChanges();
            Console.WriteLine("Added tracks to the new playlist.");

            
            var loaded = context.Playlists
                .Where(p => p.Id == newPlaylist.Id)
                .Include(p => p.PlaylistTracks).ThenInclude(pt => pt.Track)
                .FirstOrDefault();

            Console.WriteLine($"Playlist: {loaded.Name}");
            foreach (var pt in loaded.PlaylistTracks)
            {
                Console.WriteLine($" - {pt.Track.Title} ({pt.Track.DurationSeconds}s)");
            }
        }
    }
}
