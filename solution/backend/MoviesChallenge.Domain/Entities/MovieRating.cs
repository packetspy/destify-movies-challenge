namespace MoviesChallenge.Domain.Entities;

public class MovieRating : BaseModel
{
    public string? Source { get; set; }

    public string? Value { get; set; }

    public int MovieId { get; set; }

    public Movie Movie { get; set; } = new Movie();
}
