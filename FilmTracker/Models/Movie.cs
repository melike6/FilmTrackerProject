using SQLite;

namespace FilmTracker.Models;

public class Movie
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Title { get; set; }
    public string Director { get; set; }
    public string Genre { get; set; }
    public int ReleaseYear { get; set; }

    public bool IsWatched { get; set; }
    public double Rating { get; set; }
    public string Note { get; set; }

    // ⭐ YENİ
    public bool IsFavorite { get; set; }
}