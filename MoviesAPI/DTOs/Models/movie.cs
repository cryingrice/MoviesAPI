namespace MoviesAPI.DTOs.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public required string Title { get; set; } = string.Empty;
        public int? GenreId { get; set; }
        public int? AudienceRatingId { get; set; }
        public string? Synopsis { get; set; } = string.Empty;
        public int Runtime { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public String? PosterUrl { get; set; }

    }
}

namespace MoviesAPI.DTOs.Responses
{
    public class MovieDetailsResponse
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public int? GenreId { get; set; }
        public int? AudienceRatingId { get; set; }
        public string? Synopsis { get; set; }
        public int Runtime { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public string? PosterUrl { get; set; }
        public List<MovieReviewResponse> Reviews { get; set; } = new();
    }

    public class MovieReviewResponse
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public required string CreatorName { get; set; }
    }
}