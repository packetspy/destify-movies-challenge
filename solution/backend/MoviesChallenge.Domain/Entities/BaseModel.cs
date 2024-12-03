namespace MoviesChallenge.Domain.Entities;

public class BaseModel
{
    public int Id { get; set; }
    
    public Guid UniqueId { get; set; }
        
    public DateTime CreatedAt { get; set; }
    
    public DateTime? ModifiedAt { get; set; }
}
