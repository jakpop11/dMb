using SQLite;



namespace dMb.Models
{
    public class MovieGenres
    {
        [NotNull]
        public int MovieId { get; set; }

        [NotNull]
        public int GenreId { get; set; }

    }
}
