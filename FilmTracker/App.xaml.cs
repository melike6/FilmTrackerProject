using FilmTracker.Views;

namespace FilmTracker;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new NavigationPage(new MovieListPage());
    }
}
