using Xamarin.Forms;

namespace dMb.Views
{

    public partial class SettingsPage : ContentPage
    {
        ViewModels.SettingsViewModel _viewModel;


        public SettingsPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ViewModels.SettingsViewModel();
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}