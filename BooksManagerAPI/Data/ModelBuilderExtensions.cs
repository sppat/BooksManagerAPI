using BooksManagerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BooksManagerAPI.Data
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>().HasData(
                new Author
                {
                    Id = 1,
                    Name = "John",
                    LastName = "Doe"
                },
                new Author
                {
                    Id = 2,
                    Name = "DummyName",
                    LastName = "DummyLastName"
                }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = 1,
                    Name = "Category 1"
                },
                new Category
                {
                    Id = 2,
                    Name = "Category 2"
                }
            );

            modelBuilder.Entity<Book>().HasData(
                new Book
                {
                    Id = 1,
                    Title = "Book 1",
                    Pages = 100,
                    CategoryId = 1,
                    AuthorId = 1
                },
                new Book
                {
                    Id = 2,
                    Title = "Book 2",
                    Pages = 200,
                    CategoryId = 2,
                    AuthorId = 2
                }
            );
        }
    }
}
