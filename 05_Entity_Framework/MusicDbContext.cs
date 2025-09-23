using _05_Entity_Framework.Entytis;
using _05_Entity_Framework.Helpers;
using Microsoft.EntityFrameworkCore;

namespace _05_Entity_Framework
{
    internal class MusicDbContext : DbContext
    {
        public MusicDbContext()
        {

            //this.Database.EnsureDeleted();
            //this.Database.EnsureCreated();
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

            
            modelBuilder.Entity<Track>(entity =>
            {
                entity.ToTable("Tracks");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Lyrics).HasMaxLength(200);

                entity.HasOne(e => e.Album)
                      .WithMany(a => a.Tracks)
                      .HasForeignKey(e => e.AlbumId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            
            modelBuilder.Entity<Album>(entity =>
            {
                entity.ToTable("Albums");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);

                entity.HasOne(e => e.Artist)
                      .WithMany(a => a.Albums)
                      .HasForeignKey(e => e.ArtistId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Genre)
                      .WithMany(g => g.Albums)
                      .HasForeignKey(e => e.GenreId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            
            modelBuilder.Entity<Artist>(entity =>
            {
                entity.ToTable("Artists");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);

                entity.HasOne(e => e.Country)
                      .WithMany(c => c.Artists)
                      .HasForeignKey(e => e.CountryId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            
            modelBuilder.Entity<Genre>(entity =>
            {
                entity.ToTable("Genres");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            });

            
            modelBuilder.Entity<Playlist>(entity =>
            {
                entity.ToTable("Playlists");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);

                entity.HasOne(e => e.Category)
                      .WithMany(c => c.Playlists)
                      .HasForeignKey(e => e.CategoryId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Categories");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            });

            
            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("Countries");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            });

            
            modelBuilder.Entity<PlaylistTrack>(entity =>
            {
                entity.ToTable("PlaylistTracks");
                entity.HasKey(pt => new { pt.PlaylistId, pt.TrackId });

                entity.HasOne(pt => pt.Playlist)
                      .WithMany(p => p.PlaylistTracks)
                      .HasForeignKey(pt => pt.PlaylistId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(pt => pt.Track)
                      .WithMany(t => t.PlaylistTracks)
                      .HasForeignKey(pt => pt.TrackId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            
            modelBuilder.SeedCountries();
            modelBuilder.SeedGenres();
            modelBuilder.SeedArtists();
            modelBuilder.SeedAlbums();
            modelBuilder.SeedTracks();
            modelBuilder.SeedCategories();
            modelBuilder.SeedPlaylists();
            modelBuilder.SeedPlaylistTracks();
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
}
