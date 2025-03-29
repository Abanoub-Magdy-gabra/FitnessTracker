using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FitnessTracker.API.Models.Requests.Users;
using FitnessTracker.API.Models.Responses.Users;
using FitnessTracker.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<UserResponse>>> GetUsers([FromQuery] UserFilterRequest filter)
        {
            var users = await _userService.GetUsersAsync(filter);
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDetailResponse>> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserResponse>> CreateUser(CreateUserRequest request)
        {
            var user = await _userService.CreateUserAsync(request);
            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserRequest request)
        {
            if (id != request.UserId)
                return BadRequest();

            await _userService.UpdateUserAsync(request);
            return NoContent();
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateUserStatus(int id, [FromBody] string status)
        {
            await _userService.UpdateUserStatusAsync(id, status);
            return NoContent();
        }

        [HttpGet("{id}/statistics")]
        public async Task<ActionResult<UserStatisticsResponse>> GetUserStatistics(int id)
        {
            var statistics = await _userService.GetUserStatisticsAsync(id);
            return Ok(statistics);
        }

        [HttpGet("{id}/subscriptions")]
        public async Task<ActionResult<IEnumerable<SubscriptionResponse>>> GetUserSubscriptions(int id)
        {
            var subscriptions = await _userService.GetUserSubscriptionsAsync(id);
            return Ok(subscriptions);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }
    }
}
