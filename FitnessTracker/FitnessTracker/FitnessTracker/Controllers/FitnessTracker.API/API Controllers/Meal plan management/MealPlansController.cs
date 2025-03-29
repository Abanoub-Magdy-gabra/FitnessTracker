using System.Collections.Generic;
using System.Threading.Tasks;
using FitnessTracker.API.Models.Requests.MealPlans;
using FitnessTracker.API.Models.Responses.MealPlans;
using FitnessTracker.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class MealPlansController : ControllerBase
    {
        private readonly IMealPlanService _mealPlanService;

        public MealPlansController(IMealPlanService mealPlanService)
        {
            _mealPlanService = mealPlanService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MealPlanResponse>>> GetMealPlans([FromQuery] MealPlanFilterRequest filter)
        {
            var plans = await _mealPlanService.GetMealPlansAsync(filter);
            return Ok(plans);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MealPlanDetailResponse>> GetMealPlan(int id)
        {
            var plan = await _mealPlanService.GetMealPlanByIdAsync(id);
            if (plan == null)
                return NotFound();

            return Ok(plan);
        }

        [HttpPost]
        public async Task<ActionResult<MealPlanResponse>> CreateMealPlan(CreateMealPlanRequest request)
        {
            var plan = await _mealPlanService.CreateMealPlanAsync(request);
            return CreatedAtAction(nameof(GetMealPlan), new { id = plan.PlanId }, plan);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMealPlan(int id, UpdateMealPlanRequest request)
        {
            if (id != request.PlanId)
                return BadRequest();

            await _mealPlanService.UpdateMealPlanAsync(request);
            return NoContent();
        }

        [HttpPut("{id}/visibility")]
        public async Task<IActionResult> ToggleMealPlanVisibility(int id)
        {
            await _mealPlanService.ToggleMealPlanVisibilityAsync(id);
            return NoContent();
        }

        [HttpGet("{id}/meals")]
        public async Task<ActionResult<IEnumerable<MealResponse>>> GetMealsForPlan(int id)
        {
            var meals = await _mealPlanService.GetMealsForPlanAsync(id);
            return Ok(meals);
        }

        [HttpPost("{id}/meals")]
        public async Task<ActionResult<MealResponse>> AddMealToPlan(int id, CreateMealRequest request)
        {
            var meal = await _mealPlanService.AddMealToPlanAsync(id, request);
            return Ok(meal);
        }

        [HttpGet("diet-types")]
        public async Task<ActionResult<IEnumerable<string>>> GetDietTypes()
        {
            var dietTypes = await _mealPlanService.GetDietTypesAsync();
            return Ok(dietTypes);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMealPlan(int id)
        {
            await _mealPlanService.DeleteMealPlanAsync(id);
            return NoContent();
        }
    }
}