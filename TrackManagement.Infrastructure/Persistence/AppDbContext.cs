using Microsoft.EntityFrameworkCore;
using TrackManagement.Domain.Entities;

namespace TrackManagement.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Artist> Artists => Set<Artist>();
    public DbSet<Track> Tracks => Set<Track>();
    public DbSet<DSP> Dsps => Set<DSP>();
    public DbSet<TrackDistribution> TrackDistributions => Set<TrackDistribution>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Artist -> Tracks (one-to-many)
        modelBuilder.Entity<Track>()
            .HasOne(t => t.Artist)
            .WithMany(a => a.Tracks)
            .HasForeignKey(t => t.ArtistId)
            .OnDelete(DeleteBehavior.Cascade);

        // Track has a unique ISRC code — no two tracks can share one
        modelBuilder.Entity<Track>()
            .HasIndex(t => t.Isrc)
            .IsUnique();

        // TrackDistribution -> Track (one-to-many)
        modelBuilder.Entity<TrackDistribution>()
            .HasOne(td => td.Track)
            .WithMany(t => t.Distributions)
            .HasForeignKey(td => td.TrackId)
            .OnDelete(DeleteBehavior.Cascade);

        // TrackDistribution -> DSP (one-to-many)
        modelBuilder.Entity<TrackDistribution>()
            .HasOne(td => td.Dsp)
            .WithMany(d => d.Distributions)
            .HasForeignKey(td => td.DspId)
            .OnDelete(DeleteBehavior.Restrict);

        // A track can't be submitted twice to the same DSP
        modelBuilder.Entity<TrackDistribution>()
            .HasIndex(td => new { td.TrackId, td.DspId })
            .IsUnique();


        modelBuilder.Entity<DSP>().HasData(
                    new DSP { Id = 1, Name = "Spotify" },
                    new DSP { Id = 2, Name = "Apple Music" },
                    new DSP { Id = 3, Name = "YouTube Music" }
                );

        // 2. Seed Artists
        modelBuilder.Entity<Artist>().HasData(
            new Artist { Id = 1, Name = "Amr Diab", Email = "amr@music.com", Country = "Egypt" },
            new Artist { Id = 2, Name = "Angham", Email = "angham@music.com", Country = "Egypt" },
            new Artist { Id = 3, Name = "Cairokee", Email = "info@cairokee.com", Country = "Egypt" }
        );


        // 3. Seed Tracks (8 الأغاني المطلوبة بأصناف وحالات مختلفة)
        modelBuilder.Entity<Track>().HasData(
            new Track { Id = 1, Title = "Nour El Ain", ArtistId = 1, Isrc = "EGX000000001", ReleaseDate = new DateTime(2024, 1, 1), Genre = "Pop", Status = TrackStatus.Draft },
            new Track { Id = 2, Title = "Tamally Maak", ArtistId = 1, Isrc = "EGX000000002", ReleaseDate = new DateTime(2024, 2, 1), Genre = "Pop", Status = TrackStatus.Submitted },
            new Track { Id = 3, Title = "Sot El Horeya", ArtistId = 3, Isrc = "EGX000000003", ReleaseDate = new DateTime(2024, 3, 1), Genre = "Rock", Status = TrackStatus.Distributed },
            new Track { Id = 4, Title = "Ya Ana Ya La", ArtistId = 1, Isrc = "EGX000000004", ReleaseDate = new DateTime(2024, 4, 1), Genre = "Pop", Status = TrackStatus.Draft },
            new Track { Id = 5, Title = "Sidi Wesalak", ArtistId = 2, Isrc = "EGX000000005", ReleaseDate = new DateTime(2024, 5, 1), Genre = "Pop", Status = TrackStatus.Submitted },
            new Track { Id = 6, Title = "Khatam Solaiman", ArtistId = 3, Isrc = "EGX000000006", ReleaseDate = new DateTime(2024, 6, 1), Genre = "Rock", Status = TrackStatus.Draft },
            new Track { Id = 7, Title = "Omri Kabeer", ArtistId = 2, Isrc = "EGX000000007", ReleaseDate = new DateTime(2024, 7, 1), Genre = "Classical", Status = TrackStatus.Distributed },
            new Track { Id = 8, Title = "Basrah W Ahooh", ArtistId = 3, Isrc = "EGX000000008", ReleaseDate = new DateTime(2024, 8, 1), Genre = "Rock", Status = TrackStatus.Draft }
        );
    }
}