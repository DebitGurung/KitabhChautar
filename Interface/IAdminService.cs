using KitabhChautari.Dto; // For MemberDto, StaffDto, AdminDto
using kitabhChautari.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KitabhChautari.Services
{
    public interface IAdminService
    {
        // Admin CRUD
        Task<IEnumerable<Admin>> GetAllAdmins(int page = 1, int pageSize = 10);
        Task<Admin> GetAdminById(int id);
        Task<Admin> CreateAdmin(AdminDto dto);
        Task UpdateAdmin(int id, AdminDto dto);
        Task DeleteAdmin(int id);
        Task<bool> AdminExists(int id);
        Task<Admin> AuthenticateAdmin(string email, string password);

        // Staff CRUD
        Task<IEnumerable<Staff>> GetAllStaff();
        Task<Staff> GetStaffById(int id);
        Task<Staff> CreateStaff(StaffDto dto);
        Task UpdateStaff(int id, StaffDto dto);
        Task DeleteStaff(int id);
        Task<bool> StaffExists(int id);

        // Member CRUD
        Task<IEnumerable<Member>> GetAllMembers();
        Task<Member> GetMemberById(int id);
        Task<Member> CreateMember(KitabhChautari.Dto.MemberDto dto);
        Task UpdateMember(int id, KitabhChautari.Dto.MemberDto dto);
        Task DeleteMember(int id);
        Task<bool> MemberExists(int id);

 
    }
}