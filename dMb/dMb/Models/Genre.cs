using System;
using System.Collections.Generic;
using System.Text;

using SQLite;



namespace dMb.Models
{
    public class Genre : ITable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }



    }


}
