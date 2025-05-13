using KitabhChauta.Interface;
using KitabhChauta.Model;
using KitabhChauta.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KitabhChauta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenresController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenreDTO>>> GetGenres()
        {
            var genres = await _genreService.GetAllGenresAsync();
            var genreDtos = genres.Select(g => new GenreDTO
            {
                Genre_id = g.Genre_id,
                Genre_Name = g.Genre_Name
            }).ToList();
            return Ok(genreDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GenreDTO>> GetGenre(int id)
        {
            var genre = await _genreService.GetGenreByIdAsync(id);
            if (genre == null)
            {
                return NotFound();
            }
            var genreDto = new GenreDTO
            {
                Genre_id = genre.Genre_id,
                Genre_Name = genre.Genre_Name
            };
            return Ok(genreDto);
        }

        [HttpPost]
        public async Task<ActionResult<GenreDTO>> PostGenre(GenreDTO genreDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var genre = new Genre
            {
                Genre_Name = genreDto.Genre_Name
            };

            var createdGenre = await _genreService.CreateGenreAsync(genre);
            var createdGenreDto = new GenreDTO
            {
                Genre_id = createdGenre.Genre_id,
                Genre_Name = createdGenre.Genre_Name
            };

            return CreatedAtAction(nameof(GetGenre), new { id = createdGenreDto.Genre_id }, createdGenreDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutGenre(int id, GenreDTO genreDto)
        {
            if (id != genreDto.Genre_id)
            {
                return BadRequest("Genre ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var genre = new Genre
            {
                Genre_id = genreDto.Genre_id,
                Genre_Name = genreDto.Genre_Name
            };

            try
            {
                await _genreService.UpdateGenreAsync(genre);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await GenreExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            var genre = await _genreService.GetGenreByIdAsync(id);
            if (genre == null)
            {
                return NotFound();
            }
            await _genreService.DeleteGenreAsync(id);
            return NoContent();
        }

        private async Task<bool> GenreExists(int id)
        {
            return await _genreService.GetGenreByIdAsync(id) != null;
        }
    }
}