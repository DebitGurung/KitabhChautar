// UserService.cs
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KitabhChautari.Services
{
    public class UserService : IUserService // Inherit from IUserService
    {
        private readonly KitabhChautariDbContext _context;

        public UserService(KitabhChautariDbContext context)
        {
            _context = context;
        }

        // 1. Implement GetAllUsersAsync
        public async Task<List<User>> GetAllUsersAsync()
            => await _context.Users.ToListAsync();

        // 2. Implement GetUserByIdAsync
        public async Task<User?> GetUserByIdAsync(int id)
            => await _context.Users.FindAsync(id);

        // 3. Implement CreateUserAsync (with UserDto)
        public async Task<User> CreateUserAsync(UserDto userDto)
        {
            ArgumentNullException.ThrowIfNull(userDto);

            if (await _context.Users.AnyAsync(u => u.Email == userDto.Email))
                throw new InvalidOperationException("Email already exists");

            var user = new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        // 4. Implement UpdateUserAsync (with UserDto)
        public async Task<bool> UpdateUserAsync(int id, UserDto userDto)
        {
            if (userDto == null || id != userDto.UserId)
                return false;

            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
                return false;

            existingUser.FirstName = userDto.FirstName;
            existingUser.LastName = userDto.LastName;
            existingUser.Email = userDto.Email;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        // 5. Implement DeleteUserAsync
        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        // 6. Implement UserExistsAsync
        public async Task<bool> UserExistsAsync(int id)
            => await _context.Users.AnyAsync(e => e.UserId == id);
    }
}