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

    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<IEnumerable<BookDto>> GetAllBooksAsync()
        {
            var books = await _bookRepository.GetAllAsync();
            return books.Select(MapToDto);
        }

        public async Task<BookDto?> GetBookByIdAsync(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            return book != null ? MapToDto(book) : null;
        }

        public async Task<BookDto> CreateBookAsync(CreateBookDto createBookDto)
        {
            var book = new Book
            {
                Title = createBookDto.Title,
                Description = createBookDto.Description,
                PageCount = createBookDto.PageCount,
                Excerpt = createBookDto.Excerpt,
                PublishDate = createBookDto.PublishDate
            };

            var createdBook = await _bookRepository.CreateAsync(book);
            return MapToDto(createdBook);
        }

        public async Task<BookDto> UpdateBookAsync(int id, UpdateBookDto updateBookDto)
        {
            var book = new Book
            {
                Id = id,
                Title = updateBookDto.Title,
                Description = updateBookDto.Description,
                PageCount = updateBookDto.PageCount,
                Excerpt = updateBookDto.Excerpt,
                PublishDate = updateBookDto.PublishDate
            };

            var updatedBook = await _bookRepository.UpdateAsync(id, book);
            return MapToDto(updatedBook);
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            return await _bookRepository.DeleteAsync(id);
        }

        private static BookDto MapToDto(Book book)
        {
            return new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                PageCount = book.PageCount,
                Excerpt = book.Excerpt,
                PublishDate = book.PublishDate
            };
        }
    }
}
