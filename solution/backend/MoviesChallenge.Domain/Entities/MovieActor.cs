namespace MoviesChallenge.Domain.Entities;

public class MovieActor
{
    public int ActorId { get; set; }
    public int MovieId { get; set; }
    public Movie Movie { get; set; } = new Movie();
}
