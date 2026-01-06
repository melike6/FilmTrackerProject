using FilmTracker.Models;

namespace FilmTracker.Views;

public partial class AddEditMoviePage : ContentPage
{
    private Movie _movie;
    
    public AddEditMoviePage()
    {
        InitializeComponent();
        _movie = new Movie();

        HookEvents();
    }
    
    public AddEditMoviePage(Movie movie)
    {
        InitializeComponent();
        _movie = movie;

        TitleEntry.Text = movie.Title;
        DirectorEntry.Text = movie.Director;
        GenrePicker.SelectedItem = movie.Genre;
        YearEntry.Text = movie.ReleaseYear.ToString();
        WatchedSwitch.IsToggled = movie.IsWatched;
        FavoriteSwitch.IsToggled = movie.IsFavorite;
        WatchlistSwitch.IsToggled = movie.IsInWatchlist;
        RatingSlider.Value = movie.Rating;
        NoteEditor.Text = movie.Note;

        HookEvents();
    }
    
    private void HookEvents()
    {
        WatchedSwitch.Toggled += (_, _) =>
        {
            if (WatchedSwitch.IsToggled)
            {
                WatchlistSwitch.IsToggled = false;
                WatchlistSwitch.IsEnabled = false;
            }
            else
            {
                WatchlistSwitch.IsEnabled = true;
            }
        };
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TitleEntry.Text))
        {
            await DisplayAlert("Hata", "Film adı boş olamaz", "Tamam");
            return;
        }

        if (!int.TryParse(YearEntry.Text, out int year))
        {
            await DisplayAlert("Hata", "Geçerli bir yıl girin", "Tamam");
            return;
        }

        _movie.Title = TitleEntry.Text;
        _movie.Director = DirectorEntry.Text;
        _movie.Genre = GenrePicker.SelectedItem?.ToString();
        _movie.ReleaseYear = year;
        _movie.IsWatched = WatchedSwitch.IsToggled;
        _movie.IsFavorite = FavoriteSwitch.IsToggled;
        _movie.IsInWatchlist = WatchlistSwitch.IsToggled;
        _movie.Rating = RatingSlider.Value;
        _movie.Note = NoteEditor.Text;
        
        if (_movie.IsWatched)
            _movie.IsInWatchlist = false;

        await App.Database.SaveMovieAsync(_movie);
        await Navigation.PopAsync();
    }
}
