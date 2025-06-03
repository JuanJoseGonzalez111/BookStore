using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using BookStore.Application.DTOs;
    using BookStore.Application.Interfaces;

    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors()
        {
            var authors = await _authorService.GetAllAuthorsAsync();
            return Ok(authors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDto>> GetAuthor(int id)
        {
            var author = await _authorService.GetAuthorByIdAsync(id);

            if (author == null)
                return NotFound($"Author with ID {id} not found");

            return Ok(author);
        }

        [HttpPost]
        public async Task<ActionResult<AuthorDto>> CreateAuthor(CreateAuthorDto createAuthorDto)
        {
            var author = await _authorService.CreateAuthorAsync(createAuthorDto);
            return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AuthorDto>> UpdateAuthor(int id, UpdateAuthorDto updateAuthorDto)
        {
            var author = await _authorService.UpdateAuthorAsync(id, updateAuthorDto);
            return Ok(author);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAuthor(int id)
        {
            var result = await _authorService.DeleteAuthorAsync(id);

            if (!result)
                return NotFound($"Author with ID {id} not found");

            return NoContent();
        }
    }
}
