using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace dMb
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(Views.MovieDetailPage), typeof(Views.MovieDetailPage));
        }

    }
}
