using BooksManagerAPI.Models.Dtos.AuthorDtos;
using BooksManagerAPI.Models.Dtos.CategoryDtos;

namespace BooksManagerAPI.Models.Dtos.BookDtos
{
    public class GetBookDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string PublicationDate { get; set; } = string.Empty;
        public int Pages { get; set; }
        public GetCategoryDto? Category { get; set; }
        public GetAuthorDto? Author { get; set; }
    }
}
