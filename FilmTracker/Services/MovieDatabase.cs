using SQLite;
using FilmTracker.Models;

namespace FilmTracker.Services;

public class MovieDatabase
{
    private readonly SQLiteAsyncConnection _database;

    // Constructor → DB bağlantısı + tablo oluşturma
    public MovieDatabase(string dbPath)
    {
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<Movie>().Wait();
    }

    // 📄 Tüm filmleri getir
    public Task<List<Movie>> GetMoviesAsync()
    {
        return _database.Table<Movie>().ToListAsync();
    }

    // 🔍 ID ile tek film getir
    public Task<Movie> GetMovieAsync(int id)
    {
        return _database.Table<Movie>()
            .Where(m => m.Id == id)
            .FirstOrDefaultAsync();
    }

    // ➕ Film ekle
    public Task<int> SaveMovieAsync(Movie movie)
    {
        if (movie.Id != 0)
        {
            return _database.UpdateAsync(movie);
        }
        else
        {
            return _database.InsertAsync(movie);
        }
    }

    // 🗑️ Film sil
    public Task<int> DeleteMovieAsync(Movie movie)
    {
        return _database.DeleteAsync(movie);
    }
}