using Microsoft.AspNetCore.Mvc;
using TrackManagement.Application.DTOs;
using TrackManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace TrackManagement.API.Controllers;

[ApiController]
[Route("api/artists")]
[Authorize]
public class ArtistsController : ControllerBase
{
      private readonly IArtistService _artistService;

      public ArtistsController(IArtistService artistService)
      {
            _artistService = artistService;
      }

      // GET /api/artists
      [HttpGet]
      public async Task<ActionResult<List<ArtistDto>>> GetAll()
      {
            var artists = await _artistService.GetAllAsync();
            return Ok(artists);
      }

      // POST /api/artists
      [HttpPost]
      public async Task<ActionResult<ArtistDto>> Create([FromBody] CreateArtistDto dto)
      {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                  return BadRequest(new { message = "Name is required." });
            }

            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                  return BadRequest(new { message = "Email is required." });
            }

            var created = await _artistService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetAll), new { id = created.Id }, created);
      }
}