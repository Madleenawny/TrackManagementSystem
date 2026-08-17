using Microsoft.AspNetCore.Mvc;
using TrackManagement.Application.DTOs;
using TrackManagement.Application.Interfaces;

namespace TrackManagement.API.Controllers;

[ApiController]
[Route("api/dsps")]
public class DspsController : ControllerBase
{
      private readonly IDspService _dspService;

      public DspsController(IDspService dspService)
      {
            _dspService = dspService;
      }

      // GET /api/dsps
      [HttpGet]
      public async Task<ActionResult<List<DspDto>>> GetAll()
      {
            var dsps = await _dspService.GetAllAsync();
            return Ok(dsps);
      }
}