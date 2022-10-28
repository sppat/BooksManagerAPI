using System.ComponentModel.DataAnnotations;

namespace BooksManagerAPI.Models.Dtos.BookDtos
{
    public class PostBookDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public DateTime PublicationDate { get; set; } = DateTime.Now;
        [Required]
        public int Pages { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int AuthorId { get; set; }
    }
}
