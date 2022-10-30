using BooksManagerAPI.Interfaces.RepositoryInterfaces;
using BooksManagerAPI.Models.Dtos.BookDtos;
using Npgsql;
using System.Data;

namespace BooksManagerAPI.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly IConfiguration _config;

        public BookRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task AddAsync(PostBookDto postBookDto)
        {
            string query = @"
                insert into ""Books"" (""Title"", ""PublicationDate"", ""Pages"", ""CategoryId"", ""AuthorId"") 
                values (@Title, @PublicationDate, @Pages, @CategoryId, @AuthorId)
            ";

            await DataManipulateAsync(query, postBookDto);
        }

        public async Task DeleteAsync(int id)
        {
            string query = @"
                delete from ""Books""
                where ""Books"".""Id"" = @Id
            ";

            await DataManipulateAsync(query, id: id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            string query = @"
                select *
                from ""Books""
                where ""Books"".""Id"" = @id
            ";

            var result = await DataQueryAsync(query, id: id);

            return result.Rows.Count != 0;
        }

        public async Task<DataTable> GetAllBooksAsync()
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

            return await DataQueryAsync(query);
        }

        public async Task<DataTable> GetByIdAsync(int id)
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

            return await DataQueryAsync(query, id: id);
        }

        public async Task<DataTable> SearchByTitleAsync(string searchString)
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
                where ""Books"".""Title"" like '%' || @title || '%'
            ";

            return await DataQueryAsync(query, title: searchString);
        }

        public async Task<DataTable> UpdateAsync(PutBookDto putBookDto)
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

            await DataManipulateAsync(query, dataObject: putBookDto);
            
            return await GetByIdAsync(putBookDto.Id);
        }

        private async Task<DataTable> DataQueryAsync(string query, int? id = null, string? title = null)
        {
            DataTable table = new DataTable();
            NpgsqlDataReader reader;

            await using (NpgsqlConnection connection = new NpgsqlConnection(_config.GetConnectionString("Default")))
            {
                await connection.OpenAsync();

                await using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    if (id is not null)
                    {
                        command.Parameters.AddWithValue("@id", id);
                    }

                    if (title is not null)
                    {
                        command.Parameters.AddWithValue("@title", title);
                    }

                    reader = await command.ExecuteReaderAsync();
                    table.Load(reader);

                    connection.Close();
                }
            }

            return table;
        }

        private async Task DataManipulateAsync(string query, object? dataObject = null, int? id = null)
        {
            await using (NpgsqlConnection connection = new NpgsqlConnection(_config.GetConnectionString("Default")))
            {
                await connection.OpenAsync();

                await using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    if (dataObject is not null)
                    {
                        foreach (var property in dataObject.GetType().GetProperties())
                        {
                            var value = property.GetValue(dataObject);
                            if (value is not null)
                            {
                                command.Parameters.AddWithValue($"@{property.Name}", value);
                            }
                        }
                    }

                    if (id is not null)
                    {
                        command.Parameters.AddWithValue("@Id", id);
                    }

                    await command.ExecuteReaderAsync();

                    connection.Close();
                }
            }
        }
    }
}
