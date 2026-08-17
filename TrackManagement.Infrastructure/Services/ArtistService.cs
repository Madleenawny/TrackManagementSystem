using Microsoft.EntityFrameworkCore;
using TrackManagement.Application.DTOs;
using TrackManagement.Application.Interfaces;
using TrackManagement.Domain.Entities;
using TrackManagement.Infrastructure.Persistence;

namespace TrackManagement.Infrastructure.Services;

public class ArtistService : IArtistService
{
      private readonly AppDbContext _context;

      public ArtistService(AppDbContext context)
      {
            _context = context;
      }

      public async Task<List<ArtistDto>> GetAllAsync()
      {
            return await _context.Artists
                .Select(a => new ArtistDto
                {
                      Id = a.Id,
                      Name = a.Name,
                      Email = a.Email,
                      Country = a.Country
                })
                .ToListAsync();
      }

      public async Task<ArtistDto> CreateAsync(CreateArtistDto dto)
      {
            var artist = new Artist
            {
                  Name = dto.Name,
                  Email = dto.Email,
                  Country = dto.Country
            };

            _context.Artists.Add(artist);
            await _context.SaveChangesAsync();

            return new ArtistDto
            {
                  Id = artist.Id,
                  Name = artist.Name,
                  Email = artist.Email,
                  Country = artist.Country
            };
      }
}