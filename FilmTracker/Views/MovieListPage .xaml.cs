using FilmTracker.Models;

namespace FilmTracker.Views;

public partial class MovieListPage : ContentPage
{
    public MovieListPage()
    {
        InitializeComponent();
        FilterPicker.SelectedIndex = 0;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadMovies();
    }

    private async Task LoadMovies()
    {
        MoviesCollectionView.ItemsSource =
            await App.Database.SearchMoviesAsync(
                SearchBar.Text,
                FilterPicker.SelectedItem?.ToString());
    }

    private async void OnSearchChanged(object sender, TextChangedEventArgs e)
        => await LoadMovies();

    private async void OnFilterChanged(object sender, EventArgs e)
        => await LoadMovies();

    private async void OnAddMovieClicked(object sender, EventArgs e)
        => await Navigation.PushAsync(new AddEditMoviePage());

    private async void MoviesCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Movie movie)
        {
            await Navigation.PushAsync(new MovieDetailPage(movie));
            MoviesCollectionView.SelectedItem = null;
        }
    }

    private async void OnSwipeDelete(object sender, EventArgs e)
    {
        var movie = (sender as SwipeItem)?.CommandParameter as Movie;
        if (movie == null) return;

        if (await DisplayAlert("Sil", $"{movie.Title} silinsin mi?", "Evet", "Hayır"))
        {
            await App.Database.DeleteMovieAsync(movie);
            await LoadMovies();
        }
    }

    private async void OnFavoriteClicked(object sender, EventArgs e)
    {
        var movie = (sender as SwipeItem)?.CommandParameter as Movie;
        if (movie == null) return;

        movie.IsFavorite = !movie.IsFavorite;
        await App.Database.SaveMovieAsync(movie);
        await LoadMovies();
    }

    private async void OnWatchlistClicked(object sender, EventArgs e)
    {
        var movie = (sender as SwipeItem)?.CommandParameter as Movie;
        if (movie == null) return;

        movie.IsInWatchlist = !movie.IsInWatchlist;

        if (movie.IsWatched)
            movie.IsInWatchlist = false;

        await App.Database.SaveMovieAsync(movie);
        await LoadMovies();
    }
}
