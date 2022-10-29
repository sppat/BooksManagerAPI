using BooksManagerAPI.Data;
using BooksManagerAPI.Interfaces.CacheInterfaces;
using BooksManagerAPI.Interfaces.RepositoryInterfaces;
using BooksManagerAPI.Middleware;
using BooksManagerAPI.Repository;
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

builder.Services.AddStackExchangeRedisCache(options =>
    options.Configuration = builder.Configuration["ConnectionStrings:Redis"]);

builder.Services.AddSingleton<ICacheService, CacheService>();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

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
