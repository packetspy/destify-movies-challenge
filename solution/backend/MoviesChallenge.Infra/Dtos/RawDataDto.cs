using MoviesChallenge.Domain.Entities;

namespace MoviesChallenge.Infra.Dtos;

public class RawDataDto
{
    public string? Title { get; set; }

    public string? Year { get; set; }

    public string? Rated { get; set; }

    public string? Genre { get; set; }

    public string? Plot { get; set; }

    public string? Language { get; set; }

    public string? Country { get; set; }

    public string? Poster { get; set; }

    public string? Actors { get; set; }

    public string? Director { get; set; }

    public List<MovieRating>? Ratings { get; set; } = new List<MovieRating>();
}
