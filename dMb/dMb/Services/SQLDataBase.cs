using System;
using System.Collections.Generic;
using System.Text;

using dMb.Models;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using SQLite;



namespace dMb.Services
{
    public class SQLDataBase
    {

        readonly SQLiteAsyncConnection database;


        public SQLDataBase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Movie>().Wait();
            database.CreateTableAsync<Genre>().Wait();
            database.CreateTableAsync<MovieGenres>().Wait();


            if (database.Table<Genre>().CountAsync().Result == 0)
            {
                GenerateGenresAsync().Wait(); // should be called only on file creation
            }

        }


        #region Movies
        public Task<List<Movie>> GetMoviesAsync()
        {
            // Get all Movies
            return database.Table<Movie>()
                .OrderByDescending(m => m.Id)
                .ToListAsync();
        }

        public Task<List<Movie>> GetMoviesAsync(string title = "", string inGenres = "")
        {
            string conditions = $"{nameof(Movie.Title)} LIKE '%{title}%'";

            if (!string.IsNullOrWhiteSpace(inGenres))
            {
                conditions += $" AND {nameof(MovieGenres.GenreId)} IN {inGenres}";
            }

            QBuilder qBuilder = new QBuilder();
            string query = qBuilder
                .SELECT("*", "COUNT(*)")
                .FROM(nameof(Movie))
                .JOIN(nameof(MovieGenres), nameof(Movie.Id), nameof(MovieGenres.MovieId))
                .WHERE(conditions)
                .GROUP_BY(nameof(Movie.Id))
                .ORDER_BY("COUNT(*)", false)
                .GetQuery().ToString();

            // Get Movies with criterias
            return database.QueryAsync<Movie>(query);
        }


        public Task<Movie> GetMovieAsync(int id)
        {
            // Get Movie by id
            return database.Table<Movie>()
                .Where(m => m.Id == id)
                .FirstOrDefaultAsync();
        }


        public Task<int> SaveMovieAsync(Movie movie)
        {
            if (movie.Id != 0)
            {
                // Update MovieGenres Table
                DeleteMovieGenresAsync(movie);

                // Insert selected Genres to Table
                foreach (var g in movie.Genres)
                {
                    AddMovieGenreAsync(new MovieGenres() { MovieId = movie.Id, GenreId = g.Id });
                }


                // Update existing Movie
                return database.UpdateAsync(movie);
            }
            else
            {
                // How to get Id of movie after inserting it to Table?
                //


                // Add new Movie
                return database.InsertAsync(movie);
            }
        }

        public Task<int> DeleteMovieAsync(Movie movie)
        {
            // Update MovieGenres Table
            DeleteMovieGenresAsync(movie);

            // Delete movie
            return database.DeleteAsync(movie);
        }
        #endregion


        #region Genres
        public Task<List<Genre>> GetGenresAsync()
        {
            // Get all Genres
            return database.Table<Genre>().ToListAsync();
        }

        public Task<List<Genre>> GetGenresAsync(int movieId)
        {
            QBuilder qBuilder = new QBuilder();
            string query = qBuilder
                .SELECT()
                .FROM(nameof(Genre))
                .JOIN(nameof(MovieGenres), nameof(Genre.Id), nameof(MovieGenres.GenreId))
                .WHERE($"{nameof(MovieGenres.MovieId)} = {movieId}")
                .ORDER_BY(nameof(Genre.Id))
                .GetQuery().ToString();

            // Get Genres related to movie
            return database.QueryAsync<Genre>(query);
        }

        public Task<Genre> GetGenreAsync(int id)
        {
            // Get Genre by id
            return database.Table<Genre>()
                .Where(g => g.Id == id)
                .FirstOrDefaultAsync();
        }


        /// <summary>
        /// / !!! IT WILL CLEAR TABLES !!!
        /// / Clears Genre and MovieGenres Tables, then inserts predefined Genres
        /// </summary>
        /// <returns></returns>
        public Task<int> GenerateGenresAsync()
        {
            System.Diagnostics.Debug.WriteLine("Here we generate genre list.");
            var genresDatabase = new List<Genre>
            {
                new Genre{ Name="Watched" },
                new Genre{ Name="Movie" },
                new Genre{ Name="Series" },
                new Genre{ Name="Comic" },
                new Genre{ Name="Anime" },
                new Genre{ Name="Animated" },
                new Genre{ Name="Action" },
                new Genre{ Name="Adventure" },
                new Genre{ Name="Drama" },
                new Genre{ Name="Fantasy" },
                new Genre{ Name="Horror" },
                new Genre{ Name="Music" },
                new Genre{ Name="Mystery" },
                new Genre{ Name="Sci-Fi" },
                new Genre{ Name="???" }
            };

            // Clear table and insert genres
            database.DeleteAllAsync<Genre>().Wait();
            database.DeleteAllAsync<MovieGenres>().Wait();
            return database.InsertAllAsync(genresDatabase);

        }


        private Task<int> AddGenreAsync(Genre genre)
        {
            if(genre.Id != 0)
            {
                // Update existing Genre
                return database.UpdateAsync(genre);
            }
            else
            {
                // Add new Genre
                return database.InsertAsync(genre);
            }
        }

        private Task<int> DeleteGenreAsync(Genre genre)
        {
            // Update MovieGenres Table
            DeleteMovieGenresAsync(genre);

            // Delete genre
            return database.DeleteAsync(genre);
        }
        #endregion


        #region MovieGenres
        public Task<List<MovieGenres>> GetMovieGenresAsync()
        {
            // Get all MovieGenres
            return database.Table<MovieGenres>().ToListAsync();
        }

        private Task<int> AddMovieGenreAsync(MovieGenres moviegenre)
        {
            return database.InsertAsync(moviegenre);
        }

        private Task<int> DeleteMovieGenreAsync(MovieGenres moviegenre)
        {
            return database.DeleteAsync(moviegenre);
        }


        private async void DeleteMovieGenresAsync(Movie movie)
        {
            QBuilder qBuilder = new QBuilder();
            string query = qBuilder
                .DELETE()
                .FROM(nameof(MovieGenres))
                .WHERE($"{nameof(MovieGenres.MovieId)} = {movie.Id}")
                .GetQuery().ToString();

            try
            {
                await database.QueryAsync<MovieGenres>(query);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("\n\nFailed to delete MovieGenres.\n" + e);
            }


        }

        private async void DeleteMovieGenresAsync(Genre genre)
        {
            QBuilder qBuilder = new QBuilder();
            string query = qBuilder
                .DELETE()
                .FROM(nameof(MovieGenres))
                .WHERE($"{nameof(MovieGenres.GenreId)} = {genre.Id}")
                .GetQuery().ToString();

            try
            {
                await database.QueryAsync<MovieGenres>(query);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("\n\nFailed to delete MovieGenres.\n" + e);
            }


        }
        #endregion

    }
}
