using LMS.Application.DTOs;
using LMS.Application.Interfaces.Services;
using LMS.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LeaveRequestsController : ControllerBase
    {
        private readonly ILeaveRequestService _leaveRequestService;

        public LeaveRequestsController(ILeaveRequestService leaveRequestService)
        {
            _leaveRequestService = leaveRequestService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (User.IsInRole("Admin"))
            {
                return Ok(await _leaveRequestService.GetAllAsync());
            }
            
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            return Ok(await _leaveRequestService.GetByUserIdAsync(userId));
        }

        [HttpPost]
        public async Task<IActionResult> Apply([FromBody] CreateLeaveRequestDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            try
            {
                var result = await _leaveRequestService.ApplyLeaveAsync(userId, dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/approve")]
        public async Task<IActionResult> Approve(int id, [FromBody] ReviewLeaveRequestDto dto)
        {
            dto.Status = LeaveStatus.Approved;
            await _leaveRequestService.ReviewLeaveAsync(id, dto);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/reject")]
        public async Task<IActionResult> Reject(int id, [FromBody] ReviewLeaveRequestDto dto)
        {
            dto.Status = LeaveStatus.Rejected;
            await _leaveRequestService.ReviewLeaveAsync(id, dto);
            return NoContent();
        }
    }
}
