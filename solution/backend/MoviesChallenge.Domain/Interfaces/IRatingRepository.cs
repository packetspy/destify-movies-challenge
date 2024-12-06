using MoviesChallenge.Domain.Entities;

namespace MoviesChallenge.Domain.Interfaces;

public interface IRatingRepository
{
    Task<MovieRating?> GetBySourceAndValue(string source, string value);
}
