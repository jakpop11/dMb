using System.ComponentModel;



namespace dMb.Models
{
    public enum State
    {
        UNCHECKED,
        CHECKED,
        CROSSED
    }



    //public class GenreState : Services.PropertyChangedHelper
    //{
    //    State _state;


    //    public Genre Genre { get; set; }

    //    public State State
    //    {
    //        get => _state;
    //        set => SetProperty(ref _state, value);
    //    }
    //}


    //public class GenreBool : Services.PropertyChangedHelper
    //{
    //    bool _bool = false;

    //    public Genre Genre { get; set; }

    //    public bool Bool
    //    {
    //        get => _bool;
    //        set => SetProperty(ref _bool, value);
    //    }
    //}



    public class GenreState
    {
        public Genre Genre { get; set; }

        public State State { get; set; }

    }


    public class GenreBool
    {
        public Genre Genre { get; set; }

        public bool Bool { get; set; }

    }




}