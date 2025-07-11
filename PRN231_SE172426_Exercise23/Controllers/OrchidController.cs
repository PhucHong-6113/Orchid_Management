using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs;
using Service.OrchidService;

namespace PRN231_SE172426_Exercise23.Controllers
{
    [ApiController]
    [Route("api/orchids")]
    public class OrchidController : ControllerBase
    {
        private readonly IOrchidService _orchidService;

        public OrchidController(IOrchidService orchidService)
        {
            _orchidService = orchidService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<OrchidDto>>> GetAllOrchids()
        {
            var orchids = await _orchidService.GetAllOrchidsAsync();
            return Ok(orchids);
        }
          
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<OrchidDto>> GetOrchidById(Guid id)
        {
            var orchid = await _orchidService.GetOrchidByIdAsync(id);

            if (orchid == null)
                return NotFound();

            return Ok(orchid);
        }

        [HttpPost]
        [Authorize(Roles = "admin,seller")]
        public async Task<ActionResult<OrchidDto>> AddOrchid([FromBody] CreateOrchidDto createOrchidDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _orchidService.AddOrchidAsync(createOrchidDto);

            if (result == null)
                return BadRequest("Failed to add orchid. Please check if the category exists.");

            return CreatedAtAction(nameof(GetOrchidById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin,seller")]
        public async Task<ActionResult<OrchidDto>> UpdateOrchid(Guid id, [FromBody] CreateOrchidDto updateOrchidDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _orchidService.UpdateOrchidAsync(id, updateOrchidDto);
            
            if (result == null)
                return BadRequest("Failed to update orchid. Please check if the orchid exists and the category is valid.");
            
            return Ok(result);
        }
    }
}