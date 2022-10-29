﻿namespace BooksManagerAPI.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<Book>? Books { get; set; }
    }
}
