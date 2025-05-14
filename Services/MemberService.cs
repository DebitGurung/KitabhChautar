using KitabhChautari.Dto;
using KitabhChautari.Services;
using kitabhChautari.Data;
using kitabhChautari.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KitabhChautari.Services
{
    public class MemberService : IMemberService
    {
        private readonly KitabhChautariDbContext _context;

        public MemberService(KitabhChautariDbContext context)
        {
<<<<<<< HEAD
            MemberId = memberDto.MemberId,
            FirstName = memberDto.FirstName,
            LastName = memberDto.LastName,
            Email = memberDto.Email,
            DateOfBirth = memberDto.DateOfBirth,
            RegistrationDate = DateTime.UtcNow
        };
=======
            _context = context;
        }
>>>>>>> f5451a52d1c4c87b33f69c61b45926a525e29c94

        public async Task<IEnumerable<Member>> GetAllMembers()
            => await _context.Members.ToListAsync();

        public async Task<Member> GetMemberById(int id)
            => await _context.Members.FindAsync(id) ?? throw new KeyNotFoundException("Member not found");

<<<<<<< HEAD
        if (await _context.Members.AnyAsync(m => m.Email == memberDto.Email && m.MemberId != id))
            throw new InvalidOperationException("Email already exists");

        member.MemberId = memberDto.MemberId;
        member.FirstName = memberDto.FirstName;
        member.LastName = memberDto.LastName;
        member.Email = memberDto.Email;
        member.DateOfBirth = memberDto.DateOfBirth;


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
=======
        public async Task<Member> CreateMember(MemberDto dto)
>>>>>>> f5451a52d1c4c87b33f69c61b45926a525e29c94
        {
            var member = new Member
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                ContactNo = dto.ContactNo,
                DateOfBirth = dto.DateOfBirth,
                MembershipStatus = dto.MembershipStatus
            };

            _context.Members.Add(member);
            await _context.SaveChangesAsync();
            return member;
        }

        public async Task UpdateMember(int id, MemberDto dto)
        {
            var member = await _context.Members.FindAsync(id)
                ?? throw new KeyNotFoundException("Member not found");

            member.FirstName = dto.FirstName;
            member.LastName = dto.LastName;
            member.ContactNo = dto.ContactNo;
            member.DateOfBirth = dto.DateOfBirth;
            member.MembershipStatus = dto.MembershipStatus;

            await _context.SaveChangesAsync();
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
    }

    public Task AssignBookToMember(int memberId, int bookId)
    {
        throw new NotImplementedException();
    }
}