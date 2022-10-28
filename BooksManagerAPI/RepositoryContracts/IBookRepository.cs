using BooksManagerAPI.Models.Dtos.BookDtos;
using System.Data;

namespace BooksManagerAPI.RepositoryContracts
{
    public interface IBookRepository
    {
        void Add(PostBookDto postBookDto);
        void Delete(int id);
        DataTable GetAllBooks();
        DataTable GetById(int id);
        void Update(PutBookDto putBookDto);
    }
}
