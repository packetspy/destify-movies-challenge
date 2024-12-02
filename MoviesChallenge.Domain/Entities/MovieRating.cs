namespace MoviesChallenge.Domain.Entities;

public class MovieRating : BaseModel
{
    public int RatingValue { get; set; }

    public int MovieId { get; set; }

    public Movie Movie { get; set; } = new Movie();
}
