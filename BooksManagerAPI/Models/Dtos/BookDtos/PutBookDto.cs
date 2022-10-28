using System.ComponentModel.DataAnnotations;

namespace BooksManagerAPI.Models.Dtos.BookDtos
{
    public class PutBookDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public DateTime? PublicationDate { get; set; }
        public int? Pages { get; set; }
        public int? CategoryId { get; set; }
        public int? AuthorId { get; set; }
    }
}
