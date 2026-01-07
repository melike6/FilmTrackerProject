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

    private async void OnStatisticsClicked(object sender, EventArgs e)
        => await Navigation.PushAsync(new StatisticsPage());

    private async void OnMovieSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Movie movie)
        {
            await Navigation.PushAsync(new MovieDetailPage(movie));
            MoviesCollectionView.SelectedItem = null;
        }
    }

   
    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        var movie = (sender as SwipeItem)?.CommandParameter as Movie;
        if (movie == null) return;

        bool confirm = await DisplayAlert(
            "Sil",
            $"{movie.Title} silinsin mi?",
            "Evet",
            "Hayır");

        if (!confirm) return;

        await App.Database.DeleteMovieAsync(movie);
        await LoadMovies();
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

        
        if (movie.IsWatched)
            return;

        movie.IsInWatchlist = !movie.IsInWatchlist;
        await App.Database.SaveMovieAsync(movie);
        await LoadMovies();
    }
}
