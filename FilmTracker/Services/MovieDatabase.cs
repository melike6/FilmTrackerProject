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

    // Tüm filmleri getir
    public Task<List<Movie>> GetMoviesAsync()
    {
        return _database.Table<Movie>().OrderBy(m => m.Title).ToListAsync();
    }

    // ID ile film getir
    public Task<Movie> GetMovieAsync(int id)
    {
        return _database.Table<Movie>()
            .Where(m => m.Id == id)
            .FirstOrDefaultAsync();
    }

    // Film ekle / güncelle
    public Task<int> SaveMovieAsync(Movie movie)
    {
        if (movie.Id != 0)
            return _database.UpdateAsync(movie);

        return _database.InsertAsync(movie);
    }

    // Film sil
    public Task<int> DeleteMovieAsync(Movie movie)
    {
        return _database.DeleteAsync(movie);
    }
    public Task<List<Movie>> SearchMoviesAsync(string searchText, string filter)
    {
        var query = _database.Table<Movie>();

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            query = query.Where(m => m.Title.Contains(searchText));
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
        }

        return query.OrderBy(m => m.Title).ToListAsync();
    }

}