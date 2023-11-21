using Xamarin.Forms;
using Xamarin.Forms.Xaml;



namespace dMb.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();

            BindingContext = new ViewModels.AboutViewModel();
        }
    }
}