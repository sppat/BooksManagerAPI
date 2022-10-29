using BooksManagerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BooksManagerAPI.Data
{
    public class BookManagerDbContext : DbContext
    {
        public BookManagerDbContext(DbContextOptions<BookManagerDbContext> options)
            : base(options) { }

        public DbSet<Author>? Authors { get; set; }
        public DbSet<Category>? Categories { get; set; }
        public DbSet<Book>? Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>()
                .HasMany<Book>(a => a.Books)
                .WithOne(b => b.Author)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<Category>()
                .HasMany<Book>(c => c.Books)
                .WithOne(b => b.Category)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<Book>()
                .Property("PublicationDate")
                .HasColumnType("timestamp without time zone");

            modelBuilder.Seed();
        }
    }
}
