﻿using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace dMb.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MoviesPage : ContentPage
    {
        ViewModels.MoviesViewModel _viewModel;


        public MoviesPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ViewModels.MoviesViewModel();
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.OnAppearing();
        }

    }
}