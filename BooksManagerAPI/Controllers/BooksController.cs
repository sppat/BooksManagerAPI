using BooksManagerAPI.Attributes;
using BooksManagerAPI.Models.Dtos.BookDtos;
using BooksManagerAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BooksManagerAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookDataManager _bookManager;

        public BooksController(BookDataManager bookManager)
        {
            _bookManager = bookManager;
        }

        [HttpGet]
        [Cache(300)]
        public async Task<ActionResult<ICollection<GetBookDto>>> GetAll()
            => Ok(await _bookManager.GetAllBooksAsync());

        [HttpGet("{id}")]
        [Cache(300)]
        public async Task<ActionResult<GetBookDto>> GetById(int id)
        {
            if (!await _bookManager.BookExistsAsync(id))
            {
                return NotFound("The book you are looking for, does not exist.");
            }

            return Ok(await _bookManager.GetBookByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Add(PostBookDto postBookDto)
        {
            await _bookManager.AddAsync(postBookDto);

            return Ok("Book added successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _bookManager.BookExistsAsync(id))
            {
                return NotFound("The book you are trying to delete, does not exist.");
            }

            await _bookManager.DeleteAsync(id);

            return Ok("Book deleted successfully");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PutBookDto putBookDto)
        {
            if (id != putBookDto.Id)
            {
                return BadRequest("Url Id does not match with book Id");
            }

            if (!await _bookManager.BookExistsAsync(id))
            {
                return NotFound("The book you are trying to update, does not exist.");
            }

            await _bookManager.UpdateAsync(putBookDto);

            return Ok("Book updated successfully");
        }

        [HttpGet("search/{searchString}")]
        [Cache(300)]
        public async Task<ActionResult<ICollection<GetBookDto>>> SearchByTitle(string searchString)
            => Ok(await _bookManager.GetBooksByTitleSearchAsync(searchString));
    }
}
