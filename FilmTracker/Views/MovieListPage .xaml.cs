using FilmTracker.Models;

namespace FilmTracker.Views;

public partial class MovieListPage : ContentPage
{
    public MovieListPage()
    {
        InitializeComponent();
        FilterPicker.SelectedIndex = 0; // Tümü
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadMovies();
    }

    private async Task LoadMovies()
    {
        string searchText = SearchBar.Text;
        string filter = FilterPicker.SelectedItem?.ToString();

        MoviesCollectionView.ItemsSource =
            await App.Database.SearchMoviesAsync(searchText, filter);
    }

    private async void OnSearchChanged(object sender, TextChangedEventArgs e)
    {
        await LoadMovies();
    }

    private async void OnFilterChanged(object sender, EventArgs e)
    {
        await LoadMovies();
    }

    private async void OnAddMovieClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddEditMoviePage());
    }

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
        var swipeItem = sender as SwipeItem;
        var movie = swipeItem?.CommandParameter as Movie;

        if (movie == null)
            return;

        bool confirm = await DisplayAlert(
            "Sil",
            $"{movie.Title} silinsin mi?",
            "Evet",
            "Hayır");

        if (!confirm)
            return;

        await App.Database.DeleteMovieAsync(movie);
        await LoadMovies();
    }

    private async void OnFavoriteClicked(object sender, EventArgs e)
    {
        var swipeItem = sender as SwipeItem;
        var movie = swipeItem?.CommandParameter as Movie;

        if (movie == null)
            return;

        movie.IsFavorite = !movie.IsFavorite;
        await App.Database.SaveMovieAsync(movie);
        await LoadMovies();
    }
}
