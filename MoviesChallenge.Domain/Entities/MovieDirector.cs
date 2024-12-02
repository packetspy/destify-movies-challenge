namespace MoviesChallenge.Domain.Entities;

public class MovieDirector
{
    public int DirectorId { get; set; }

    public int MovieId { get; set; }

    public Movie Movie { get; set; } = new Movie();
}
