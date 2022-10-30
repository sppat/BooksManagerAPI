using BooksManagerAPI.Interfaces.CacheInterfaces;
using BooksManagerAPI.Interfaces.RepositoryInterfaces;
using BooksManagerAPI.Models.Dtos.BookDtos;
using Microsoft.Extensions.Caching.Distributed;
using System.Data;

namespace BooksManagerAPI.Services
{
    public class BookDataManager
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICacheService _cacheService;
        private readonly IConfiguration _configuration;
        private readonly BookDataMappingManager _bookMapper;
        private int _liveTime = 300;

        public string BaseUrl => _configuration["BaseUrl"];

        public BookDataManager(IBookRepository bookRepository, ICacheService cacheService, IConfiguration configuration, BookDataMappingManager bookMapper)
        {
            _bookRepository = bookRepository;
            _cacheService = cacheService;
            _configuration = configuration;
            _bookMapper = bookMapper;
        }

        public async Task<ICollection<GetBookDto>> GetAllBooksAsync()
        {
            ICollection<GetBookDto> getBookDtos = _bookMapper.MapBooksToGetDto(await _bookRepository.GetAllBooksAsync());
            
            await _cacheService.CacheResponseAsync(BaseUrl, getBookDtos, TimeSpan.FromSeconds(_liveTime));

            return getBookDtos;
        }


        public async Task<GetBookDto> GetBookByIdAsync(int id)
        {
            GetBookDto getBookDto = _bookMapper.MapBookToGetDto(await _bookRepository.GetByIdAsync(id));

            await _cacheService.CacheResponseAsync($"{BaseUrl}/{getBookDto.Id}", getBookDto, TimeSpan.FromSeconds(_liveTime));

            return getBookDto;
        }

        public async Task<ICollection<GetBookDto>> GetBooksByTitleSearchAsync(string searchString)
        {
            ICollection<GetBookDto> getBookDtos = _bookMapper.MapBooksToGetDto(await _bookRepository.SearchByTitleAsync(searchString.ToLower()));

            await _cacheService.CacheResponseAsync($"{BaseUrl}/search/{searchString}", getBookDtos, TimeSpan.FromSeconds(_liveTime));

            return getBookDtos;
        }

        public async Task AddAsync(PostBookDto postBookDto)
        {
            await _bookRepository.AddAsync(postBookDto);

            ICollection<GetBookDto> getBookDtos = await GetAllBooksAsync();

            await _cacheService.CacheResponseAsync(BaseUrl, getBookDtos, TimeSpan.FromSeconds(_liveTime));
        }

        public async Task DeleteAsync(int id)
        {
            await _bookRepository.DeleteAsync(id);
            await _cacheService.RemoveCachedAsync($"{BaseUrl}/{id}");

            ICollection<GetBookDto> getBookDtos = await GetAllBooksAsync();

            await _cacheService.CacheResponseAsync(BaseUrl, getBookDtos, TimeSpan.FromSeconds(_liveTime));
        }

        public async Task UpdateAsync(PutBookDto putBookDto)
        {
            DataTable table = await _bookRepository.UpdateAsync(putBookDto);
            GetBookDto getBookDto = _bookMapper.MapBookToGetDto(table);
            ICollection<GetBookDto> getBookDtos = await GetAllBooksAsync();

            await _cacheService.CacheResponseAsync($"{BaseUrl}/{getBookDto.Id}", getBookDto, TimeSpan.FromSeconds(_liveTime));
            await _cacheService.CacheResponseAsync(BaseUrl, getBookDtos, TimeSpan.FromSeconds(_liveTime));
        }

        public async Task<bool> BookExistsAsync(int id) => await _bookRepository.ExistsAsync(id);
    }
}
