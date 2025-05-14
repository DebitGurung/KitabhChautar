using KitabhChautari.Dto;
using kitabhChauta.DbContext;
using kitabhChauta.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
        {
            return await _context.Members.ToListAsync();
        }

        public async Task<IEnumerable<Member>> GetAllMembers(int page, int pageSize)
        {
            return await _context.Members
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Member> GetMemberById(int id)
        {
            return await _context.Members.FindAsync(id)
                ?? throw new KeyNotFoundException("Member not found");
        }

        public async Task<Member> CreateMember(MemberDto memberDto)
        {
            if (await _context.Members.AnyAsync(m => m.Email == memberDto.Email))
            {
                throw new InvalidOperationException("Email already exists");
            }

            var member = new Member
            {
                FirstName = memberDto.FirstName,
                LastName = memberDto.LastName,
                Email = memberDto.Email,
                ContactNo = memberDto.ContactNo,
                DateOfBirth = memberDto.DateOfBirth,
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
            {
                throw new InvalidOperationException("Email already exists");
            }

            member.FirstName = memberDto.FirstName;
            member.LastName = memberDto.LastName;
            member.Email = memberDto.Email;
            member.ContactNo = memberDto.ContactNo;
            member.DateOfBirth = memberDto.DateOfBirth;

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
        {
            return await _context.Members.AnyAsync(m => m.MemberId == id);
        }

        public async Task AssignBookToMember(int memberId, int bookId)
        {
            // Implementation for assigning a book to a member
            throw new NotImplementedException();
        }

        private async Task SaveWithConcurrencyCheck(int id)
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await MemberExists(id))
                {
                    throw new KeyNotFoundException("Member not found");
                }
                throw;
            }
        }
    }
}