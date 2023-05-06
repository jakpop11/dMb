using System;
using System.Collections.Generic;
using System.Text;

using dMb.Models;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;



namespace dMb.ViewModels
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public class MovieDetailViewModel : BaseViewModel
    {
        int _id;
        string _movieTitle;
        string _movieImgUrl;
        string _movieDetails;
        DateTime _movieEditDate;
        

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                LoadMovieId(value);
                LoadGenres(value);

            }
        }

        public int MovieId { get; set; }

        public string MovieTitle
        {
            get => _movieTitle;
            set => SetProperty(ref _movieTitle, value);
        }

        public string MovieImgUrl
        {
            get => _movieImgUrl;
            set => SetProperty(ref _movieImgUrl, value);
        }

        public string MovieDetails
        {
            get => _movieDetails;
            set => SetProperty(ref _movieDetails, value);
        }

        public DateTime MovieEditDate
        {
            get => _movieEditDate;
            set => SetProperty(ref _movieEditDate, value);
        }


        public ObservableCollection<GenreBool> Genres { get; }


        public Command SaveMovieCommand { get; }
        public Command DeleteMovieCommand { get; }
        public Command ChangeImgUrlCommand { get; }


        public MovieDetailViewModel()
        {
            Title = "Details";
            Genres = new ObservableCollection<GenreBool>();

            SaveMovieCommand = new Command(SaveMovie);
            DeleteMovieCommand = new Command(DeleteMovie);
            ChangeImgUrlCommand = new Command(ChangeImgUrl);

        }



        async void SaveMovie()
        {
            List<Genre> selectedGenre = new List<Genre>();
            foreach(var gb in Genres)
            {
                if (gb.Bool)
                {
                    selectedGenre.Add(gb.Genre);
                }
            }

            System.Diagnostics.Debug.WriteLine("Saving movie");
            Movie newMovie = new Movie
            {
                Id = MovieId,
                Title = MovieTitle,
                ImgUrl = MovieImgUrl,
                Details = MovieDetails,
                EditDate = DateTime.Now,
                Genres = selectedGenre
            };

            // Instert or update Movie in Database
            await App.Database.SaveMovieAsync(newMovie);


            await Shell.Current.GoToAsync("..");
        }

        async void DeleteMovie()
        {
            Movie movie = await App.Database.GetMovieAsync(Id);


            try
            {
                await App.Database.DeleteMovieAsync(movie);
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Failed to delete movie");
            }

            await Shell.Current.GoToAsync("..");
        }

        async void ChangeImgUrl()
        {
            try
            {
                // Display prompt to pass image url adress
                string result = await Shell.Current.DisplayPromptAsync("Select URL for image", "Enter Url", "OK", "Cancel", placeholder: MovieImgUrl);

                // On Cancel
                if (result == null) return;

                // On passing empty string
                if (result == "")
                {
                    result = "https://images5.fanpop.com/image/photos/29000000/Death-the-Kid-mtndewluver-29044843-225-350.jpg";
                }

                MovieImgUrl = result;

            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Failed to Load an image.");
            }

        }



        async void LoadMovieId(int movieId)
        {
            try
            {
                var movie = await App.Database.GetMovieAsync(movieId);


                if (movie == null)
                {
                    // Generate values for new movie
                    MovieImgUrl = "https://shinden.pl/res/other/placeholders/title/225x350.jpg";
                    MovieEditDate = DateTime.Now;
                }
                else
                {
                    // Generate values from selected movie
                    MovieId = movie.Id;
                    MovieTitle = movie.Title;
                    MovieImgUrl = movie.ImgUrl;
                    MovieDetails = movie.Details;
                    MovieEditDate = movie.EditDate;
                }

            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("Failed to load movie");
            }
        }

        /// <summary>
        /// Genrate list of Genres with checked Bool
        /// </summary>
        /// <param name="movieID"></param>
        async void LoadGenres(int movieID)
        {
            try
            {
                Genres.Clear();
                var genres = await App.Database.GetGenresAsync();
                var movieGenres = await App.Database.GetGenresAsync(movieID);

                foreach (var genre in genres)
                {
                    Genres.Add(new GenreBool(genre, movieGenres.Any(g => g.Id == genre.Id)));
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }


        

    }
}
