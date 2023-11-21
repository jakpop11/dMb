using dMb.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;



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
            return database
                .Table<Movie>()
                .OrderByDescending(m => m.Id)
                .ToListAsync();
        }


        public Task<List<Movie>> GetMoviesAsync(string title = "", List<Genre> includedGenres = null, List<Genre> excludedGenres = null)
        {
            QBuilder qBuilder = new QBuilder();

            List<string> conditionsList = new List<string>();

            if (!string.IsNullOrWhiteSpace(title))
            {
                conditionsList.Add($"{nameof(Movie.Title)} LIKE '%{title}%'");
                System.Diagnostics.Debug.WriteLine($"Title: {title}");
            }

            if (includedGenres != null && includedGenres.Count != 0)
            {
                string genres = ConvertGenreListToString(includedGenres);
                conditionsList.Add($"{nameof(MovieGenres.GenreId)} IN {genres}");
                System.Diagnostics.Debug.WriteLine($"Included Genres: {genres}");
            }

            if (excludedGenres != null && excludedGenres.Count != 0)
            {
                string genres = ConvertGenreListToString(excludedGenres);
                string innerQuery = qBuilder
                    .SELECT(nameof(MovieGenres.MovieId))
                    .FROM(nameof(MovieGenres))
                    .WHERE($"{nameof(MovieGenres.GenreId)} IN {genres}")
                    .GROUP_BY(nameof(MovieGenres.MovieId))
                    .GetQuery().ToString();

                conditionsList.Add($"{nameof(MovieGenres.MovieId)} NOT IN ({innerQuery})");
                System.Diagnostics.Debug.WriteLine($"NOT included Genres: {genres}");
            }

            // Return basic query results
            if (conditionsList.Count == 0) return GetMoviesAsync();

            string conditions = QBuilder.ConditionsListToString(conditionsList);

            string query = qBuilder
                .SELECT("*", "COUNT(*)")
                .FROM(nameof(Movie))
                .LEFT_JOIN(nameof(MovieGenres), nameof(MovieGenres.MovieId), nameof(Movie.Id))
                .WHERE(conditions)
                .GROUP_BY(nameof(Movie.Id))
                .ORDER_BY("COUNT(*)", false)
                .GetQuery().ToString();

            System.Diagnostics.Debug.WriteLine($"Query: {query}");

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
            try
            {

                if (movie.Id != 0)
                {
                    // Update MovieGenres Table
                    DeleteMovieGenresAsync(movie);

                    // Update existing Movie
                    return database.UpdateAsync(movie);
                }
                else
                {

                    // Add new Movie
                    var result = database.InsertAsync(movie);
                    result.Wait();
                    return result;
                }
            }
            // is this best way to get inserted movie id
            finally
            {
                // Insert selected Genres to Table
                foreach (var g in movie.Genres)
                {
                    AddMovieGenreAsync(new MovieGenres() { MovieId = movie.Id, GenreId = g.Id });
                }
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
                new Genre{ Name="Comedy" },
                new Genre{ Name="Crime" },
                new Genre{ Name="Disaster" },
                new Genre{ Name="Drama" },
                new Genre{ Name="Fantasy" },
                new Genre{ Name="History" },
                new Genre{ Name="Horror" },
                new Genre{ Name="Isekai" },
                new Genre{ Name="Music" },
                new Genre{ Name="Mystery" },
                new Genre{ Name="Psychological" },
                new Genre{ Name="Romance" },
                new Genre{ Name="Sci-Fi" },
                new Genre{ Name="Slice of Life" },
                new Genre{ Name="Sports" },
                new Genre{ Name="Thriller" },
                new Genre{ Name="???" }
            };

            // Clear table and insert genres
            database.DeleteAllAsync<Genre>().Wait();
            database.DeleteAllAsync<MovieGenres>().Wait();
            return database.InsertAllAsync(genresDatabase);

        }


        private Task<int> AddGenreAsync(Genre genre)
        {
            if (genre.Id != 0)
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


        #region Helpers
        private string ConvertGenreListToString(List<Genre> genres)
        {
            string result = "(";
            foreach (var genre in genres)
            {
                result += $"{genre.Id}, ";
            }
            // Delete last comma and close brackets
            result = result.Remove(result.Length - 2) + ")";

            return result;
        }


        #endregion
    }
}
