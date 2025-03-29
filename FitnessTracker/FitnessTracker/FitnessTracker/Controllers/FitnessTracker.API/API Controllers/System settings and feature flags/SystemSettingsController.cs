using System.Collections.Generic;
using System.Threading.Tasks;
using FitnessTracker.API.Models.Requests.Settings;
using FitnessTracker.API.Models.Responses.Settings;
using FitnessTracker.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class SystemSettingsController : ControllerBase
    {
        private readonly ISystemSettingsService _settingsService;

        public SystemSettingsController(ISystemSettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        [HttpGet("app-settings")]
        public async Task<ActionResult<IEnumerable<AppSettingResponse>>> GetAppSettings([FromQuery] string category = null)
        {
            var settings = await _settingsService.GetAppSettingsAsync(category);
            return Ok(settings);
        }

        [HttpGet("app-settings/{id}")]
        public async Task<ActionResult<AppSettingResponse>> GetAppSetting(int id)
        {
            var setting = await _settingsService.GetAppSettingByIdAsync(id);
            if (setting == null)
                return NotFound();

            return Ok(setting);
        }

        [HttpPut("app-settings/{id}")]
        public async Task<IActionResult> UpdateAppSetting(int id, UpdateAppSettingRequest request)
        {
            if (id != request.SettingId)
                return BadRequest();

            await _settingsService.UpdateAppSettingAsync(request);
            return NoContent();
        }

        [HttpPut("app-settings/{id}/toggle")]
        public async Task<IActionResult> ToggleAppSetting(int id)
        {
            await _settingsService.ToggleAppSettingAsync(id);
            return NoContent();
        }

        [HttpGet("feature-flags")]
        public async Task<ActionResult<IEnumerable<FeatureFlagResponse>>> GetFeatureFlags()
        {
            var featureFlags = await _settingsService.GetFeatureFlagsAsync();
            return Ok(featureFlags);
        }

        [HttpGet("feature-flags/{id}")]
        public async Task<ActionResult<FeatureFlagResponse>> GetFeatureFlag(int id)
        {
            var featureFlag = await _settingsService.GetFeatureFlagByIdAsync(id);
            if (featureFlag == null)
                return NotFound();

            return Ok(featureFlag);
        }

        [HttpPut("feature-flags/{id}")]
        public async Task<IActionResult> UpdateFeatureFlag(int id, UpdateFeatureFlagRequest request)
        {
            if (id != request.FlagId)
                return BadRequest();

            await _settingsService.UpdateFeatureFlagAsync(request);
            return NoContent();
        }

        [HttpPut("feature-flags/{id}/toggle")]
        public async Task<IActionResult> ToggleFeatureFlag(int id)
        {
            await _settingsService.ToggleFeatureFlagAsync(id);
            return NoContent();
        }

        [HttpPut("feature-flags/{id}/percentage")]
        public async Task<IActionResult> UpdateFeatureFlagPercentage(int id, [FromBody] int percentage)
        {
            await _settingsService.UpdateFeatureFlagPercentageAsync(id, percentage);
            return NoContent();
        }

        [HttpGet("app-settings/categories")]
        public async Task<ActionResult<IEnumerable<string>>> GetSettingCategories()
        {
            var categories = await _settingsService.GetSettingCategoriesAsync();
            return Ok(categories);
        }
    }
}