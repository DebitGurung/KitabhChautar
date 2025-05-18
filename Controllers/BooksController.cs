using KitabhChauta.Interfaces;
using KitabhChauta.Model;
using KitabhChauta.Dto;
using kitabhChauta.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KitabhChauta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooks(
            [FromQuery] int? genreId = null,
            [FromQuery] int? authorId = null,
            [FromQuery] int? publisherId = null,
            [FromQuery] string? category = null)
        {
            var books = await _bookService.GetFilteredBooksAsync(genreId, authorId, publisherId, category);
            var bookDtos = books.Select(b => new BookDTO
            {
                BookId = b.BookId,
                Title = b.Title,
                ISBN = b.ISBN,
                Price = b.Price,
                PublishedDate = b.PublishedDate,
                Pages = b.Pages,
                StockCount = b.StockCount,
                Synopsis = b.Synopsis,
                CoverImageUrl = b.CoverImageUrl,
                IsOnSale = b.IsOnSale,
                DiscountPercentage = b.DiscountPercentage,
                Category = b.Category,
                Author_id = b.Author_id,
                Genre_id = b.Genre_id,
                Publisher_id = b.Publisher_id
            }).ToList();
            return Ok(bookDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDTO>> GetBook(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            var bookDto = new BookDTO
            {
                BookId = book.BookId,
                Title = book.Title,
                ISBN = book.ISBN,
                Price = book.Price,
                PublishedDate = book.PublishedDate,
                Pages = book.Pages,
                StockCount = book.StockCount,
                Synopsis = book.Synopsis,
                CoverImageUrl = book.CoverImageUrl,
                IsOnSale = book.IsOnSale,
                DiscountPercentage = book.DiscountPercentage,
                Category = book.Category,
                Author_id = book.Author_id,
                Genre_id = book.Genre_id,
                Publisher_id = book.Publisher_id
            };
            return Ok(bookDto);
        }

        [HttpPost]
        public async Task<ActionResult<BookDTO>> PostBook([FromBody] BookDTO bookDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validate foreign keys
            if (!await _bookService.AuthorExistsAsync(bookDto.Author_id))
            {
                return BadRequest("Invalid Author ID");
            }
            if (!await _bookService.GenreExistsAsync(bookDto.Genre_id))
            {
                return BadRequest("Invalid Genre ID");
            }
            if (!await _bookService.PublisherExistsAsync(bookDto.Publisher_id))
            {
                return BadRequest("Invalid Publisher ID");
            }

            // Validate DiscountPercentage
            if (bookDto.IsOnSale && !bookDto.DiscountPercentage.HasValue)
            {
                return BadRequest("DiscountPercentage is required when IsOnSale is true");
            }
            if (!bookDto.IsOnSale)
            {
                bookDto.DiscountPercentage = null;
            }

            var book = new Book
            {
                Title = bookDto.Title,
                ISBN = bookDto.ISBN,
                Price = bookDto.Price,
                PublishedDate = bookDto.PublishedDate,
                Pages = bookDto.Pages,
                StockCount = bookDto.StockCount,
                Synopsis = bookDto.Synopsis,
                CoverImageUrl = bookDto.CoverImageUrl,
                IsOnSale = bookDto.IsOnSale,
                DiscountPercentage = bookDto.DiscountPercentage,
                Category = bookDto.Category,
                Author_id = bookDto.Author_id,
                Genre_id = bookDto.Genre_id,
                Publisher_id = bookDto.Publisher_id
            };

            var createdBook = await _bookService.CreateBookAsync(book);
            var createdBookDto = new BookDTO
            {
                BookId = createdBook.BookId,
                Title = createdBook.Title,
                ISBN = createdBook.ISBN,
                Price = createdBook.Price,
                PublishedDate = createdBook.PublishedDate,
                Pages = createdBook.Pages,
                StockCount = createdBook.StockCount,
                Synopsis = createdBook.Synopsis,
                CoverImageUrl = createdBook.CoverImageUrl,
                IsOnSale = createdBook.IsOnSale,
                DiscountPercentage = createdBook.DiscountPercentage,
                Category = createdBook.Category,
                Author_id = createdBook.Author_id,
                Genre_id = createdBook.Genre_id,
                Publisher_id = createdBook.Publisher_id
            };

            return CreatedAtAction(nameof(GetBook), new { id = createdBookDto.BookId }, createdBookDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, [FromBody] BookDTO bookDto)
        {
            if (id != bookDto.BookId)
            {
                return BadRequest("Book ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validate foreign keys
            if (!await _bookService.AuthorExistsAsync(bookDto.Author_id))
            {
                return BadRequest("Invalid Author ID");
            }
            if (!await _bookService.GenreExistsAsync(bookDto.Genre_id))
            {
                return BadRequest("Invalid Genre ID");
            }
            if (!await _bookService.PublisherExistsAsync(bookDto.Publisher_id))
            {
                return BadRequest("Invalid Publisher ID");
            }

            // Validate DiscountPercentage
            if (bookDto.IsOnSale && !bookDto.DiscountPercentage.HasValue)
            {
                return BadRequest("DiscountPercentage is required when IsOnSale is true");
            }
            if (!bookDto.IsOnSale)
            {
                bookDto.DiscountPercentage = null;
            }

            var book = new Book
            {
                BookId = bookDto.BookId,
                Title = bookDto.Title,
                ISBN = bookDto.ISBN,
                Price = bookDto.Price,
                PublishedDate = bookDto.PublishedDate,
                Pages = bookDto.Pages,
                StockCount = bookDto.StockCount,
                Synopsis = bookDto.Synopsis,
                CoverImageUrl = bookDto.CoverImageUrl,
                IsOnSale = bookDto.IsOnSale,
                DiscountPercentage = bookDto.DiscountPercentage,
                Category = bookDto.Category,
                Author_id = bookDto.Author_id,
                Genre_id = bookDto.Genre_id,
                Publisher_id = bookDto.Publisher_id
            };

            try
            {
                await _bookService.UpdateBookAsync(book);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _bookService.GetBookByIdAsync(id) == null)
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            await _bookService.DeleteBookAsync(id);
            return NoContent();
        }
    }
}