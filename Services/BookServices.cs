using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using KitabhChautari;
using KitabhChautari.Dto;
using KitabhChautari.Dtos;

namespace KitabhChautari.Services
{
    public class BookService : IBookService
    {
        private readonly KitabhChautariDbContext _context;

        public BookService(KitabhChautariDbContext context)
        {
            _context = context;
        }

        public async Task<List<BookDto>> GetAllBooksAsync()
        {
            return await _context.Books
                .Select(b => new BookDto
                {
                    BookId = b.BookId,
                    Title = b.Title,
                    Author = b.Author,
                    Genre = b.Genre,
                    ISBN = b.ISBN,
                    Price = b.Price,
                    PublishedDate = b.PublishedDate,
                    Pages = b.Pages,
                    StockCount = b.StockCount,
                    Synopsis = b.Synopsis,
                    CoverImageUrl = b.CoverImageUrl,
                    AdminId = b.AdminId,
                    MemberId = b.MemberId,
                    StaffId = b.StaffId
                })
                .ToListAsync();
        }

        public async Task<BookDto?> GetBookByIdAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return null;
            }

            return new BookDto
            {
                BookId = book.BookId,
                Title = book.Title,
                Author = book.Author,
                Genre = book.Genre,
                ISBN = book.ISBN,
                Price = book.Price,
                PublishedDate = book.PublishedDate,
                Pages = book.Pages,
                StockCount = book.StockCount,
                Synopsis = book.Synopsis,
                CoverImageUrl = book.CoverImageUrl,
                AdminId = book.AdminId,
                MemberId = book.MemberId,
                StaffId = book.StaffId
            };
        }

        public async Task<BookDto> CreateBookAsync(BookDto bookDto)
        {
            if (bookDto == null)
            {
                throw new ArgumentNullException(nameof(bookDto));
            }

            var book = new Book
            {
                BookId = bookDto.BookId,
                Title = bookDto.Title,
                Author = bookDto.Author,
                Genre = bookDto.Genre,
                ISBN = bookDto.ISBN,
                Price = bookDto.Price,
                PublishedDate = bookDto.PublishedDate,
                Pages = bookDto.Pages,
                StockCount = bookDto.StockCount,
                Synopsis = bookDto.Synopsis,
                CoverImageUrl = bookDto.CoverImageUrl,
                AdminId = bookDto.AdminId,
                MemberId = bookDto.MemberId,
                StaffId = bookDto.StaffId
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            bookDto.BookId = book.BookId; // Update DTO with generated ID
            return bookDto;
        }

        public async Task<bool> UpdateBookAsync(int id, BookDto bookDto)
        {
            if (bookDto == null || id != bookDto.BookId)
            {
                return false;
            }

            var existingBook = await _context.Books.FindAsync(id);
            if (existingBook == null)
            {
                return false;
            }

            existingBook.Title = bookDto.Title;
            existingBook.Author = bookDto.Author;
            existingBook.Genre = bookDto.Genre;
            existingBook.ISBN = bookDto.ISBN;
            existingBook.Price = bookDto.Price;
            existingBook.PublishedDate = bookDto.PublishedDate;
            existingBook.Pages = bookDto.Pages;
            existingBook.StockCount = bookDto.StockCount;
            existingBook.Synopsis = bookDto.Synopsis;
            existingBook.CoverImageUrl = bookDto.CoverImageUrl;
            existingBook.AdminId = bookDto.AdminId;
            existingBook.MemberId = bookDto.MemberId;
            existingBook.StaffId = bookDto.StaffId;

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

        public async Task<bool> DeleteBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return false;
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> BookExistsAsync(int id)
        {
            return await _context.Books.AnyAsync(b => b.BookId == id);
        }
    }
}