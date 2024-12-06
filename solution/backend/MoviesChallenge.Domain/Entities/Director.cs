namespace MoviesChallenge.Domain.Entities;

public class Director : BaseModel
{
    public string Name { get; set; } = string.Empty;
    public List<Movie> Movies { get; set; } = new List<Movie>();
}
