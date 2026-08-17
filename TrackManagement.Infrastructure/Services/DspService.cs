using Microsoft.EntityFrameworkCore;
using TrackManagement.Application.DTOs;
using TrackManagement.Application.Interfaces;
using TrackManagement.Infrastructure.Persistence;

namespace TrackManagement.Infrastructure.Services;

public class DspService : IDspService
{
      private readonly AppDbContext _context;

      public DspService(AppDbContext context)
      {
            _context = context;
      }

      public async Task<List<DspDto>> GetAllAsync()
      {
            return await _context.Dsps
                .Select(d => new DspDto { Id = d.Id, Name = d.Name })
                .ToListAsync();
      }
}