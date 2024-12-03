namespace MoviesChallenge.Domain.Entities;

public class Movie : BaseModel
{
    public string? Title { get; set; }
    
    public int Year { get; set; }

    public string? Rated { get; set; }

    public string? Genre { get; set; }

    public string? Plot { get; set; }

    public string? Language { get; set; }

    public string? Country { get; set; }

    public string? Poster { get; set; }

    public List<Actor> Actors { get; set; } = [];

    public List<Director> Directors { get; set; } = [];

    public List<MovieRating>? Ratings { get; set; } = [];
}
