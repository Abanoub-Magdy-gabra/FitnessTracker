using System.Collections.Generic;
using System.Threading.Tasks;
using FitnessTracker.API.Models.Requests.Coaches;
using FitnessTracker.API.Models.Responses.Coaches;
using FitnessTracker.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class CoachesController : ControllerBase
    {
        private readonly ICoachService _coachService;

        public CoachesController(ICoachService coachService)
        {
            _coachService = coachService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CoachResponse>>> GetCoaches([FromQuery] CoachFilterRequest filter)
        {
            var coaches = await _coachService.GetCoachesAsync(filter);
            return Ok(coaches);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CoachDetailResponse>> GetCoach(int id)
        {
            var coach = await _coachService.GetCoachByIdAsync(id);
            if (coach == null)
                return NotFound();

            return Ok(coach);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCoach(int id, UpdateCoachRequest request)
        {
            if (id != request.CoachId)
                return BadRequest();

            await _coachService.UpdateCoachAsync(request);
            return NoContent();
        }

        [HttpPut("{id}/verify")]
        public async Task<IActionResult> VerifyCoach(int id)
        {
            await _coachService.VerifyCoachAsync(id);
            return NoContent();
        }

        [HttpPut("{id}/accepting-clients")]
        public async Task<IActionResult> UpdateAcceptingClients(int id, [FromBody] bool isAcceptingClients)
        {
            await _coachService.UpdateAcceptingClientsAsync(id, isAcceptingClients);
            return NoContent();
        }

        [HttpGet("{id}/clients")]
        public async Task<ActionResult<IEnumerable<CoachClientResponse>>> GetCoachClients(int id)
        {
            var clients = await _coachService.GetCoachClientsAsync(id);
            return Ok(clients);
        }

        [HttpGet("{id}/reviews")]
        public async Task<ActionResult<IEnumerable<CoachReviewResponse>>> GetCoachReviews(int id)
        {
            var reviews = await _coachService.GetCoachReviewsAsync(id);
            return Ok(reviews);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCoach(int id)
        {
            await _coachService.DeleteCoachAsync(id);
            return NoContent();
        }
    }
}
