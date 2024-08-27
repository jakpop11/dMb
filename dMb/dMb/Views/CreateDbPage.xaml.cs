using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace dMb.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateDbPage : ContentPage
    {
        ViewModels.CreateDbViewModel _viewModel;

        public CreateDbPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ViewModels.CreateDbViewModel();
        }
    }
}