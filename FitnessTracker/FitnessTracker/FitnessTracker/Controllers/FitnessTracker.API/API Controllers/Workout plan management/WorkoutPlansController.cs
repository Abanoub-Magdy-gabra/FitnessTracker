using System.Collections.Generic;
using System.Threading.Tasks;
using FitnessTracker.API.Models.Requests.WorkoutPlans;
using FitnessTracker.API.Models.Responses.WorkoutPlans;
using FitnessTracker.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class WorkoutPlansController : ControllerBase
    {
        private readonly IWorkoutPlanService _workoutPlanService;

        public WorkoutPlansController(IWorkoutPlanService workoutPlanService)
        {
            _workoutPlanService = workoutPlanService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkoutPlanResponse>>> GetWorkoutPlans([FromQuery] WorkoutPlanFilterRequest filter)
        {
            var plans = await _workoutPlanService.GetWorkoutPlansAsync(filter);
            return Ok(plans);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WorkoutPlanDetailResponse>> GetWorkoutPlan(int id)
        {
            var plan = await _workoutPlanService.GetWorkoutPlanByIdAsync(id);
            if (plan == null)
                return NotFound();

            return Ok(plan);
        }

        [HttpPost]
        public async Task<ActionResult<WorkoutPlanResponse>> CreateWorkoutPlan(CreateWorkoutPlanRequest request)
        {
            var plan = await _workoutPlanService.CreateWorkoutPlanAsync(request);
            return CreatedAtAction(nameof(GetWorkoutPlan), new { id = plan.PlanId }, plan);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWorkoutPlan(int id, UpdateWorkoutPlanRequest request)
        {
            if (id != request.PlanId)
                return BadRequest();

            await _workoutPlanService.UpdateWorkoutPlanAsync(request);
            return NoContent();
        }

        [HttpPut("{id}/feature")]
        public async Task<IActionResult> ToggleFeatureWorkoutPlan(int id)
        {
            await _workoutPlanService.ToggleFeatureWorkoutPlanAsync(id);
            return NoContent();
        }

        [HttpGet("{id}/workouts")]
        public async Task<ActionResult<IEnumerable<WorkoutResponse>>> GetWorkoutsForPlan(int id)
        {
            var workouts = await _workoutPlanService.GetWorkoutsForPlanAsync(id);
            return Ok(workouts);
        }

        [HttpPost("{id}/workouts")]
        public async Task<ActionResult<WorkoutResponse>> AddWorkoutToPlan(int id, CreateWorkoutRequest request)
        {
            var workout = await _workoutPlanService.AddWorkoutToPlanAsync(id, request);
            return Ok(workout);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkoutPlan(int id)
        {
            await _workoutPlanService.DeleteWorkoutPlanAsync(id);
            return NoContent();
        }
    }
}