using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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