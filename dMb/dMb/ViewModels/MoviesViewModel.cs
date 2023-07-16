using System;
using System.Collections.Generic;
using System.Text;

using dMb.Models;
using dMb.Views;
using dMb.Services;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Threading.Tasks;



namespace dMb.ViewModels
{
    public class MoviesViewModel : BaseViewModel
    {
        Movie _selectedMovie;

        bool _panelVisibility = false;


        public Movie SelectedMovie
        {
            get => _selectedMovie;
            set
            {
                SetProperty(ref _selectedMovie, value);
                SelectMovie(value);
            }
        }

        public bool PanelVisibility
        {
            get => _panelVisibility;
            set => SetProperty(ref _panelVisibility, value);
        }


        public ObservableCollection<Movie> Movies { get; }

        public ObservableCollection<GenreBool> Genres { get; }


        public Command LoadMoviesCommand { get; }

        public Command AddMovieCommand { get; }

        public Command<Movie> ItemTappedCommand { get; }

        public Command FilterClickCommand { get; }



        public MoviesViewModel()
        {
            Title = "Browse";
            Movies = new ObservableCollection<Movie>();
            Genres = new ObservableCollection<GenreBool>();

            LoadMoviesCommand = new Command(async () => await LoadMovies());
            AddMovieCommand = new Command(AddMovie);
            ItemTappedCommand = new Command<Movie>(SelectMovie);
            FilterClickCommand = new Command(TogglePanelVisibility);

            LoadGenres();
        }



        async Task LoadMovies()
        {
            IsBusy = true;

            try
            {
                Movies.Clear();
                var movies = await App.Database.GetMoviesAsync();

                foreach(var movie in movies)
                {
                    movie.Genres = await App.Database.GetGenresAsync(movie.Id);
                    Movies.Add(movie);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
            finally
            {
                IsBusy = false;
            }
        }

        async void AddMovie(object obj)
        {
            // Go to Page for new movie
            await Shell.Current.GoToAsync($"{nameof(MovieDetailPage)}?{nameof(MovieDetailViewModel.Id)}={-1}");
        }

        async void SelectMovie(Movie movie)
        {
            if(movie == null)
            {
                return;
            }

            //await Shell.Current.GoToAsync(nameof(MovieDetailPage));

            await Shell.Current.GoToAsync($"{nameof(MovieDetailPage)}?{nameof(MovieDetailViewModel.Id)}={movie.Id}");
        }

        void TogglePanelVisibility()
        {
            if (PanelVisibility) PanelVisibility = false;
            else PanelVisibility = true;

            // TEST
            TempVoid();
        }

        async Task LoadGenres()
        {
            try
            {
                Genres.Clear();
                var genres = await App.Database.GetGenresAsync();

                foreach (var genre in genres)
                {
                    Genres.Add(new GenreBool { Genre = genre, Bool = false });
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }



        public void OnAppearing()
        {
            IsBusy = true;
            SelectedMovie = null;

            //LoadGenres();

        }


        // TEMP TEST
        void TempVoid()
        {
            //System.Diagnostics.Debug.WriteLine($"{Genres[0].Genre.Name}: {Genres[0].Bool}");
            //System.Diagnostics.Debug.WriteLine($"{Genres[3].Genre.Name}: {Genres[3].Bool}");

            //System.IO.File.WriteAllText(App.LocalPath, "Hello World from txt");
            //System.Diagnostics.Debug.WriteLine($"Write to file: {App.LocalPath}");

            //var text = System.IO.File.ReadAllText(App.LocalPath);
            //System.Diagnostics.Debug.WriteLine($"In File: {text}");

        }


        // END
    }
}
