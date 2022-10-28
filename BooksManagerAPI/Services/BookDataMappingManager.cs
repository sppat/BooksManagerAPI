using BooksManagerAPI.Models.Dtos.AuthorDtos;
using BooksManagerAPI.Models.Dtos.BookDtos;
using BooksManagerAPI.Models.Dtos.CategoryDtos;
using BooksManagerAPI.RepositoryContracts;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;

namespace BooksManagerAPI.Services
{
    public class BookDataMappingManager
    {

        public ICollection<GetBookDto> MapBooksToGetDto(DataTable booksTable)
        {
            List<GetBookDto> getBookDtos = new List<GetBookDto>();

            foreach (DataRow book in booksTable.Rows)
            {
                getBookDtos.Add(new GetBookDto
                {
                    Id = Convert.ToInt32(book["Id"]),
                    Title = book["Title"].ToString() ?? string.Empty,
                    PublicationDate = Convert.ToDateTime(book["PublicationDate"]).ToString("dd-MM-yyyy"),
                    Pages = Convert.ToInt32(book["Pages"]),
                    Category = new GetCategoryDto
                    {
                        Id = Convert.ToInt32(book["CategoryId"]),
                        Name = book["CategoryName"].ToString() ?? string.Empty,
                    },
                    Author = new GetAuthorDto
                    {
                        Id = Convert.ToInt32(book["AuthorId"]),
                        Name = book["AuthorName"].ToString() ?? string.Empty,
                        LastName = book["AuthorLastName"].ToString() ?? string.Empty,
                    }
                });
            }

            return getBookDtos;
        }

        public GetBookDto MapBookToGetDto(DataTable booksTable)
        {
            GetBookDto getBookDto = new();

            foreach (DataRow book in booksTable.Rows)
            {
                getBookDto.Id = Convert.ToInt32(book["Id"]);
                getBookDto.Title = book["Title"].ToString() ?? string.Empty;
                getBookDto.PublicationDate = Convert.ToDateTime(book["PublicationDate"]).ToString("dd-MM-yyyy");
                getBookDto.Pages = Convert.ToInt32(book["Pages"]);
                getBookDto.Category = new GetCategoryDto
                {
                    Id = Convert.ToInt32(book["CategoryId"]),
                    Name = book["CategoryName"].ToString() ?? string.Empty,
                };
                getBookDto.Author = new GetAuthorDto
                {
                    Id = Convert.ToInt32(book["AuthorId"]),
                    Name = book["AuthorName"].ToString() ?? string.Empty,
                    LastName = book["AuthorLastName"].ToString() ?? string.Empty,
                };
            }

            return getBookDto;
        }
    }
}
