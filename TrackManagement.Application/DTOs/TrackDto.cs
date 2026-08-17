using TrackManagement.Domain.Entities;

namespace TrackManagement.Application.DTOs;

// بيستخدم لما نرجع بيانات أغنية للـ frontend (بدون تفاصيل)
public class TrackDto
{
      public int Id { get; set; }
      public string Title { get; set; } = string.Empty;
      public int ArtistId { get; set; }
      public string ArtistName { get; set; } = string.Empty;
      public string Isrc { get; set; } = string.Empty;
      public DateTime ReleaseDate { get; set; }
      public string Genre { get; set; } = string.Empty;
      public string Status { get; set; } = string.Empty;
}

// بيستخدم لما نرجع تفاصيل أغنية كاملة، شاملة حالة التوزيع
public class TrackDetailDto : TrackDto
{
      public List<TrackDistributionDto> Distributions { get; set; } = new();
}

public class TrackDistributionDto
{
      public int Id { get; set; }
      public int DspId { get; set; }
      public string DspName { get; set; } = string.Empty;
      public DateTime SubmittedAt { get; set; }
      public string Status { get; set; } = string.Empty;
}

// بيستخدم لما الـ frontend يبعتلنا أغنية جديدة عايزة تتعمل
public class CreateTrackDto
{
      public string Title { get; set; } = string.Empty;
      public int ArtistId { get; set; }
      public string Isrc { get; set; } = string.Empty;
      public DateTime ReleaseDate { get; set; }
      public string Genre { get; set; } = string.Empty;
}

// بيستخدم لما نحدث حالة الأغنية
public class UpdateTrackStatusDto
{
      public TrackStatus Status { get; set; }
}

// بيستخدم لما نبعت أغنية لمنصة أو أكتر
public class DistributeTrackDto
{
      public List<int> DspIds { get; set; } = new();
}