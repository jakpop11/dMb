using dMb.Services;



namespace dMb.ViewModels
{
    public class BaseViewModel : PropertyChangedHelper
    {

        bool isBusy = false;
        string title = string.Empty;


        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value);
        }

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

    }
}
