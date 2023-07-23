using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;
using Xamarin.Essentials;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using dMb.Models;



namespace dMb.ViewModels
{
    public class SelectDbViewModel : BaseViewModel
    {

        public Command GoBackCommand { get; }

        public SelectDbViewModel()
        {
            Title = "Select Database";

            GoBackCommand = new Command(async () => await Shell.Current.GoToAsync(".."));

            MovieGenres = new ObservableCollection<MovieGenres>();
        }



        public void OnAppearing()
        {
            LoadMovieGenres();
        }



        public ObservableCollection<MovieGenres> MovieGenres { get; }

        async Task LoadMovieGenres()
        {
            try
            {
                MovieGenres.Clear();
                var movieGenres = await App.Database.GetMovieGenresAsync();

                foreach (var mg in movieGenres)
                {
                    MovieGenres.Add(mg);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Failed to load MovieGenres.");
            }
        }



    }
}
