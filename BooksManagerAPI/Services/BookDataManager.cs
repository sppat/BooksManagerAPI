using BooksManagerAPI.Interfaces.RepositoryInterfaces;
using BooksManagerAPI.Models.Dtos.BookDtos;

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

        public async Task<ICollection<GetBookDto>> GetAllBooksAsync() => _bookMapper.MapBooksToGetDto(await _bookRepository.GetAllBooksAsync());

        public async Task<GetBookDto> GetBookByIdAsync(int id) => _bookMapper.MapBookToGetDto(await _bookRepository.GetByIdAsync(id));

        public async Task<ICollection<GetBookDto>> GetBooksByTitleSearchAsync(string searchString)
            => _bookMapper.MapBooksToGetDto(await _bookRepository.SearchByTitleAsync(searchString));

        public async Task AddAsync(PostBookDto postBookDto) => await _bookRepository.AddAsync(postBookDto);

        public async Task DeleteAsync(int id) => await _bookRepository.DeleteAsync(id);

        public async Task UpdateAsync(PutBookDto putBookDto) => await _bookRepository.UpdateAsync(putBookDto);

        public async Task<bool> BookExistsAsync(int id) => await _bookRepository.ExistsAsync(id);
    }
}
