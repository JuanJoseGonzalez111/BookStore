using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Interfaces
{
    using BookStore.Application.DTOs;

    public interface IAuthorService
    {
        Task<IEnumerable<AuthorDto>> GetAllAuthorsAsync();
        Task<AuthorDto?> GetAuthorByIdAsync(int id);
        Task<AuthorDto> CreateAuthorAsync(CreateAuthorDto createAuthorDto);
        Task<AuthorDto> UpdateAuthorAsync(int id, UpdateAuthorDto updateAuthorDto);
        Task<bool> DeleteAuthorAsync(int id);
    }
}
