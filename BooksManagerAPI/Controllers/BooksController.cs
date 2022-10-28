using BooksManagerAPI.Models.Dtos.AuthorDtos;
using BooksManagerAPI.Models.Dtos.BookDtos;
using BooksManagerAPI.Models.Dtos.CategoryDtos;
using BooksManagerAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Npgsql;
using System.Data;

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
        public IActionResult GetAll() => Ok(JsonConvert.SerializeObject(_bookManager.GetAllBooks(), Formatting.Indented));

        [HttpGet("{id}")]
        public IActionResult GetById(int id) => Ok(JsonConvert.SerializeObject(_bookManager.GetBookById(id), Formatting.Indented));

        [HttpPost]
        public IActionResult Add(PostBookDto postBookDto)
        {
            _bookManager.Add(postBookDto);
            return Ok("Book added successfully");
        }
        
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _bookManager.Delete(id);
            return Ok("Book deleted successfully");
        }
        
        [HttpPut]
        public IActionResult Update(PutBookDto putBookDto)
        {
            _bookManager.Update(putBookDto);
            return Ok("Book updated successfully");
        }
    }
}
