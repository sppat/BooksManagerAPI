namespace BooksManagerAPI.Models.Dtos.AuthorDtos
{
    public class GetAuthorDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
