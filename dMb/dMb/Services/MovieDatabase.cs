using dMb.Models;
using System;
using System.Collections.Generic;
using System.Linq;



namespace dMb.Services
{
    public class MovieDatabase
    {

        readonly List<Movie> database;

        readonly List<Genre> genresDatabase;

        public MovieDatabase(string dbPath)
        {
            //string database = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));


            database = new List<Movie>();
            for (int i = 0; i < 20; i++)
            {
                database.Add(new Movie
                {
                    Id = i,
                    Title = $"Movie {i}",
                    EditDate = DateTime.Now,
                    Details = "Movie details will be loaded here.",
                    ImgUrl = "https://shinden.pl/res/other/placeholders/title/225x350.jpg"
                });
            }

            genresDatabase = new List<Genre>
            {
                new Genre{ Id=0, Name="Watched" },
                new Genre{ Id=1, Name="Movie" },
                new Genre{ Id=3, Name="Series" },
                new Genre{ Id=4, Name="Comic" },
                new Genre{ Id=5, Name="Anime" },
                new Genre{ Id=6, Name="Animated" },
                new Genre{ Id=7, Name="Action" },
                new Genre{ Id=8, Name="Adventure" },
                new Genre{ Id=9, Name="Drama" },
                new Genre{ Id=10, Name="Fantasy" },
                new Genre{ Id=11, Name="Horror" },
                new Genre{ Id=12, Name="Music" },
                new Genre{ Id=13, Name="Mystery" },
                new Genre{ Id=14, Name="Sci-Fi" },
                new Genre{ Id=15, Name="???" }
            };


        }


        public List<Movie> GetMovies()
        {
            return database;
        }


        public Movie GetMovie(int id)
        {
            return database
                .Where(m => m.Id == id)
                .FirstOrDefault();
        }


        public void AddMovie(Movie movie)
        {
            if (movie.Id != 0)
            {
                var index = database.FindIndex(m => m.Id == movie.Id);
                database.Remove((from m in database
                                 where m.Id == movie.Id
                                 select m).FirstOrDefault());
                //database.Add(movie);
                database.Insert(index, movie);

            }
            else
            {
                int lastindex = database[database.Count - 1].Id;
                movie.Id = lastindex + 1;
                database.Add(movie);
            }
        }


        public void DeleteMovie(Movie movie)
        {
            if (movie == null) return;
            database.Remove((from m in database
                             where m.Id == movie.Id
                             select m).FirstOrDefault());
        }





        public List<Genre> GetGenres()
        {
            return genresDatabase;
        }


        public Genre GetGenre(int id)
        {
            return genresDatabase
                .Where(g => g.Id == id)
                .FirstOrDefault();
        }




    }
}
