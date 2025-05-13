using KitabhChauta.Interfaces;
using KitabhChauta.Model;
using KitabhChauta.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KitabhChauta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDTO>>> GetAuthors()
        {
            var authors = await _authorService.GetAllAuthorsAsync();
            var authorDtos = authors.Select(a => new AuthorDTO
            {
                Author_id = a.Author_id,
                Author_Name = a.Author_Name
            }).ToList();
            return Ok(authorDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDTO>> GetAuthor(int id)
        {
            var author = await _authorService.GetAuthorByIdAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            var authorDto = new AuthorDTO
            {
                Author_id = author.Author_id,
                Author_Name = author.Author_Name
            };
            return Ok(authorDto);
        }

        [HttpPost]
        public async Task<ActionResult<AuthorDTO>> PostAuthor(AuthorDTO authorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var author = new Author
            {
                Author_Name = authorDto.Author_Name
            };

            var createdAuthor = await _authorService.CreateAuthorAsync(author);
            var createdAuthorDto = new AuthorDTO
            {
                Author_id = createdAuthor.Author_id,
                Author_Name = createdAuthor.Author_Name
            };

            return CreatedAtAction(nameof(GetAuthor), new { id = createdAuthorDto.Author_id }, createdAuthorDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, AuthorDTO authorDto)
        {
            if (id != authorDto.Author_id)
            {
                return BadRequest("Author ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var author = new Author
            {
                Author_id = authorDto.Author_id,
                Author_Name = authorDto.Author_Name
            };

            try
            {
                await _authorService.UpdateAuthorAsync(author);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _authorService.GetAuthorByIdAsync(id) == null)
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _authorService.GetAuthorByIdAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            await _authorService.DeleteAuthorAsync(id);
            return NoContent();
        }
    }
}