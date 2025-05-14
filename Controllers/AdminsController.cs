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

    

   
}
