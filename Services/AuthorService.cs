using kitabhChauta.DbContext;
using KitabhChauta.Interfaces;
using KitabhChauta.Model;
using Microsoft.EntityFrameworkCore;

namespace KitabhChauta.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly KitabhChautariDbContext _context;

        public AuthorService(KitabhChautariDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Author>> GetAllAuthorsAsync()
        {
            return await _context.Authors.ToListAsync();
        }

        public async Task<Author?> GetAuthorByIdAsync(int id)
        {
            return await _context.Authors.FindAsync(id);
        }

        public async Task<Author> CreateAuthorAsync(Author author)
        {
            // Ensure ID isn't set (let database generate it)
            author.Author_id = 0;

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
            return author;
        }

        public async Task UpdateAuthorAsync(Author author)
        {
            _context.Entry(author).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAuthorAsync(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author != null)
            {
                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();
            }
        }
    }
}