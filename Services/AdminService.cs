using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using KitabhChautari;
using BCrypt.Net;
using KitabhChautari.Dto;
using KitabhChautari.Dtos;

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

        // User CRUD
        public async Task<IEnumerable<User>> GetAllUsers()
            => await _context.Users.ToListAsync();

        public async Task<User> GetUserById(int id)
            => await _context.Users.FindAsync(id) ?? throw new KeyNotFoundException("User not found");

        public async Task UpdateUser(int id, UserDto dto)
        {
            var user = await _context.Users.FindAsync(id)
                ?? throw new KeyNotFoundException("User not found");

            if (await _context.Users.AnyAsync(u => u.Email == dto.Email && u.UserId != id))
                throw new InvalidOperationException("Email already exists");

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Email = dto.Email;

            _context.Entry(user).State = EntityState.Modified;
            await SaveWithConcurrencyCheck<User>(id, UserExists);
        }

        public async Task DeleteUser(int id) => await DeleteEntity<User>(id);

        public async Task<bool> UserExists(int id) => await EntityExists<User>(id);

        // Staff CRUD
        public async Task<IEnumerable<Staff>> GetAllStaff()
            => await _context.Staffs.ToListAsync();

        public async Task<Staff> GetStaffById(int id)
            => await _context.Staffs.FindAsync(id) ?? throw new KeyNotFoundException("Staff not found");

        public async Task<Staff> CreateStaff(StaffDto dto)
        {
            if (await _context.Staffs.AnyAsync(s => s.Email == dto.Email))
                throw new InvalidOperationException("Email already exists");

            var staff = new Staff
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName = dto.FirstName,
                Email = dto.Email,
            };

            _context.Staffs.Add(staff);
            await _context.SaveChangesAsync();
            return staff;
        }

        public async Task UpdateStaff(int id, StaffDto dto)
        {
            var staff = await _context.Staffs.FindAsync(id)
                ?? throw new KeyNotFoundException("Staff not found");

            if (await _context.Staffs.AnyAsync(s => s.Email == dto.Email && s.StaffId != id))
                throw new InvalidOperationException("Email already exists");

            staff.FirstName = dto.FirstName;
            staff.LastName = dto.LastName;
            staff.Email = dto.Email;

            _context.Entry(staff).State = EntityState.Modified;
            await SaveWithConcurrencyCheck<Staff>(id, StaffExists);
        }

        public async Task DeleteStaff(int id) => await DeleteEntity<Staff>(id);

        public async Task<bool> StaffExists(int id) => await EntityExists<Staff>(id);

        // Member CRUD
        public async Task<IEnumerable<Member>> GetAllMembers()
            => await _context.Members.ToListAsync();

        public async Task<Member> GetMemberById(int id)
            => await _context.Members.FindAsync(id) ?? throw new KeyNotFoundException("Member not found");

        public async Task<Member> CreateMember(MemberDto dto)
        {
            if (await _context.Members.AnyAsync(m => m.Email == dto.Email))
                throw new InvalidOperationException("Email already exists");

            var member = new Member
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName = dto.FirstName,
                Email = dto.Email,
            };

            _context.Members.Add(member);
            await _context.SaveChangesAsync();
            return member;
        }

        public async Task UpdateMember(int id, MemberDto dto)
        {
            var member = await _context.Members.FindAsync(id)
                ?? throw new KeyNotFoundException("Member not found");

            if (await _context.Members.AnyAsync(m => m.Email == dto.Email && m.MemberId != id))
                throw new InvalidOperationException("Email already exists");

            member.FirstName = dto.FirstName;
            member.LastName = dto.LastName;

            member.Email = dto.Email;

            _context.Entry(member).State = EntityState.Modified;
            await SaveWithConcurrencyCheck<Member>(id, MemberExists);
        }

        public async Task DeleteMember(int id) => await DeleteEntity<Member>(id);

        public async Task<bool> MemberExists(int id) => await EntityExists<Member>(id);

        // Book CRUD
        public async Task<IEnumerable<Book>> GetAllBooks() => await _context.Books.ToListAsync();

        public async Task<Book> GetBookById(int id)
            => await _context.Books.FindAsync(id) ?? throw new KeyNotFoundException("Book not found");

        public async Task<Book> CreateBook(BookDto dto, int adminId)
        {
            var admin = await _context.Admins.FindAsync(adminId)
                ?? throw new KeyNotFoundException("Admin not found");

            var book = new Book
            {
                Title = dto.Title,
                Author = dto.Author,
                Genre = dto.Genre,
                ISBN = dto.ISBN,
                Price = dto.Price,
                PublishedDate = dto.PublishedDate,
                Pages = dto.Pages,
                StockCount = dto.StockCount,
                Synopsis = dto.Synopsis,
                CoverImageUrl = dto.CoverImageUrl,
                AdminId = adminId,
                MemberId = dto.MemberId,
                StaffId = dto.StaffId
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }
        public async Task UpdateBook(int id, BookDto dto)
        {
            var book = await _context.Books.FindAsync(id)
                ?? throw new KeyNotFoundException("Book not found");

            book.Title = dto.Title;
            book.Author = dto.Author;
            book.Genre = dto.Genre;
            book.ISBN = dto.ISBN;
            book.Price = dto.Price;
            book.PublishedDate = dto.PublishedDate;
            book.Pages = dto.Pages;
            book.StockCount = dto.StockCount;
            book.Synopsis = dto.Synopsis;
            book.CoverImageUrl = dto.CoverImageUrl;
            book.MemberId = dto.MemberId;
            book.StaffId = dto.StaffId;

            _context.Entry(book).State = EntityState.Modified;
            await SaveWithConcurrencyCheck<Book>(id, BookExists);
        }

        public async Task DeleteBook(int id) => await DeleteEntity<Book>(id);

        public async Task<bool> BookExists(int id) => await EntityExists<Book>(id);

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
    }
}