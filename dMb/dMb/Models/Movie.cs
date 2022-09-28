using System;
using System.Collections.Generic;
using System.Text;


using SQLite;



namespace dMb.Models
{
    public class Movie : ITable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Title { get; set; }

        public string ImgUrl { get; set; }
        // Default img Url
        // "https://shinden.pl/res/other/placeholders/title/225x350.jpg"

        public string Details { get; set; }

        public DateTime EditDate { get; set; }


        // To store genres in run time ??
        [Ignore]
        public List<Genre> Genres { get; set; }
    }
}
