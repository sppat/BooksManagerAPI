using BooksManagerAPI.Models.Dtos.AuthorDtos;
using BooksManagerAPI.Models.Dtos.BookDtos;
using BooksManagerAPI.Models.Dtos.CategoryDtos;
using BooksManagerAPI.RepositoryContracts;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Npgsql;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BooksManagerAPI.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly IConfiguration _config;

        public BookRepository(IConfiguration config)
        {
            _config = config;
        }

        public void Add(PostBookDto postBookDto)
        {
            string query = @"
                insert into ""Books"" (""Title"", ""PublicationDate"", ""Pages"", ""CategoryId"", ""AuthorId"") 
                values (@Title, @PublicationDate, @Pages, @CategoryId, @AuthorId)
            ";

            DataTable table = new DataTable();
            NpgsqlDataReader reader;

            using (NpgsqlConnection connection = new NpgsqlConnection(_config.GetConnectionString("Default")))
            {
                connection.Open();

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Title", postBookDto.Title);
                    command.Parameters.AddWithValue("@PublicationDate", Convert.ToDateTime(postBookDto.PublicationDate));
                    command.Parameters.AddWithValue("@Pages", postBookDto.Pages);
                    command.Parameters.AddWithValue("@CategoryId", postBookDto.CategoryId);
                    command.Parameters.AddWithValue("@AuthorId", postBookDto.AuthorId);
                    reader = command.ExecuteReader();

                    reader.Close();
                    connection.Close();
                }
            }
        }

        public void Delete(int id)
        {
            string query = @"
                delete from ""Books""
                where ""Books"".""Id"" = @id
            ";

            NpgsqlDataReader reader;

            using (NpgsqlConnection connection = new NpgsqlConnection(_config.GetConnectionString("Default")))
            {
                connection.Open();

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    reader = command.ExecuteReader();

                    reader.Close();
                    connection.Close();
                }
            }
        }

        public DataTable GetAllBooks()
        {
            string query = @"
                select 
                    ""Books"".""Id"",
                    ""Books"".""Title"",
                    ""Books"".""PublicationDate"",
                    ""Books"".""Pages"",
                    ""Books"".""CategoryId"",
                    ""Books"".""AuthorId"",
                    ""Categories"".""Name"" as ""CategoryName"",
                    ""Authors"".""Name"" as ""AuthorName"",
                    ""Authors"".""LastName"" as ""AuthorLastName""
                from ""Books""
                inner join ""Categories""
                on ""Categories"".""Id"" = ""Books"".""CategoryId""
                inner join ""Authors""
                on ""Authors"".""Id"" = ""Books"".""AuthorId""
            ";

            DataTable table = new DataTable();
            NpgsqlDataReader reader;

            using (NpgsqlConnection connection = new NpgsqlConnection(_config.GetConnectionString("Default")))
            {
                connection.Open();

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    reader = command.ExecuteReader();
                    table.Load(reader);

                    reader.Close();
                    connection.Close();
                }
            }

            return table;
        }

        public DataTable GetById(int id)
        {
            string query = @"
                select 
                    ""Books"".""Id"",
                    ""Books"".""Title"",
                    ""Books"".""PublicationDate"",
                    ""Books"".""Pages"",
                    ""Books"".""CategoryId"",
                    ""Books"".""AuthorId"",
                    ""Categories"".""Name"" as ""CategoryName"",
                    ""Authors"".""Name"" as ""AuthorName"",
                    ""Authors"".""LastName"" as ""AuthorLastName""
                from ""Books""
                inner join ""Categories""
                on ""Categories"".""Id"" = ""Books"".""CategoryId""
                inner join ""Authors""
                on ""Authors"".""Id"" = ""Books"".""AuthorId""
                where ""Books"".""Id"" = @id
            ";

            DataTable table = new DataTable();
            NpgsqlDataReader reader;

            using (NpgsqlConnection connection = new NpgsqlConnection(_config.GetConnectionString("Default")))
            {
                connection.Open();

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    reader = command.ExecuteReader();
                    table.Load(reader);

                    reader.Close();
                    connection.Close();
                }
            }

            return table;
        }

        public void Update(PutBookDto putBookDto)
        {
            string buildSet = "set ";
            foreach (var property in putBookDto.GetType().GetProperties())
            {
                var value = property.GetValue(putBookDto);
                if (value is not null && property.Name != "Id")
                {
                    buildSet += $"\"{property.Name}\" = @{property.Name}, ";
                }
            }

            string query = @"update ""Books"" " +
                buildSet.TrimEnd().TrimEnd(',') +
                @" where ""Books"".""Id"" = @Id";

            DataTable table = new DataTable();
            NpgsqlDataReader reader;

            using (NpgsqlConnection connection = new NpgsqlConnection(_config.GetConnectionString("Default")))
            {
                connection.Open();

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    foreach (var property in putBookDto.GetType().GetProperties())
                    {
                        var value = property.GetValue(putBookDto);
                        if (value is not null)
                        {
                            command.Parameters.AddWithValue($"@{property.Name}", value);
                        }
                    }

                    reader = command.ExecuteReader();

                    reader.Close();
                    connection.Close();
                }
            }
        }
    }
}
