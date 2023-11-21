using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;



namespace dMb.ViewModels
{
    public class SelectDbViewModel : BaseViewModel
    {
        public Command RefreshCommand { get; }
        public Command SelectFileCommand { get; }
        public Command DeleteFileCommand { get; }
        public Command ImportFileCommand { get; }
        public Command GoToCreatePageCommand { get; }


        public ObservableCollection<string> Files { get; set; }


        public SelectDbViewModel()
        {
            Title = "Select Database";

            RefreshCommand = new Command(async () => await LoadFiles());
            SelectFileCommand = new Command(OnAppearing);
            DeleteFileCommand = new Command(OnAppearing);
            ImportFileCommand = new Command(OnAppearing);
            // TODO: add Page path
            GoToCreatePageCommand = new Command(async () => await Shell.Current.GoToAsync(""));


            Files = new ObservableCollection<string>();
        }

        public void OnAppearing()
        {
            LoadFiles();
        }


        async Task LoadFiles()
        {
            IsBusy = true;
            try
            {
                Files.Clear();
                var files = new List<string>()
                {
                    "Database1.db3",
                    "Database2.db3",
                    "Database3.db3",
                    "Database4.db3",
                    "Database5.db3",
                    "Database6.db3",
                    "Database7.db3",
                    "Database8.db3",
                    "Database9.db3",
                    "Database10.db3",
                };

                foreach (var f in files)
                {
                    Files.Add(f);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Failed to load files.");
            }
            finally { IsBusy = false; }
        }



    }
}
