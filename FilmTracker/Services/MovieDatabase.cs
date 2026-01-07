using SQLite;
using FilmTracker.Models;

namespace FilmTracker.Data;

public class MovieDatabase
{
    private readonly SQLiteAsyncConnection _database;

    public MovieDatabase(string dbPath)
    {
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<Movie>().Wait();
    }
    

    public Task<List<Movie>> GetMoviesAsync()
        => _database.Table<Movie>()
                    .OrderBy(m => m.Title)
                    .ToListAsync();

    public Task<Movie> GetMovieAsync(int id)
        => _database.Table<Movie>()
                    .FirstOrDefaultAsync(m => m.Id == id);

    public Task<int> SaveMovieAsync(Movie movie)
    {
        if (movie.Id != 0)
            return _database.UpdateAsync(movie);

        return _database.InsertAsync(movie);
    }

    public Task<int> DeleteMovieAsync(Movie movie)
        => _database.DeleteAsync(movie);

    

    public async Task<List<Movie>> SearchMoviesAsync(string searchText, string filter)
    {
        var query = _database.Table<Movie>();

        
        if (!string.IsNullOrWhiteSpace(searchText))
        {
            query = query.Where(m =>
                m.Title.Contains(searchText) ||
                m.Director.Contains(searchText));
        }

     
        switch (filter)
        {
            case "İzlenen":
                query = query.Where(m => m.IsWatched);
                break;

            case "İzlenmeyen":
                query = query.Where(m => !m.IsWatched);
                break;

            case "Favoriler":
                query = query.Where(m => m.IsFavorite);
                break;

            case "Watchlist":
                query = query.Where(m => m.IsInWatchlist);
                break;

            // "Tümü" → filtre yok
        }

        return await query
            .OrderByDescending(m => m.IsFavorite)
            .ThenBy(m => m.Title)
            .ToListAsync();
    }

    
    // STATISTICS
   

    public async Task<MovieStatistics> GetStatisticsAsync()
    {
        var movies = await _database.Table<Movie>().ToListAsync();

        return new MovieStatistics
        {
            TotalMovies = movies.Count,
            WatchedMovies = movies.Count(m => m.IsWatched),
            WatchlistMovies = movies.Count(m => m.IsInWatchlist),
            FavoriteMovies = movies.Count(m => m.IsFavorite),
            AverageRating = movies.Any()
                ? Math.Round(movies.Average(m => m.Rating), 1)
                : 0,
            MostWatchedGenre = movies
                .Where(m => m.IsWatched && !string.IsNullOrWhiteSpace(m.Genre))
                .GroupBy(m => m.Genre)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault() ?? "Yok"
        };
    }
}

