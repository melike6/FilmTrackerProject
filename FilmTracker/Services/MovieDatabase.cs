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

    // 🔹 TÜM FİLMLER
    public Task<List<Movie>> GetMoviesAsync()
        => _database.Table<Movie>().OrderBy(m => m.Title).ToListAsync();

    // 🔹 ID İLE TEK FİLM
    public Task<Movie> GetMovieAsync(int id)
        => _database.Table<Movie>().FirstOrDefaultAsync(m => m.Id == id);

    // 🔹 EKLE / GÜNCELLE
    public Task<int> SaveMovieAsync(Movie movie)
    {
        if (movie.Id != 0)
            return _database.UpdateAsync(movie);
        else
            return _database.InsertAsync(movie);
    }

    // 🔹 SİL
    public Task<int> DeleteMovieAsync(Movie movie)
        => _database.DeleteAsync(movie);

    // 🔹 ARAMA + FİLTRE (EN ÖNEMLİ METOT)
    public async Task<List<Movie>> SearchMoviesAsync(string searchText, string filter)
    {
        var query = _database.Table<Movie>();

        // 🔍 Arama
        if (!string.IsNullOrWhiteSpace(searchText))
        {
            query = query.Where(m =>
                m.Title.Contains(searchText) ||
                m.Director.Contains(searchText));
        }

        // 🎯 Filtre
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

            // "Tümü" veya null → filtre yok
        }

        return await query
            .OrderByDescending(m => m.IsFavorite)
            .ThenBy(m => m.Title)
            .ToListAsync();
    }
}
