using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;
using Xamarin.Essentials;



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
