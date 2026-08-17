namespace TrackManagement.Domain.Entities;

public enum TrackStatus
{
    Draft,
    Submitted,
    Distributed
}

public enum DistributionStatus
{
    Pending,
    Live,
    Rejected
}