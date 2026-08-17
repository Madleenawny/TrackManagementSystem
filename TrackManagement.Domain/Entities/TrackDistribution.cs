namespace TrackManagement.Domain.Entities;

public class TrackDistribution
{
    public int Id { get; set; }
    public int TrackId { get; set; }
    public int DspId { get; set; }
    public DateTime SubmittedAt { get; set; }
    public DistributionStatus Status { get; set; } = DistributionStatus.Pending;

    public Track? Track { get; set; }
    public DSP? Dsp { get; set; }
}