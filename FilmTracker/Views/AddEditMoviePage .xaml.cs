using FilmTracker.Models;

namespace FilmTracker.Views;

public partial class AddEditMoviePage : ContentPage
{
    private Movie _movie;

    public AddEditMoviePage()
    {
        InitializeComponent();
        _movie = new Movie();
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
        RatingSlider.Value = movie.Rating;
        NoteEditor.Text = movie.Note;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TitleEntry.Text))
        {
            await DisplayAlert("Hata", "Film adı boş olamaz", "Tamam");
            return;
        }

        _movie.Title = TitleEntry.Text;
        _movie.Director = DirectorEntry.Text;
        _movie.Genre = GenrePicker.SelectedItem?.ToString();
        _movie.IsWatched = WatchedSwitch.IsToggled;
        _movie.Rating = RatingSlider.Value;
        _movie.Note = NoteEditor.Text;

        if (!int.TryParse(YearEntry.Text, out int year))
        {
            await DisplayAlert("Hata", "Geçerli bir yıl giriniz", "Tamam");
            return;
        }

        _movie.ReleaseYear = year;

        await App.Database.SaveMovieAsync(_movie);
        await Navigation.PopAsync();
    }
}