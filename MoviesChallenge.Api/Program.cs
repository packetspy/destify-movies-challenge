using Microsoft.EntityFrameworkCore;
using MoviesChallenge.Infra.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MovieDbContext>(options => options.UseInMemoryDatabase("MoviesDB"));

var app = builder.Build();


using (var scope = app.Services.CreateScope())
    MovieSeeder.RunSeed(scope);


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
