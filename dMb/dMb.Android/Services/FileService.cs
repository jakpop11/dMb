using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

using dMb.Services;
using dMb.Droid.Services;
using Xamarin.Forms;
using System.Threading.Tasks;


[assembly: Xamarin.Forms.Dependency(typeof(FileService))]
namespace dMb.Droid.Services
{
    public class FileService : IFileService
    {
        Java.IO.File rootDir { get => Android.App.Application.Context.GetExternalFilesDir(null); }
        Java.IO.File dbDir { get => new Java.IO.File(GetDatabasesPath()); }


        public string GetRoothPath()
        {
            return rootDir.ToString();
        }

        public string GetDatabasesPath()
        {
            string dirName = "databases";
            string dbPath = Path.Combine(GetRoothPath(), dirName);

            if (!Directory.Exists(dbPath)) { Directory.CreateDirectory(dbPath); }
            
            return dbPath;
        }

        public async Task<string> CreateFileAsync()
        {
            Exception exception = new NotImplementedException();
            return null;
        }

        public void DeleteFile(string filePath)
        {
            Exception exception = new NotImplementedException();
            return;
        }

        public async Task<string[]> GetFilesNamesAsync()
        {
            var files = dbDir.ListFiles();

            List<string> names = new List<string>();
            foreach (var file in files)
            {
                names.Add(file.Name);
            }

            return names.ToArray();
        }

        public async Task<string> PickFileAsync()
        {
            Exception exception = new NotImplementedException();
            return null;
        }
    }
}