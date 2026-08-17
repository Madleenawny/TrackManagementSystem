namespace TrackManagement.Domain.Entities;

public class Track
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int ArtistId { get; set; }
    public string Isrc { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }
    public string Genre { get; set; } = string.Empty;
    public TrackStatus Status { get; set; } = TrackStatus.Draft;
 
    public Artist? Artist { get; set; }
    public ICollection<TrackDistribution> Distributions { get; set; } = new List<TrackDistribution>();
}