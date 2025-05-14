using KitabhChautari.Dto; // For MemberDto, StaffDto, AdminDto
using kitabhChauta.Models;
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



    }
}