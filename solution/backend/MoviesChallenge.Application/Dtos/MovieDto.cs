namespace MoviesChallenge.Application.Dtos;

public class MovieDto
{
    public Guid UniqueId { get; set; }
    public string? Title { get; set; }
    public int Year { get; set; }
    public string? Rated { get; set; }
    public string? Genre { get; set; }
    public string? Plot { get; set; }
    public string? Language { get; set; }
    public string? Country { get; set; }
    public string? Poster { get; set; }
    public List<ActorDto>? Actors { get; set; }
    public List<DirectorDto>? Directors { get; set; }
    public List<MovieRatingDto>? Ratings { get; set; } = new List<MovieRatingDto>();
}
