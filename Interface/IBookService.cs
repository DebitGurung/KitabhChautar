﻿using KitabhChauta.Model;

namespace KitabhChauta.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<IEnumerable<Book>> GetFilteredBooksAsync(int? genreId, int? authorId, int? publisherId, string? category);
        Task<Book?> GetBookByIdAsync(int id);
        Task<Book> CreateBookAsync(Book book);
        Task UpdateBookAsync(Book book);
        Task DeleteBookAsync(int id);
        Task<bool> AuthorExistsAsync(int authorId);
        Task<bool> GenreExistsAsync(int genreId);
        Task<bool> PublisherExistsAsync(int publisherId);
    }
}