using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

    public class StaffService : IStaffService
    {
        private readonly KitabhChautariDbContext _context;

        public StaffService(KitabhChautariDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Staff>> GetAllStaff()
            => await _context.Staffs.ToListAsync();

        public async Task<Staff> GetStaffById(int id)
            => await _context.Staffs.FindAsync(id)
               ?? throw new KeyNotFoundException("Staff not found");

        public async Task<Staff> CreateStaff(StaffDto dto)
        {
            if (await _context.Staffs.AnyAsync(s => s.Email == dto.Email))
                throw new InvalidOperationException("Email already exists");

            var staff = new Staff
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email
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
            await _context.SaveChangesAsync();
        }

        public async Task DeleteStaff(int id)
        {
            var staff = await _context.Staffs.FindAsync(id)
                ?? throw new KeyNotFoundException("Staff not found");

            _context.Staffs.Remove(staff);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> StaffExists(int id)
            => await _context.Staffs.AnyAsync(s => s.StaffId == id);
    }
