using Xamarin.Essentials;
using Xamarin.Forms;



namespace dMb.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {

        public Command OpenWebCommand { get; }

        public AboutViewModel()
        {
            Title = "About";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://github.com/jakpop11"));
        }
    }
}
