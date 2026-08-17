using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackManagement.Application.DTOs;
using TrackManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace TrackManagement.API.Controllers;

[ApiController]
[Route("api/tracks")]
public class TracksController : ControllerBase
{
      private readonly ITrackService _trackService;

      public TracksController(ITrackService trackService)
      {
            _trackService = trackService;
      }

      // GET /api/tracks?artistId=&genre=&status=
      [HttpGet]
      public async Task<ActionResult<List<TrackDto>>> GetAll(
          [FromQuery] int? artistId,
          [FromQuery] string? genre,
          [FromQuery] string? status)
      {
            var tracks = await _trackService.GetAllAsync(artistId, genre, status);
            return Ok(tracks);
      }

      // GET /api/tracks/{id}
      [HttpGet("{id}")]
      public async Task<ActionResult<TrackDetailDto>> GetById(int id)
      {
            var track = await _trackService.GetByIdAsync(id);

            if (track == null)
            {
                  return NotFound(new { message = $"Track with id {id} was not found." });
            }

            return Ok(track);
      }

      // POST /api/tracks
      [HttpPost]
      [Authorize]
      public async Task<ActionResult<TrackDto>> Create([FromBody] CreateTrackDto dto)
      {
            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                  return BadRequest(new { message = "Title is required." });
            }

            if (string.IsNullOrWhiteSpace(dto.Isrc))
            {
                  return BadRequest(new { message = "ISRC is required." });
            }

            if (dto.ArtistId <= 0)
            {
                  return BadRequest(new { message = "A valid ArtistId is required." });
            }

            try
            {
                  var created = await _trackService.CreateAsync(dto);
                  return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                  return Conflict(new { message = "A track with this ISRC already exists, or the ArtistId is invalid." });
            }
      }

      // PATCH /api/tracks/{id}/status
      [HttpPatch("{id}/status")]
      [Authorize]
      public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateTrackStatusDto dto)
      {
            var updated = await _trackService.UpdateStatusAsync(id, dto);

            if (!updated)
            {
                  return NotFound(new { message = $"Track with id {id} was not found." });
            }

            return NoContent();
      }

      // POST /api/tracks/{id}/distribute
      [HttpPost("{id}/distribute")]
      [Authorize]
      public async Task<ActionResult<TrackDetailDto>> Distribute(int id, [FromBody] DistributeTrackDto dto)
      {
            if (dto.DspIds == null || dto.DspIds.Count == 0)
            {
                  return BadRequest(new { message = "At least one DspId is required." });
            }

            var result = await _trackService.DistributeAsync(id, dto);

            if (result == null)
            {
                  return NotFound(new { message = $"Track with id {id} was not found." });
            }

            return Ok(result);
      }
}