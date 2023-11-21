using dMb.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;



namespace dMb.ViewModels
{
    public class SelectDbViewModel : BaseViewModel
    {

        public ObservableCollection<Models.Genre> Files { get; }


        public Command SelectFileCommand { get; }
        public Command DeleteFileCommand { get; }
        public Command ImportFileCommand { get; }
        public Command GoToCreatePageCommand { get; }


        public SelectDbViewModel()
        {
            Title = "Select Database";
            Files = new ObservableCollection<Models.Genre>();


            SelectFileCommand = new Command<string>(SelectFile);
            DeleteFileCommand = new Command<string>(DeleteFile);
            ImportFileCommand = new Command(ImportFile);

            GoToCreatePageCommand = new Command(
                async () => await Shell.Current.GoToAsync($"//{nameof(Views.SelectDbPage)}/{nameof(Views.CreateDbPage)}"));


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
                var files = await DependencyService.Get<IFileService>().GetFilesNamesAsync();

                foreach (var f in files)
                {
                    Files.Add(new Models.Genre() { Name=f});
                }

                //int filesCount = 15;
                //for(int i=0; i<filesCount; i++)
                //{
                //    // TODO: Change to FileInfo or smt
                //    //Files.Add($"Database{i+1}.db3");
                //    //FileInfo fi = new FileInfo("filename");

                //    Files.Add(new Models.Genre() { Name= $"Database{i + 1}.db3" });
                //}

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("!!! ERROR ERROR ERROR !!!");
                System.Diagnostics.Debug.WriteLine("Failed to load files.");
                System.Diagnostics.Debug.WriteLine(e);
                System.Diagnostics.Debug.WriteLine("!!! ERROR ERROR ERROR !!!");
            }
            finally { IsBusy = false; }
        }

        async void SelectFile(string filename)
        {
            try
            {
                App.DbPath = Path.Combine(App.RootPath, filename);
                Debug.WriteLine("\n===========================");
                Debug.WriteLine($"Root path: {App.RootPath}");
                Debug.WriteLine($"Db path: {App.DbPath}");
                Debug.WriteLine($"Db Name: {App.DbName}");
                Debug.WriteLine("===========================\n");
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("!!! Error !!!");
                System.Diagnostics.Debug.WriteLine($"Faild to select file: {filename}");
                System.Diagnostics.Debug.WriteLine(e);
                System.Diagnostics.Debug.WriteLine("!!! Error !!!");
            }

            // Exit to Browse
            System.Diagnostics.Debug.WriteLine($"Selected: {filename}");
            await Shell.Current.GoToAsync($"//{nameof(Views.MoviesPage)}");
        }

        async void DeleteFile(string filename)
        {
            System.Diagnostics.Debug.WriteLine($"You want to delete this {filename}.");

            bool result = await Shell.Current.DisplayAlert(
                title: "Delete Database",
                message: "Are you sure that you want to delete this database?",
                accept: "Yes, delete",
                cancel: "No");

            // Canceled
            if (!result) return;

            DependencyService.Get<IFileService>().DeleteFile(filename);
        }


        async void ImportFile()
        {
            // Pick file
            var pickedFile = await PickFile();
            if (pickedFile == null) return;

            // Copy file to local storage
            string destination = Path.Combine(App.RootPath, pickedFile.FileName);
            Directory.CreateDirectory(App.RootPath); // Create rootPath directory if not existing

            // Check if file exists
            if (File.Exists(destination))
            {
                bool overwriteRequest = await Shell.Current.DisplayAlert(
                    title: "Do you want to overwrite this file?",
                    message: "This file was alredy imported. Do you want to overwrite it?",
                    accept: "Yes, overwrite",
                    cancel: "No");

                if (!overwriteRequest) return;
            }

            File.Copy(pickedFile.FullPath, destination, true);

            // Reload files list
            LoadFiles();

            // Don't set App.FilePath to imported file
            // let the user choose if wants to import more files or select other one
            //App.FilePath = destination;
        }

        async Task<FileResult> PickFile()
        {
            //var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            //{
            //    {DevicePlatform.Android, new[] {"application.db3"} },
            //    {DevicePlatform.UWP, new[] {".db3"} },
            //});


            try
            {
                //var file = await FilePicker.PickAsync(new PickOptions { FileTypes = customFileType});
                var file = await FilePicker.PickAsync();

                return file;
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Failed to pick a file.");
            }
            return null;
        }

    }
}
