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
            _context = context;
        }

        public async Task<IEnumerable<Member>> GetAllMembers()
            => await _context.Members.ToListAsync();

        public async Task<Member> GetMemberById(int id)
            => await _context.Members.FindAsync(id) ?? throw new KeyNotFoundException("Member not found");

        public async Task<Member> CreateMember(MemberDto dto)
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
}