using FilmTracker.Views;
using FilmTracker.Services;

namespace FilmTracker;

public partial class App : Application
{
    private static MovieDatabase _database;

    public static MovieDatabase Database
    {
        get
        {
            if (_database == null)
            {
                string dbPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "filmtracker.db3"
                );

                _database = new MovieDatabase(dbPath);
            }

            return _database;
        }
    }

    public App()
    {
        InitializeComponent();

        MainPage = new NavigationPage(new MovieListPage());
    }
}