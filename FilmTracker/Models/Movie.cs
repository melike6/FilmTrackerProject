using SQLite;

namespace FilmTracker.Models;

public class Movie
{
    // SQLite için Primary Key
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    // Film adı
    [NotNull]
    public string Title { get; set; }

    // Yönetmen
    public string Director { get; set; }

    // Tür (Dram, Komedi, Bilim Kurgu vb.)
    public string Genre { get; set; }

    // Yayın yılı
    public int ReleaseYear { get; set; }

    // İzlenme durumu
    public bool IsWatched { get; set; }

    // Kullanıcı puanı (0–10)
    public double Rating { get; set; }

    // Kişisel not / yorum
    public string Note { get; set; }
}