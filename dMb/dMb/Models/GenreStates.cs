namespace dMb.Models
{

    public class GenreState : Services.PropertyChangedHelper
    {
        private Genre _genre;
        public Genre Genre
        {
            get => _genre;
            set => SetProperty(ref _genre, value);
        }

        private Controls.StateCheckBox.CheckBoxState _state;
        public Controls.StateCheckBox.CheckBoxState State
        {
            get => _state;
            set => SetProperty(ref _state, value);
        }

        public GenreState(Genre genre, Controls.StateCheckBox.CheckBoxState state)
        {
            Genre = genre;
            State = state;
        }

    }


    public class GenreBool : Services.PropertyChangedHelper
    {
        private Genre _genre;
        public Genre Genre
        {
            get => _genre;
            set => SetProperty(ref _genre, value);
        }

        private bool _bool;
        public bool Bool
        {
            get => _bool;
            set => SetProperty(ref _bool, value);
        }

        public GenreBool(Genre genre, bool boolen)
        {
            Genre = genre;
            Bool = boolen;
        }

    }

}