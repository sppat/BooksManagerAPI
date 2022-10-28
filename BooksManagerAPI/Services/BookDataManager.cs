using BooksManagerAPI.Models.Dtos.BookDtos;
using BooksManagerAPI.RepositoryContracts;

namespace BooksManagerAPI.Services
{
    public class BookDataManager
    {
        private readonly IBookRepository _bookRepository;
        private readonly BookDataMappingManager _bookMapper;

        public BookDataManager(IBookRepository bookRepository, BookDataMappingManager bookMapper)
        {
            _bookRepository = bookRepository;
            _bookMapper = bookMapper;
        }

        public ICollection<GetBookDto> GetAllBooks() => _bookMapper.MapBooksToGetDto(_bookRepository.GetAllBooks());
        public ICollection<GetBookDto> GetBookById(int id) => _bookMapper.MapBooksToGetDto(_bookRepository.GetById(id));
        public void Add(PostBookDto postBookDto) => _bookRepository.Add(postBookDto);
        public void Delete(int id) => _bookRepository.Delete(id);
        public void Update(PutBookDto putBookDto) => _bookRepository.Update(putBookDto);
    }
}
