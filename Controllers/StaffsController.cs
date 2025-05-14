using Microsoft.AspNetCore.Mvc;
using KitabhChautari.Dto; // Added for StaffDto
using System.Collections.Generic;
using System.Threading.Tasks;
using KitabhChautari.Services;

namespace KitabhChautari.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffsController : ControllerBase
    {
        private readonly IStaffService _staffService;

        public StaffsController(IStaffService staffService)
        {
            _staffService = staffService;
        }

        // GET: api/Staffs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StaffDto>>> GetStaff()
        {
            var staffList = await _staffService.GetAllStaff();
            return Ok(staffList);
        }

        // GET: api/Staffs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StaffDto>> GetStaff(int id)
        {
            var staff = await _staffService.GetStaffById(id);
            if (staff == null)
            {
                return NotFound();
            }
            return Ok(staff);
        }

        // PUT: api/Staffs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStaff(int id, StaffDto staffDto)
        {
            if (id != staffDto.StaffId)
            {
                return BadRequest("Staff ID mismatch");
            }

            try
            {
                await _staffService.UpdateStaff(id, staffDto);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Staffs
        [HttpPost]
        public async Task<ActionResult<StaffDto>> PostStaff(StaffDto staffDto)
        {
            var createdStaff = await _staffService.CreateStaff(staffDto);
            return CreatedAtAction(nameof(GetStaff),
                 new { id = createdStaff.StaffId },
                 createdStaff);
        }

        // DELETE: api/Staffs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStaff(int id)
        {
            try
            {
                await _staffService.DeleteStaff(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}