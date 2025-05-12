using System.Collections.Generic;
using System.Threading.Tasks;
using KitabhChautari.Dto;
using KitabhChautari.Dtos;

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

        // User CRUD
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUserById(int id);
        Task UpdateUser(int id, UserDto dto);
        Task DeleteUser(int id);
        Task<bool> UserExists(int id);

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
        Task<Member> CreateMember(MemberDto dto);
        Task UpdateMember(int id, MemberDto dto);
        Task DeleteMember(int id);
        Task<bool> MemberExists(int id);

        // Book CRUD
        Task<IEnumerable<Book>> GetAllBooks();
        Task<Book> GetBookById(int id);
        Task<Book> CreateBook(BookDto dto, int adminId);
        Task UpdateBook(int id, BookDto dto);
        Task DeleteBook(int id);
        Task<bool> BookExists(int id);
    }
}