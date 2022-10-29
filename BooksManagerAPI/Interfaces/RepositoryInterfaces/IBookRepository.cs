using BooksManagerAPI.Models.Dtos.BookDtos;
using System.Data;

namespace BooksManagerAPI.Interfaces.RepositoryInterfaces
{
    public interface IBookRepository
    {
        Task AddAsync(PostBookDto postBookDto);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<DataTable> GetAllBooksAsync();
        Task<DataTable> GetByIdAsync(int id);
        Task<DataTable> SearchByTitleAsync(string searchString);
        Task UpdateAsync(PutBookDto putBookDto);
    }
}
