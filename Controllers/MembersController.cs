using Microsoft.AspNetCore.Mvc;
using KitabhChautari.Dto;
using KitabhChautari.Services;
using kitabhChauta.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KitabhChautari.Controllers
{
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
        public async Task<ActionResult<IEnumerable<Member>>> GetMembers()
        {
            var members = await _memberService.GetAllMembers();
            return Ok(members);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetMember(int id)
        {
            try
            {
                var member = await _memberService.GetMemberById(id);
                return Ok(member);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult<Member>> PostMember(MemberDto dto)
        {
            try
            {
                var created = await _memberService.CreateMember(dto);
                return CreatedAtAction(nameof(GetMember), new { id = created.MemberId }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMember(int id, MemberDto dto)
        {
            try
            {
                await _memberService.UpdateMember(id, dto);
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
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            try
            {
                await _memberService.DeleteMember(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}