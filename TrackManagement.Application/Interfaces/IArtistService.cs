using TrackManagement.Application.DTOs;

namespace TrackManagement.Application.Interfaces;

public interface IArtistService
{
      Task<List<ArtistDto>> GetAllAsync();
      Task<ArtistDto> CreateAsync(CreateArtistDto dto);
}