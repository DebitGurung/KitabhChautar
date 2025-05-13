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
    public class PublishersController : ControllerBase
    {
        private readonly IPublisherService _publisherService;

        public PublishersController(IPublisherService publisherService)
        {
            _publisherService = publisherService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PublisherDTO>>> GetPublishers()
        {
            var publishers = await _publisherService.GetAllPublishersAsync();
            var publisherDtos = publishers.Select(p => new PublisherDTO
            {
                Publisher_id = p.Publisher_id,
                Publisher_Name = p.Publisher_Name
            }).ToList();
            return Ok(publisherDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PublisherDTO>> GetPublisher(int id)
        {
            var publisher = await _publisherService.GetPublisherByIdAsync(id);
            if (publisher == null)
            {
                return NotFound();
            }
            var publisherDto = new PublisherDTO
            {
                Publisher_id = publisher.Publisher_id,
                Publisher_Name = publisher.Publisher_Name
            };
            return Ok(publisherDto);
        }

        [HttpPost]
        public async Task<ActionResult<PublisherDTO>> PostPublisher(PublisherDTO publisherDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var publisher = new Publisher
            {
                Publisher_Name = publisherDto.Publisher_Name
            };

            var createdPublisher = await _publisherService.CreatePublisherAsync(publisher);
            var createdPublisherDto = new PublisherDTO
            {
                Publisher_id = createdPublisher.Publisher_id,
                Publisher_Name = createdPublisher.Publisher_Name
            };

            return CreatedAtAction(nameof(GetPublisher), new { id = createdPublisherDto.Publisher_id }, createdPublisherDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPublisher(int id, PublisherDTO publisherDto)
        {
            if (id != publisherDto.Publisher_id)
            {
                return BadRequest("Publisher ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var publisher = new Publisher
            {
                Publisher_id = publisherDto.Publisher_id,
                Publisher_Name = publisherDto.Publisher_Name
            };

            try
            {
                await _publisherService.UpdatePublisherAsync(publisher);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _publisherService.GetPublisherByIdAsync(id) == null)
                {
                    return NotFound();
                }
                throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePublisher(int id)
        {
            var publisher = await _publisherService.GetPublisherByIdAsync(id);
            if (publisher == null)
            {
                return NotFound();
            }
            await _publisherService.DeletePublisherAsync(id);
            return NoContent();
        }
    }
}