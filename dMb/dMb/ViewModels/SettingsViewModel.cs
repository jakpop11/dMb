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
        string _FileNameDisplay = App.DbName;


        public string FileNameDisplay { get => _FileNameDisplay; set => SetProperty(ref _FileNameDisplay, value); }


        public Command SelectDBCommand { get; }
        public Command PickFileCommand { get; }
        public Command ResetGenresCommand { get; }


        public SettingsViewModel()
        {
            Title = "Settings";


            SelectDBCommand = new Command(SelectDb);
            PickFileCommand = new Command(async () => await PickFile());
            ResetGenresCommand = new Command(async () => await ResetGenres());

        }


        async void SelectDb()
        {
            await Shell.Current.GoToAsync(nameof(Views.SelectDbPage));
        }


        async Task PickFile()
        {
            var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                {DevicePlatform.Android, new[] { "image/jpeg" }  }, // how to select directory???? //Mime type for Android
                {DevicePlatform.UWP, new[] { ".jpg" } } // extention for Windows
            });

            
            try
            {
                var file = await FilePicker.PickAsync( new PickOptions { FileTypes = customFileType });

                FileNameDisplay = file.FullPath;
            }
            catch(Exception)
            {
                System.Diagnostics.Debug.WriteLine("Failed to pick a file.");
            }
        }


        async Task<int> ResetGenres()
        {
            bool result = await Shell.Current.DisplayAlert(
                "Are you sure?", "Do you want to reset genres and clear all data about them?", 
                accept: "Yes, I'm sure", cancel: "No, keep them");
            if (result)
            {
                return await App.Database.GenerateGenresAsync();
            }
            return 0;
        
        }

    }
}
