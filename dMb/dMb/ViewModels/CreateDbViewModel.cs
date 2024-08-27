using dMb.Models;
using dMb.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace dMb.ViewModels
{
    public class CreateDbViewModel : BaseViewModel
    {
        string _dbName;
        string _genreName;

        public string DbName
        {
            get => _dbName;
            set => SetProperty(ref _dbName, value);
        }

        public string GenreName
        {
            get => _genreName;
            set => SetProperty(ref _genreName, value);
        }

        public ObservableCollection<Genre> Genres { get; }


        public Command AddGenreCommand { get; }
        public Command DeleteGenreCommand { get; }
        public Command CreateDbCommand { get; }


        public CreateDbViewModel()
        {
            Title = "Create Database";
            Genres = new ObservableCollection<Genre>();

            AddGenreCommand = new Command(AddGenre);
            DeleteGenreCommand = new Command<string>(DeleteGenre);
            CreateDbCommand = new Command(CreateDb);
        }

        void AddGenre()
        {
            if (string.IsNullOrEmpty(GenreName)) { return; }
            Genres.Add(new Genre { Name = GenreName });
            // Clear Entry
            GenreName = "";

        }

        void DeleteGenre(string name)
        {
            if (name==null) { return; }
            var genre = Genres.First(g => g.Name == name);
            if (genre != null) { Genres.Remove(genre); }

        }

        void CreateDb()
        {
            string dbPath = System.IO.Path.Combine(App.RootPath, $"{DbName}.db3");

            // Creates new Database
            new SQLDataBase(dbPath, Genres.ToList());

            Debug.WriteLine("\n===========================");
            Debug.WriteLine($"Root path: {App.RootPath}");
            Debug.WriteLine($"Db path: {App.DbPath}");
            Debug.WriteLine($"Db Name: {App.DbName}");
            Debug.WriteLine("===========================\n");

            Shell.Current.GoToAsync("..");
        }
    }
}
