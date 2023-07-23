using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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