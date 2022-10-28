using BooksManagerAPI.Data;
using BooksManagerAPI.Repository;
using BooksManagerAPI.RepositoryContracts;
using BooksManagerAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BookManagerDbContext>(options =>
    options.UseNpgsql(builder.Configuration["ConnectionStrings:Default"]));

builder.Services.AddScoped(typeof(BookDataMappingManager));
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped(typeof(BookDataManager));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
});

app.UseAuthorization();

app.MapControllers();

app.Run();
