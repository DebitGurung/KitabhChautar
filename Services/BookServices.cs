using KitabhChauta.Interfaces;
using KitabhChauta.Model;
using Microsoft.EntityFrameworkCore;
using kitabhChauta.DbContext;
using System;

namespace KitabhChauta.Services
{
    public class BookService : IBookService
    {
        private readonly KitabhChautariDbContext _context;

        public BookService(KitabhChautariDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .Include(b => b.Publisher)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetFilteredBooksAsync(int? genreId, int? authorId, int? publisherId, string? category)
        {
            var query = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .Include(b => b.Publisher)
                .AsQueryable();

            if (genreId.HasValue)
            {
                query = query.Where(b => b.Genre_id == genreId.Value);
            }

            if (authorId.HasValue)
            {
                query = query.Where(b => b.Author_id == authorId.Value);
            }

            if (publisherId.HasValue)
            {
                query = query.Where(b => b.Publisher_id == publisherId.Value);
            }

            if (!string.IsNullOrEmpty(category) && System.Enum.TryParse<KitabhChauta.Enum.Category>(category, true, out var categoryEnum))
            {
                query = query.Where(b => b.Category == categoryEnum);
            }

            return await query.ToListAsync();
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .Include(b => b.Publisher)
                .FirstOrDefaultAsync(b => b.BookId == id);
        }

        public async Task<Book> CreateBookAsync(Book book)
        {
            // Ensure ID isn't set (let database generate it)
            book.BookId = 0;

            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task UpdateBookAsync(Book book)
        {
            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> AuthorExistsAsync(int authorId)
        {
            return await _context.Authors.AnyAsync(a => a.Author_id == authorId);
        }

        public async Task<bool> GenreExistsAsync(int genreId)
        {
            return await _context.Genres.AnyAsync(g => g.Genre_id == genreId);
        }

        public async Task<bool> PublisherExistsAsync(int publisherId)
        {
            return await _context.Publishers.AnyAsync(p => p.Publisher_id == publisherId);
        }
    }
}