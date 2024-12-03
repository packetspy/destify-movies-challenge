using Microsoft.EntityFrameworkCore;
using MoviesChallenge.Application.Interfaces;
using MoviesChallenge.Application.Services;
using MoviesChallenge.Domain.Interfaces;
using MoviesChallenge.Infra.Data;
using MoviesChallenge.Infra.Repositories;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add Services
builder.Services.AddScoped<IMovieService, MovieService>();
//services.AddScoped<IActorService, ActorService>();
//services.AddScoped<IMovieRatingService, MovieRatingService>();

//Add Repositories
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
//services.AddScoped<IActorRepository, ActorRepository>();
//services.AddScoped<IMovieRatingRepository, MovieRatingRepository>();

//Add Database Context
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
