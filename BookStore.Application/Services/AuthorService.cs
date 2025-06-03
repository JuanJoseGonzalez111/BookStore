using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Services
{
    using BookStore.Application.DTOs;
    using BookStore.Application.Interfaces;
    using BookStore.Domain.Entities;
    using BookStore.Domain.Interfaces;

    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<IEnumerable<AuthorDto>> GetAllAuthorsAsync()
        {
            var authors = await _authorRepository.GetAllAsync();
            return authors.Select(MapToDto);
        }

        public async Task<AuthorDto?> GetAuthorByIdAsync(int id)
        {
            var author = await _authorRepository.GetByIdAsync(id);
            return author != null ? MapToDto(author) : null;
        }

        public async Task<AuthorDto> CreateAuthorAsync(CreateAuthorDto createAuthorDto)
        {
            var author = new Author
            {
                IdBook = createAuthorDto.IdBook,
                FirstName = createAuthorDto.FirstName,
                LastName = createAuthorDto.LastName
            };

            var createdAuthor = await _authorRepository.CreateAsync(author);
            return MapToDto(createdAuthor);
        }

        public async Task<AuthorDto> UpdateAuthorAsync(int id, UpdateAuthorDto updateAuthorDto)
        {
            var author = new Author
            {
                Id = id,
                IdBook = updateAuthorDto.IdBook,
                FirstName = updateAuthorDto.FirstName,
                LastName = updateAuthorDto.LastName
            };

            var updatedAuthor = await _authorRepository.UpdateAsync(id, author);
            return MapToDto(updatedAuthor);
        }

        public async Task<bool> DeleteAuthorAsync(int id)
        {
            return await _authorRepository.DeleteAsync(id);
        }

        private static AuthorDto MapToDto(Author author)
        {
            return new AuthorDto
            {
                Id = author.Id,
                IdBook = author.IdBook,
                FirstName = author.FirstName,
                LastName = author.LastName
            };
        }
    }
}
