using TrackManagement.Application.DTOs;

namespace TrackManagement.Application.Interfaces;

public interface ITrackService
{
      Task<List<TrackDto>> GetAllAsync(int? artistId, string? genre, string? status);
      Task<TrackDetailDto?> GetByIdAsync(int id);
      Task<TrackDto> CreateAsync(CreateTrackDto dto);
      Task<bool> UpdateStatusAsync(int id, UpdateTrackStatusDto dto);
      Task<TrackDetailDto?> DistributeAsync(int id, DistributeTrackDto dto);
}