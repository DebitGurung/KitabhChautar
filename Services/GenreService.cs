using kitabhChauta.DbContext;
using KitabhChauta.Interface;
using KitabhChauta.Model;
using Microsoft.EntityFrameworkCore;

namespace KitabhChauta.Services
{
    public class GenreService : IGenreService
    {
        private readonly KitabhChautariDbContext _context;

        public GenreService(KitabhChautariDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Genre>> GetAllGenresAsync()
        {
            return await _context.Genres.ToListAsync();
        }
        public async Task<Genre> CreateGenreAsync(Genre genre)
        {
            // Ensure ID isn't set (let database generate it)
            genre.Genre_id = 0;

            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();
            return genre;
        }

        public async Task UpdateGenreAsync(Genre genre)
        {
            _context.Entry(genre).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<Genre?> GetGenreByIdAsync(int id)
        {
            return await _context.Genres.FindAsync(id);
        }

        public async Task DeleteGenreAsync(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre != null)
            {
                _context.Genres.Remove(genre);
                await _context.SaveChangesAsync();
            }
        }
    }
   
}
