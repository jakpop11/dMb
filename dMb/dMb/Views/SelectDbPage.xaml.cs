using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace dMb.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectDbPage : ContentPage
    {

        ViewModels.SelectDbViewModel _viewModel;


        public SelectDbPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ViewModels.SelectDbViewModel();
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.OnAppearing();
        }
    }
}