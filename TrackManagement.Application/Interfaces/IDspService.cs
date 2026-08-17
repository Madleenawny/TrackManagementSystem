using TrackManagement.Application.DTOs;

namespace TrackManagement.Application.Interfaces;

public interface IDspService
{
      Task<List<DspDto>> GetAllAsync();
}