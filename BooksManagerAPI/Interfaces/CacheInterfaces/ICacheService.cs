namespace BooksManagerAPI.Interfaces.CacheInterfaces
{
    public interface ICacheService
    {
        Task CacheResponseAsync(string key, object response, TimeSpan timeLive);
        Task<string> GetCachedResponseAsync(string key);
        Task RemoveCachedAsync(string key);
    }
}
