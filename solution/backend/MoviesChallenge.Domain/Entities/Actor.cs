namespace MoviesChallenge.Domain.Entities;

public class Actor : BaseModel
{
    public string Name { get; set; } = string.Empty;
    
    public List<Movie> Movies { get; set; } = [];
}
