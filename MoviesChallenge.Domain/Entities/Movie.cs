namespace MoviesChallenge.Domain.Entities;

public class Movie : BaseModel
{
    public string? Title { get; set; }

    public DateTime ReleaseDate { get; set; }
    
    public List<Actor> Actors { get; set; } = [];
    
    public List<MovieRating>? Ratings { get; set; } = [];
}
