namespace MoviesChallenge.Application.Dtos;

public class DirectorDto
{
    public Guid UniqueId { get; set; }
    public string? Name { get; set; }
    public List<MovieDto>? Movies { get; set; } = new List<MovieDto>();
}
