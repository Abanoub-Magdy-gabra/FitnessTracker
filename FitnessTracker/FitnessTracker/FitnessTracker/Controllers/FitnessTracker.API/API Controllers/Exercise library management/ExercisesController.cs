using System.Collections.Generic;
using System.Threading.Tasks;
using FitnessTracker.API.Models.Requests.Exercises;
using FitnessTracker.API.Models.Responses.Exercises;
using FitnessTracker.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class ExercisesController : ControllerBase
    {
        private readonly IExerciseService _exerciseService;

        public ExercisesController(IExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExerciseResponse>>> GetExercises([FromQuery] ExerciseFilterRequest filter)
        {
            var exercises = await _exerciseService.GetExercisesAsync(filter);
            return Ok(exercises);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExerciseDetailResponse>> GetExercise(int id)
        {
            var exercise = await _exerciseService.GetExerciseByIdAsync(id);
            if (exercise == null)
                return NotFound();

            return Ok(exercise);
        }

        [HttpPost]
        public async Task<ActionResult<ExerciseResponse>> CreateExercise(CreateExerciseRequest request)
        {
            var exercise = await _exerciseService.CreateExerciseAsync(request);
            return CreatedAtAction(nameof(GetExercise), new { id = exercise.ExerciseId }, exercise);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExercise(int id, UpdateExerciseRequest request)
        {
            if (id != request.ExerciseId)
                return BadRequest();

            await _exerciseService.UpdateExerciseAsync(request);
            return NoContent();
        }

        [HttpPut("{id}/verify")]
        public async Task<IActionResult> VerifyExercise(int id)
        {
            await _exerciseService.VerifyExerciseAsync(id);
            return NoContent();
        }

        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<string>>> GetExerciseCategories()
        {
            var categories = await _exerciseService.GetExerciseCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("equipment")]
        public async Task<ActionResult<IEnumerable<string>>> GetExerciseEquipment()
        {
            var equipment = await _exerciseService.GetExerciseEquipmentAsync();
            return Ok(equipment);
        }

        [HttpGet("muscles")]
        public async Task<ActionResult<IEnumerable<string>>> GetMuscleGroups()
        {
            var muscles = await _exerciseService.GetMuscleGroupsAsync();
            return Ok(muscles);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExercise(int id)
        {
            await _exerciseService.DeleteExerciseAsync(id);
            return NoContent();
        }
    }
}