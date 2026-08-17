using Microsoft.EntityFrameworkCore;
using TrackManagement.Application.DTOs;
using TrackManagement.Application.Interfaces;
using TrackManagement.Domain.Entities;
using TrackManagement.Infrastructure.Persistence;

namespace TrackManagement.Infrastructure.Services;

public class TrackService : ITrackService
{
      private readonly AppDbContext _context;

      public TrackService(AppDbContext context)
      {
            _context = context;
      }

      public async Task<List<TrackDto>> GetAllAsync(int? artistId, string? genre, string? status)
      {
            var query = _context.Tracks
                .AsNoTracking()
                .Include(t => t.Artist)
                .AsQueryable();

            if (artistId.HasValue)
            {
                  query = query.Where(t => t.ArtistId == artistId.Value);
            }

            if (!string.IsNullOrWhiteSpace(genre))
            {
                  query = query.Where(t => t.Genre == genre);
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                  if (Enum.TryParse<TrackStatus>(status, true, out var statusEnum))
                  {
                        query = query.Where(t => t.Status == statusEnum);
                  }
            }

            return await query
                .Select(t => new TrackDto
                {
                      Id = t.Id,
                      Title = t.Title,
                      ArtistId = t.ArtistId,
                      ArtistName = t.Artist!.Name,
                      Isrc = t.Isrc,
                      ReleaseDate = t.ReleaseDate,
                      Genre = t.Genre,
                      Status = t.Status.ToString()
                })
                .ToListAsync();
      }

      public async Task<TrackDto> CreateAsync(CreateTrackDto dto)
      {
            var track = new Track
            {
                  Title = dto.Title,
                  ArtistId = dto.ArtistId,
                  Isrc = dto.Isrc,
                  ReleaseDate = dto.ReleaseDate,
                  Genre = dto.Genre,
                  Status = TrackStatus.Draft
            };

            _context.Tracks.Add(track);
            await _context.SaveChangesAsync();

            var artist = await _context.Artists.FindAsync(dto.ArtistId);

            return new TrackDto
            {
                  Id = track.Id,
                  Title = track.Title,
                  ArtistId = track.ArtistId,
                  ArtistName = artist?.Name ?? string.Empty,
                  Isrc = track.Isrc,
                  ReleaseDate = track.ReleaseDate,
                  Genre = track.Genre,
                  Status = track.Status.ToString()
            };
      }
      public async Task<bool> UpdateStatusAsync(int id, UpdateTrackStatusDto dto)
      {
            var track = await _context.Tracks.FindAsync(id);

            if (track == null)
            {
                  return false;
            }

            track.Status = dto.Status;
            await _context.SaveChangesAsync();

            return true;
      }
      public async Task<TrackDetailDto?> GetByIdAsync(int id)
      {
            var track = await _context.Tracks
                .AsNoTracking()
                .Include(t => t.Artist)
                .Include(t => t.Distributions)
                    .ThenInclude(d => d.Dsp)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (track == null)
            {
                  return null;
            }

            return new TrackDetailDto
            {
                  Id = track.Id,
                  Title = track.Title,
                  ArtistId = track.ArtistId,
                  ArtistName = track.Artist!.Name,
                  Isrc = track.Isrc,
                  ReleaseDate = track.ReleaseDate,
                  Genre = track.Genre,
                  Status = track.Status.ToString(),
                  Distributions = track.Distributions.Select(d => new TrackDistributionDto
                  {
                        Id = d.Id,
                        DspId = d.DspId,
                        DspName = d.Dsp!.Name,
                        SubmittedAt = d.SubmittedAt,
                        Status = d.Status.ToString()
                  }).ToList()
            };
      }

      public async Task<TrackDetailDto?> DistributeAsync(int id, DistributeTrackDto dto)
      {
            var track = await _context.Tracks
                .Include(t => t.Distributions)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (track == null)
            {
                  return null;
            }

            foreach (var dspId in dto.DspIds.Distinct())
            {
                  var alreadyExists = track.Distributions.Any(d => d.DspId == dspId);
                  if (alreadyExists)
                  {
                        continue;
                  }

                  var dspExists = await _context.Dsps.AnyAsync(d => d.Id == dspId);
                  if (!dspExists)
                  {
                        continue;
                  }

                  track.Distributions.Add(new TrackDistribution
                  {
                        TrackId = track.Id,
                        DspId = dspId,
                        SubmittedAt = DateTime.UtcNow,
                        Status = DistributionStatus.Pending
                  });
            }

            if (track.Distributions.Any() && track.Status == TrackStatus.Draft)
            {
                  track.Status = TrackStatus.Submitted;
            }

            await _context.SaveChangesAsync();

            return await GetByIdAsync(id);
      }

}