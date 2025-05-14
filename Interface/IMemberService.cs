using kitabhChauta.Models;
using KitabhChautari.Dto;

public interface IMemberService
{
    Task<IEnumerable<Member>> GetAllMembers();
    Task<IEnumerable<Member>> GetAllMembers(int page, int pageSize);
    Task<Member> GetMemberById(int id);
    Task<Member> CreateMember(MemberDto memberDto);
    Task UpdateMember(int id, MemberDto memberDto);
    Task DeleteMember(int id);
    Task<bool> MemberExists(int id);
    Task AssignBookToMember(int memberId, int bookId);
}