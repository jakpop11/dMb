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
    public class SettingsViewModel : BaseViewModel
    {
        string _FilePathDisplay = App.LocalPath;


        public string FilePathDisplay { get => _FilePathDisplay; set => SetProperty(ref _FilePathDisplay, value); }


        public Command PickFileCommand { get; }
        public Command ResetGenresCommand { get; }


        public SettingsViewModel()
        {
            Title = "Settings";


            PickFileCommand = new Command(async () => await PickFile());
            ResetGenresCommand = new Command(async () => await ResetGenres());



            MovieGenres = new ObservableCollection<MovieGenres>();
        }


        async Task PickFile()
        {
            var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                {DevicePlatform.Android, new[] { "vnd.android.document/directory" }  }, // how to select directory???? //Mime type for Android
                {DevicePlatform.UWP, new[] { ".jpg" } } //? // extention for Windows
            });

            
            try
            {
                var file = await FilePicker.PickAsync( new PickOptions { FileTypes = customFileType });

                FilePathDisplay = file.FullPath;
            }
            catch(Exception)
            {
                System.Diagnostics.Debug.WriteLine("Failed to pick a file.");
            }
        }


        async Task<int> ResetGenres()
        {
            bool result = await Shell.Current.DisplayAlert("Are you sure?", "Do you want to reset genres and clear all data about them?", accept: "Yes, I'm sure", cancel: "No, keep them");
            if (result)
            {
                return await App.Database.GenerateGenresAsync();
            }
            return 0;
        
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
