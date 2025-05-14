using Microsoft.EntityFrameworkCore;
using KitabhChautari.Dto;
using kitabhChauta.Models;
using kitabhChauta.DbContext;

namespace KitabhChautari.Services
{
    public class AdminService : IAdminService
    {
        private readonly KitabhChautariDbContext _context;

        public AdminService(KitabhChautariDbContext context)
        {
            _context = context;
        }

        // Admin CRUD
        public async Task<IEnumerable<Admin>> GetAllAdmins(int page = 1, int pageSize = 10)
            => await _context.Admins
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

        public async Task<Admin> GetAdminById(int id)
            => await _context.Admins.FindAsync(id) ?? throw new KeyNotFoundException("Admin not found");

        public async Task<Admin> CreateAdmin(AdminDto dto)
        {
            if (await _context.Admins.AnyAsync(a => a.Email == dto.Email))
                throw new InvalidOperationException("Email already exists");

            var admin = new Admin
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = dto.Role,
                CreatedAt = DateTime.UtcNow
            };

            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();
            return admin;
        }

        public async Task UpdateAdmin(int id, AdminDto dto)
        {
            var admin = await _context.Admins.FindAsync(id)
                ?? throw new KeyNotFoundException("Admin not found");

            if (await _context.Admins.AnyAsync(a => a.Email == dto.Email && a.AdminId != id))
                throw new InvalidOperationException("Email already exists");

            admin.Name = dto.Name;
            admin.Email = dto.Email;
            if (!string.IsNullOrEmpty(dto.Password))
                admin.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            admin.Role = dto.Role;
            admin.UpdatedAt = DateTime.UtcNow;

            _context.Entry(admin).State = EntityState.Modified;
            await SaveWithConcurrencyCheck<Admin>(id, AdminExists);
        }

        public async Task DeleteAdmin(int id) => await DeleteEntity<Admin>(id);

        public async Task<bool> AdminExists(int id) => await EntityExists<Admin>(id);

        public async Task<Admin> AuthenticateAdmin(string email, string password)
        {
            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Email == email && a.IsActive);
            if (admin == null || !BCrypt.Net.BCrypt.Verify(password, admin.PasswordHash))
                throw new UnauthorizedAccessException("Invalid email or password");
            return admin;
        }

       
        

        // Generic Helpers
        private async Task DeleteEntity<T>(int id) where T : class
        {
            var entity = await _context.FindAsync<T>(id)
                ?? throw new KeyNotFoundException($"{typeof(T).Name} not found");
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }

        private async Task<bool> EntityExists<T>(int id) where T : class
            => await _context.Set<T>().AnyAsync(e => EF.Property<int>(e, $"{typeof(T).Name}Id") == id);

        private async Task SaveWithConcurrencyCheck<T>(int id, Func<int, Task<bool>> existsCheck) where T : class
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await existsCheck(id))
                    throw new KeyNotFoundException($"{typeof(T).Name} not found");
                throw;
            }
        }

        public Task<IEnumerable<Staff>> GetAllStaff()
        {
            throw new NotImplementedException();
        }

        public Task<Staff> GetStaffById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Staff> CreateStaff(StaffDto dto)
        {
            throw new NotImplementedException();
        }

        public Task UpdateStaff(int id, StaffDto dto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteStaff(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> StaffExists(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Member>> GetAllMembers()
        {
            throw new NotImplementedException();
        }

        public Task<Member> GetMemberById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Member> CreateMember(MemberDto dto)
        {
            throw new NotImplementedException();
        }

        public Task UpdateMember(int id, MemberDto dto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMember(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> MemberExists(int id)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Admin>> IAdminService.GetAllAdmins(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        Task<Admin> IAdminService.GetAdminById(int id)
        {
            throw new NotImplementedException();
        }

        Task<Admin> IAdminService.CreateAdmin(AdminDto dto)
        {
            throw new NotImplementedException();
        }

        Task<Admin> IAdminService.AuthenticateAdmin(string email, string password)
        {
            throw new NotImplementedException();
        }
    }
}