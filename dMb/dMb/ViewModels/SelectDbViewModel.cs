using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
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


            //SelectFileCommand = new Command(OnAppearing);
            //DeleteFileCommand = new Command(OnAppearing);
            ImportFileCommand = new Command(OnAppearing);
            // TODO: add Page path
            GoToCreatePageCommand = new Command(
                async () => await Shell.Current.GoToAsync(nameof(Views.CreateDbPage)));


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
                //foreach (var f in files)
                //{
                //    Files.Add(f);
                //}

                int filesCount = 15;
                for(int i=0; i<filesCount; i++)
                {
                    // TODO: Change to FileInfo or smt
                    //Files.Add($"Database{i+1}.db3");
                    //FileInfo fi = new FileInfo("filename");

                    Files.Add(new Models.Genre() { Name= $"Database{i + 1}.db3" });
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
