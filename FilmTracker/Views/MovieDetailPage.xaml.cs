using FilmTracker.Models;

namespace FilmTracker.Views;

public partial class MovieDetailPage : ContentPage
{
    private Movie _movie;

    public MovieDetailPage(Movie movie)
    {
        InitializeComponent();
        _movie = movie;
        LoadMovie();
    }

    private void LoadMovie()
    {
        TitleLabel.Text = _movie.Title;
        DirectorLabel.Text = $"Yönetmen: {_movie.Director}";
        GenreLabel.Text = $"Tür: {_movie.Genre}";
        YearLabel.Text = $"Yıl: {_movie.ReleaseYear}";
        WatchedLabel.Text = $"İzlendi: {(_movie.IsWatched ? "Evet" : "Hayır")}";
        RatingLabel.Text = $"Puan: {_movie.Rating}/10";
        NoteLabel.Text = _movie.Note;
    }

    private async void OnEditClicked(object sender, EventArgs e)
        => await Navigation.PushAsync(new AddEditMoviePage(_movie));

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (await DisplayAlert("Sil", "Bu film silinsin mi?", "Evet", "Hayır"))
        {
            await App.Database.DeleteMovieAsync(_movie);
            await Navigation.PopAsync();
        }
    }
}