using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using dMb.Droid.Services;
using dMb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;


[assembly: Xamarin.Forms.Dependency(typeof(FileService))]
namespace dMb.Droid.Services
{
    public class FileService : IFileService
    {
        static Java.IO.File rootDirectory = Android.App.Application.Context.GetExternalFilesDir(null);
        static Java.IO.File databasesDirectory = new Java.IO.File(App.RootPath);


        /// <summary>
        /// Returns ExternalFilesDir Path
        /// </summary>
        /// <returns></returns>
        public string GetRootPath()
        {
            var path = rootDirectory;
            Console.WriteLine("-----------------------");
            Console.WriteLine($"ExternalFilesDir: {path}");
            Console.WriteLine("-----------------------");

            return path.ToString();
        }

        /// <summary>
        /// Deletes file by its name from RootDirectory
        /// </summary>
        /// <param name="fileName"></param>
        public void DeleteFile(string fileName)
        {
            var destination = Path.Combine(GetRootPath(), fileName);
            File.Delete(destination);
        }

        /// <summary>
        /// Returns array containing files names from RootDirectory
        /// </summary>
        /// <returns></returns>
        public async Task<string[]> GetFilesNamesAsync()
        {
            // TODO:
            // Change it to be array of Files
            // with Name, Path, etc.

            var files = databasesDirectory.ListFiles();
            List<string> filesNames = new List<string>();

            Console.WriteLine("=======================");
            foreach (var file in files)
            {
                Console.WriteLine(file.Name);
                filesNames.Add(file.Name);
            }
            Console.WriteLine("=======================");


            return filesNames.ToArray();
        }

        /// <summary>
        /// Probably not neede, to delete
        /// ActionSheet to pick filename from Rootdirectory
        /// </summary>
        /// <returns></returns>
        public async Task<string> PickFileAsync()
        {
            // Probably not neede, to delete

            // List
            var files = await GetFilesNamesAsync();

            string fileName = await Shell.Current.CurrentPage.DisplayActionSheet("Choose file", "cancel", null, files);

            if (fileName == "cancel" || fileName == null)
            {
                return null;
            }

            // TODO: try to read and save in local path

            return Path.Combine(GetRootPath(), fileName);
        }
    }
}