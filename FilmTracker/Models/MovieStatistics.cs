namespace FilmTracker.Models;

public class MovieStatistics
{
    public int TotalMovies { get; set; }
    public int WatchedMovies { get; set; }
    public int WatchlistMovies { get; set; }
    public int FavoriteMovies { get; set; }
    public double AverageRating { get; set; }
    public string MostWatchedGenre { get; set; }
}