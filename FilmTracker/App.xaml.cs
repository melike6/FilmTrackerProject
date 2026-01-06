using FilmTracker.Data;
using FilmTracker.Views;

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
                    FileSystem.AppDataDirectory,
                    "films.db");

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