using Dapper;
using Microsoft.AspNetCore.Mvc;
using MoviesAPI.DTOs.Models;
using MoviesAPI.DTOs.Responses;
using System.Data;

[Route("api/[controller]")]
[ApiController]
public class MoviesController : ControllerBase
{
    private readonly IDbConnection _db;

    public MoviesController(IDbConnection db)
    {
        _db = db;
    }

    // GET api/movies
    // https://localhost:7243/api/movies
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Movie>>> GetAll()
    {
        var movies = await _db.QueryAsync<Movie>("SELECT * FROM dbo.Movies");
        return Ok(movies);
    }
    +
    // GET api/movies/{id}
    // http://localhost:5180/api/movies/1-
    // http://localhost:5180/api/movies/999999 (fake)
    [HttpGet("{id:int}")]
    public async Task<ActionResult<MovieDetailsResponse>> GetById(int id)
    {
        const string sql = """
        SELECT Id, Title, GenreId, AudienceRatingId, Synopsis, Runtime, ReleaseDate, PosterUrl
        FROM dbo.Movies
        WHERE Id = @Id;

        SELECT
            r.Id,
            r.MovieId,
            r.Ratings        AS Rating,
            r.Description    AS Comment,
            r.CreatedDate    AS CreatedAtUtc,
            u.Name           AS CreatorName
        FROM dbo.CriticReviews r
        INNER JOIN dbo.UserSnapshot u ON u.UserId = r.CreatedBy
        WHERE r.MovieId = @Id AND r.IsPublished = 1
        ORDER BY r.CreatedDate DESC;
        """;

        using var multi = await _db.QueryMultipleAsync(sql, new { Id = id });

        var movie = await multi.ReadSingleOrDefaultAsync<Movie>();
        if (movie is null)
        {
            return NotFound();
        }

        var reviews = (await multi.ReadAsync<MovieReviewResponse>()).ToList();

        return Ok(new MovieDetailsResponse
        {
            Id = movie.Id,
            Title = movie.Title,
            GenreId = movie.GenreId,
            AudienceRatingId = movie.AudienceRatingId,
            Synopsis = movie.Synopsis,
            Runtime = movie.Runtime,
            ReleaseDate = movie.ReleaseDate,
            PosterUrl = movie.PosterUrl,
            Reviews = reviews
        });
    }
}