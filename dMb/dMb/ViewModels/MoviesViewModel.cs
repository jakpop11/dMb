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

                // Get Movies by criteria (search & filters)
                //

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

            // Go to Page of selected movie
            await Shell.Current.GoToAsync($"{nameof(MovieDetailPage)}?{nameof(MovieDetailViewModel.Id)}={movie.Id}");
        }

        void TogglePanelVisibility()
        {
            if (PanelVisibility) PanelVisibility = false;
            else PanelVisibility = true;
        }

        /// <summary>
        /// Generate list of genres with default state (Bool = false)
        /// </summary>
        /// <returns></returns>
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
            // Refresh list of Movies
            IsBusy = true;
            SelectedMovie = null;

        }





        // TEMP TEST
        void TempVoid()
        {


        }


        // END
    }
}
