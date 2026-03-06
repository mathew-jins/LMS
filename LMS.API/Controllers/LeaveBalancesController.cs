using LMS.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LeaveBalancesController : ControllerBase
    {
        private readonly ILeaveBalanceService _leaveBalanceService;

        public LeaveBalancesController(ILeaveBalanceService leaveBalanceService)
        {
            _leaveBalanceService = leaveBalanceService;
        }

        [HttpGet("my-balance")]
        public async Task<IActionResult> GetMyBalance()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var res = await _leaveBalanceService.GetByUserIdAsync(userId);
            return Ok(res);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("initialize/{userId}/{year}")]
        public async Task<IActionResult> Initialize(int userId, int year)
        {
            await _leaveBalanceService.InitializeBalancesForUserAsync(userId, year);
            return Ok();
        }
    }
}
