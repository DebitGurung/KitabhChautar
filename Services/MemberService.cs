using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
public class MemberService : IMemberService
{
    private readonly KitabhChautariDbContext _context;

    public MemberService(KitabhChautariDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Member>> GetAllMembers(int page = 1, int pageSize = 10)
        => await _context.Members
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

    public async Task<Member> GetMemberById(int id)
        => await _context.Members.FindAsync(id) ?? throw new KeyNotFoundException("Member not found");

    public async Task<Member> CreateMember(MemberDto memberDto)
    {
        if (await _context.Members.AnyAsync(m => m.Email == memberDto.Email))
            throw new InvalidOperationException("Email already exists");

        if (await _context.Members.AnyAsync(m => m.MemberId == memberDto.MemberId))
            throw new InvalidOperationException("Member already exists");



        var member = new Member
        {
            MemberId = memberDto.MemberId,
            FirstName = memberDto.FirstName,
            LastName = memberDto.LastName,
            Email = memberDto.Email,
            DateOfBirth = memberDto.DateOfBirth,
            MembershipStatus = memberDto.MembershipStatus,
            RegistrationDate = DateTime.UtcNow
        };

        _context.Members.Add(member);
        await _context.SaveChangesAsync();
        return member;
    }

    public async Task UpdateMember(int id, MemberDto memberDto)
    {
        var member = await _context.Members.FindAsync(id)
            ?? throw new KeyNotFoundException("Member not found");

        if (await _context.Members.AnyAsync(m => m.Email == memberDto.Email && m.MemberId != id))
            throw new InvalidOperationException("Email already exists");

        member.MemberId = memberDto.MemberId;
        member.FirstName = memberDto.FirstName;
        member.LastName = memberDto.LastName;
        member.Email = memberDto.Email;
        member.DateOfBirth = memberDto.DateOfBirth;
        member.MembershipStatus = memberDto.MembershipStatus;

        _context.Entry(member).State = EntityState.Modified;
        await SaveWithConcurrencyCheck(id);
    }

    public async Task DeleteMember(int id)
    {
        var member = await _context.Members.FindAsync(id)
            ?? throw new KeyNotFoundException("Member not found");
        _context.Members.Remove(member);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> MemberExists(int id)
        => await _context.Members.AnyAsync(m => m.MemberId == id);

    

    private async Task SaveWithConcurrencyCheck(int id)
    {
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await MemberExists(id)) throw new KeyNotFoundException("Member not found");
            throw;
        }
    }

    public Task AssignBookToMember(int memberId, int bookId)
    {
        throw new NotImplementedException();
    }
}