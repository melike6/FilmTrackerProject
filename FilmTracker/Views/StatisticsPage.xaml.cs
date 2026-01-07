using FilmTracker.Models;

namespace FilmTracker.Views;

public partial class StatisticsPage : ContentPage
{
    public StatisticsPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadStatistics();
    }

    private async Task LoadStatistics()
    {
        MovieStatistics stats = await App.Database.GetStatisticsAsync();

        TotalMoviesLabel.Text = $"🎬 Toplam Film: {stats.TotalMovies}";
        WatchedMoviesLabel.Text = $"✔️ İzlenen: {stats.WatchedMovies}";
        WatchlistMoviesLabel.Text = $"👀 Watchlist: {stats.WatchlistMovies}";
        FavoriteMoviesLabel.Text = $"⭐ Favoriler: {stats.FavoriteMovies}";
        AverageRatingLabel.Text = $"⭐ Ortalama Puan: {stats.AverageRating}/10";
        MostWatchedGenreLabel.Text = $"🎭 En Çok İzlenen Tür: {stats.MostWatchedGenre}";
    }
}