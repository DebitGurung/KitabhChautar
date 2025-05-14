using kitabhChauta.DbContext;
using KitabhChauta.Interface;
using KitabhChauta.Model;
using Microsoft.EntityFrameworkCore;

namespace KitabhChauta.Services
{
    public class PublisherService : IPublisherService
    {

        private readonly KitabhChautariDbContext _context;

        public PublisherService(KitabhChautariDbContext context)
        {
            _context = context;
        }   

        public async Task<IEnumerable<Publisher>> GetAllPublishersAsync()
        {
            return await _context.Publishers.ToListAsync();
        }

        public async Task<Publisher?> GetPublisherByIdAsync(int id)
        {
            return await _context.Publishers.FindAsync(id);
        }

        public async Task<Publisher> CreatePublisherAsync(Publisher publisher)
        {
            // Ensure ID isn't set (let database generate it)
            publisher.Publisher_id = 0;
            _context.Publishers.Add(publisher);
            await _context.SaveChangesAsync();
            return publisher;
        }

        public async Task UpdatePublisherAsync(Publisher publisher)
        {
            _context.Entry(publisher).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeletePublisherAsync(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);
            if (publisher != null)
            {
                _context.Publishers.Remove(publisher);
                await _context.SaveChangesAsync();
            }
        }


    }
}
