using Xamarin.Forms;
using Xamarin.Forms.Xaml;




namespace dMb.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MovieDetailPage : ContentPage
    {
        public MovieDetailPage()
        {
            InitializeComponent();

            BindingContext = new ViewModels.MovieDetailViewModel();
        }
    }
}