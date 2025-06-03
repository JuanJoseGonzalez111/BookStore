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

    public class AuthorRepository : IAuthorRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://fakerestapi.azurewebsites.net/api/v1/Authors";

        public AuthorRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(_baseUrl);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var authors = JsonSerializer.Deserialize<List<Author>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return authors ?? new List<Author>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting authors: {ex.Message}");
                return new List<Author>();
            }
        }

        public async Task<Author?> GetByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/{id}");

                if (!response.IsSuccessStatusCode)
                    return null;

                var content = await response.Content.ReadAsStringAsync();
                var author = JsonSerializer.Deserialize<Author>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return author;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting author {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<Author> CreateAsync(Author author)
        {
            try
            {
                var json = JsonSerializer.Serialize(author);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(_baseUrl, content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var createdAuthor = JsonSerializer.Deserialize<Author>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return createdAuthor ?? author;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating author: {ex.Message}");
                return author;
            }
        }

        public async Task<Author> UpdateAsync(int id, Author author)
        {
            try
            {
                var json = JsonSerializer.Serialize(author);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"{_baseUrl}/{id}", content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var updatedAuthor = JsonSerializer.Deserialize<Author>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return updatedAuthor ?? author;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating author {id}: {ex.Message}");
                return author;
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
                Console.WriteLine($"Error deleting author {id}: {ex.Message}");
                return false;
            }
        }
    }
}
