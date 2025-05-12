using System.Collections.Generic;
using System.Threading.Tasks;
using KitabhChautari.Dto;
using KitabhChautari.Dtos;

namespace KitabhChautari.Services {
    public interface IBookService {
        Task<List<BookDto>> GetAllBooksAsync(); 
        Task<BookDto?> GetBookByIdAsync(int id);
        Task<BookDto> CreateBookAsync(BookDto bookDto);
        Task<bool> UpdateBookAsync(int id, BookDto bookDto);
        Task<bool> DeleteBookAsync(int id);
        Task<bool> BookExistsAsync(int id);
    }
}