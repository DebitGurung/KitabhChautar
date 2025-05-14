using KitabhChautari.Dto;
using kitabhChautari.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KitabhChautari.Services
{
    public interface IMemberService
    {
        Task<IEnumerable<Member>> GetAllMembers();
        Task<Member> GetMemberById(int id);
        Task<Member> CreateMember(MemberDto dto);
        Task UpdateMember(int id, MemberDto dto);
        Task DeleteMember(int id);
        Task<bool> MemberExists(int id);
    }
}