using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Infrastructure.Repositories
{
    using System.Text;
    using System.Text.Json;
    using BookStore.Domain.Entities;
    using BookStore.Domain.Interfaces;

    public class BookRepository : IBookRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://fakerestapi.azurewebsites.net/api/v1/Books";

        public BookRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(_baseUrl);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var books = JsonSerializer.Deserialize<List<Book>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return books ?? new List<Book>();
            }
            catch (Exception ex)
            {
                // Log error
                Console.WriteLine($"Error getting books: {ex.Message}");
                return new List<Book>();
            }
        }

        public async Task<Book?> GetByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/{id}");

                if (!response.IsSuccessStatusCode)
                    return null;

                var content = await response.Content.ReadAsStringAsync();
                var book = JsonSerializer.Deserialize<Book>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return book;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting book {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<Book> CreateAsync(Book book)
        {
            try
            {
                var json = JsonSerializer.Serialize(book);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(_baseUrl, content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var createdBook = JsonSerializer.Deserialize<Book>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return createdBook ?? book;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating book: {ex.Message}");
                return book;
            }
        }

        public async Task<Book> UpdateAsync(int id, Book book)
        {
            try
            {
                var json = JsonSerializer.Serialize(book);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"{_baseUrl}/{id}", content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var updatedBook = JsonSerializer.Deserialize<Book>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return updatedBook ?? book;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating book {id}: {ex.Message}");
                return book;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_baseUrl}/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting book {id}: {ex.Message}");
                return false;
            }
        }
    }
}
