using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BooksManagerAPI.Models
{
    public class Book
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public DateTime PublicationDate { get; set; } = DateTime.Now;
        [Required]
        public int Pages { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        [Required]
        public int AuthorId { get; set; }
        public Author? Author { get; set; }

    }
}
