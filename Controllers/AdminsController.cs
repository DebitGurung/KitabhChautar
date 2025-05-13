using KitabhChautari;
using KitabhChautari.Dto;
using KitabhChautari.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class AdminsController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminsController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    // ---------------- ADMIN ----------------
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Admin>>> GetAdmins(int page = 1, int pageSize = 10)
        => Ok(await _adminService.GetAllAdmins(page, pageSize));

    [HttpGet("{id}")]
    public async Task<ActionResult<Admin>> GetAdmin(int id)
    {
        try
        {
            var admin = await _adminService.GetAdminById(id);
            return Ok(admin);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<ActionResult<Admin>> PostAdmin(AdminDto dto)
    {
        try
        {
            var created = await _adminService.CreateAdmin(dto);
            return CreatedAtAction(nameof(GetAdmin), new { id = created.AdminId }, created);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutAdmin(int id, AdminDto dto)
    {
        try
        {
            await _adminService.UpdateAdmin(id, dto);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _adminService.AdminExists(id))
                return NotFound();
            throw;
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAdmin(int id)
    {
        try
        {
            await _adminService.DeleteAdmin(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    // ---------------- USERS ----------------
    [HttpGet("users")]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        => Ok(await _adminService.GetAllUsers());

    [HttpGet("users/{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        try
        {
            var user = await _adminService.GetUserById(id);
            return Ok(user);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPut("users/{id}")]
    public async Task<IActionResult> UpdateUser(int id, UserDto dto)
    {
        try
        {
            await _adminService.UpdateUser(id, dto);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _adminService.UserExists(id))
                return NotFound();
            throw;
        }
    }

    [HttpDelete("users/{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            await _adminService.DeleteUser(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    // ---------------- MEMBERS ----------------
    [HttpGet("members")]
    public async Task<ActionResult<IEnumerable<Member>>> GetMembers()
        => Ok(await _adminService.GetAllMembers());

    [HttpGet("members/{id}")]
    public async Task<ActionResult<Member>> GetMember(int id)
    {
        try
        {
            var member = await _adminService.GetMemberById(id);
            return Ok(member);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost("members")]
    public async Task<ActionResult<Member>> PostMember(MemberDto dto)
    {
        try
        {
            var created = await _adminService.CreateMember(dto);
            return CreatedAtAction(nameof(GetMember), new { id = created.MemberId }, created);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("members/{id}")]
    public async Task<IActionResult> UpdateMember(int id, MemberDto dto)
    {
        try
        {
            await _adminService.UpdateMember(id, dto);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _adminService.MemberExists(id))
                return NotFound();
            throw;
        }
    }

    [HttpDelete("members/{id}")]
    public async Task<IActionResult> DeleteMember(int id)
    {
        try
        {
            await _adminService.DeleteMember(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    // ---------------- STAFF ----------------
    [HttpGet("staff")]
    public async Task<ActionResult<IEnumerable<Staff>>> GetStaff()
        => Ok(await _adminService.GetAllStaff());

    [HttpGet("staff/{id}")]
    public async Task<ActionResult<Staff>> GetStaff(int id)
    {
        try
        {
            var staff = await _adminService.GetStaffById(id);
            return Ok(staff);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost("staff")]
    public async Task<ActionResult<Staff>> PostStaff(StaffDto dto)
    {
        try
        {
            var created = await _adminService.CreateStaff(dto);
            return CreatedAtAction(nameof(GetStaff), new { id = created.StaffId }, created);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("staff/{id}")]
    public async Task<IActionResult> UpdateStaff(int id, StaffDto dto)
    {
        try
        {
            await _adminService.UpdateStaff(id, dto);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _adminService.StaffExists(id))
                return NotFound();
            throw;
        }
    }

    [HttpDelete("staff/{id}")]
    public async Task<IActionResult> DeleteStaff(int id)
    {
        try
        {
            await _adminService.DeleteStaff(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

   
}
