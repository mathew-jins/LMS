using LMS.Application.DTOs;
using LMS.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LeavePeriodsController : ControllerBase
    {
        private readonly ILeavePeriodService _leavePeriodService;

        public LeavePeriodsController(ILeavePeriodService leavePeriodService)
        {
            _leavePeriodService = leavePeriodService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _leavePeriodService.GetAllAsync());
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            return Ok(await _leavePeriodService.GetActivePeriodAsync());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LeavePeriodDto dto)
        {
            return Ok(await _leavePeriodService.CreateAsync(dto));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] LeavePeriodDto dto)
        {
            await _leavePeriodService.UpdateAsync(id, dto);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _leavePeriodService.DeleteAsync(id);
            return NoContent();
        }
    }
}
