using dMb.Controls;
using dMb.Models;
using dMb.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;



namespace dMb.ViewModels
{
    public class MoviesViewModel : BaseViewModel
    {
        Movie _selectedMovie;

        bool _panelVisibility = false;

        string _Search;


        List<Genre> includedGenres;
        List<Genre> excludedGenres;


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

        public string Search
        {
            get => _Search;
            set
            {
                SetProperty(ref _Search, value);

                // Refresh Movie List
                IsBusy = true; // can be moved to SearchBarCommand for better performence?
            }
        }


        public ObservableCollection<Movie> Movies { get; }

        public ObservableCollection<GenreState> Genres { get; }


        public Command LoadMoviesCommand { get; }

        public Command AddMovieCommand { get; }

        public Command<Movie> ItemTappedCommand { get; }

        public Command FilterClickCommand { get; }

        public Command ResetFiltersCommand { get; }



        public MoviesViewModel()
        {
            Title = "Browse";
            Movies = new ObservableCollection<Movie>();
            Genres = new ObservableCollection<GenreState>();

            includedGenres = new List<Genre>();
            excludedGenres = new List<Genre>();

            LoadMoviesCommand = new Command(async () => await LoadMovies());
            AddMovieCommand = new Command(AddMovie);
            ItemTappedCommand = new Command<Movie>(SelectMovie);
            FilterClickCommand = new Command(TogglePanelVisibility);
            ResetFiltersCommand = new Command(ResetFilters);

            // Loading
            IsBusy = true;
            LoadGenres();

        }



        async Task LoadMovies()
        {
            IsBusy = true;

            try
            {
                Movies.Clear();

                // Get Movies default or by criteria (search & filters) if there are any
                var movies = await App.Database.GetMoviesAsync(Search, includedGenres, excludedGenres);

                foreach (var movie in movies)
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
            if (movie == null)
            {
                return;
            }

            // Go to Page of selected movie
            await Shell.Current.GoToAsync($"{nameof(MovieDetailPage)}?{nameof(MovieDetailViewModel.Id)}={movie.Id}");
        }

        void TogglePanelVisibility()
        {
            if (PanelVisibility)
            {
                PanelVisibility = false;

                // Update selected genres
                UpdateFilters();

                // Refresh Movie List
                IsBusy = true; // for better performance should execute only if filters changed
            }
            else PanelVisibility = true;
        }

        void ResetFilters()
        {
            foreach (var gb in Genres)
            {
                gb.State = StateCheckBox.CheckBoxState.Unchecked;
            }

        }


        /// <summary>
        /// Generate list of genres with default state (State = StateCheckBox.CheckBoxState.Unchecked)
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
                    Genres.Add(new GenreState(genre, StateCheckBox.CheckBoxState.Unchecked));
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
            //IsBusy = true; // Don't refresh list when user comes back from MovieDetailsPage
            SelectedMovie = null;

        }


        private void UpdateFilters()
        {
            // Clear previous selections
            includedGenres.Clear();
            excludedGenres.Clear();

            // Get included and excluded genres as lists
            foreach (var genre in Genres)
            {
                if (genre.State == StateCheckBox.CheckBoxState.Checked) includedGenres.Add(genre.Genre);
                else if (genre.State == StateCheckBox.CheckBoxState.Cross) excludedGenres.Add(genre.Genre);
            }

        }

    }
}
