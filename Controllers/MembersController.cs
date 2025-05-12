using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class MembersController : ControllerBase
{
    private readonly IMemberService _memberService;

    public MembersController(IMemberService memberService)
    {
        _memberService = memberService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Member>>> GetMembers(int page = 1, int pageSize = 10)
    {
        var members = await _memberService.GetAllMembers(page, pageSize);
        return Ok(members);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Member>> GetMember(int id)
    {
        var member = await _memberService.GetMemberById(id);
        return member == null ? NotFound() : Ok(member);
    }

    [HttpPost]
    public async Task<ActionResult<Member>> PostMember([FromBody] MemberDto memberDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var created = await _memberService.CreateMember(memberDto);
            return CreatedAtAction(nameof(GetMember), new { id = created.MemberId }, created);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutMember(int id, [FromBody] MemberDto memberDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            await _memberService.UpdateMember(id, memberDto);
            return NoContent();
        }
        catch (ArgumentException) { return BadRequest("ID mismatch"); }
        catch (KeyNotFoundException) { return NotFound(); }
        catch (InvalidOperationException ex) { return BadRequest(ex.Message); }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _memberService.MemberExists(id)) return NotFound();
            throw;
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMember(int id)
    {
        try
        {
            await _memberService.DeleteMember(id);
            return NoContent();
        }
        catch (KeyNotFoundException) { return NotFound(); }
    }

    [HttpPost("{memberId}/books/{bookId}")]
    public async Task<IActionResult> AssignBookToMember(int memberId, int bookId)
    {
        try
        {
            await _memberService.AssignBookToMember(memberId, bookId);
            return NoContent();
        }
        catch (KeyNotFoundException) { return NotFound(); }
        catch (InvalidOperationException ex) { return BadRequest(ex.Message); }
    }
}