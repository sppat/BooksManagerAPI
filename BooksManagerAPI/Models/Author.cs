namespace BooksManagerAPI.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public ICollection<Book>? Books { get; set; }
    }
}
