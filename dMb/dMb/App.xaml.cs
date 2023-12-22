using dMb.Services;
using System;
using System.Diagnostics;
using System.IO;
using Xamarin.Forms;

namespace dMb
{
    public partial class App : Application
    {
        #region Old File Paths
        //public static string DbName
        //{
        //    get => Path.GetFileName(LocalPath);
        //}

        //public static string LocalPath
        //{
        //    get => Path.Combine(
        //        Environment.GetFolderPath(
        //            Environment.SpecialFolder.LocalApplicationData), "localDataBase.db3");
        //}

        //static string RootPath
        //{
        //    get => Path.Combine(
        //        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "databases");
        //}
        #endregion

        #region Old Code to Delete
        static MovieDatabase moviedatabase;

        public static MovieDatabase MovieDatabase
        {
            get
            {
                if (moviedatabase == null)
                {
                    moviedatabase = new MovieDatabase("Here should be a path of the database file.");
                }
                return moviedatabase;
            }
        }
        #endregion

        public static string DbName
        {
            get => Path.GetFileName(DbPath);
        }

        static string dbPath;
        public static string DbPath
        {
            get => dbPath;
            set
            {
                if (dbPath == value) return;

                // ToDo: validate path
                // Here can go path validation code if needed

                dbPath = value;

                // Connect to new database
                Database = new SQLDataBase(DbPath);
            }
        }

        public static string RootPath
        {
            get => Path.Combine(DependencyService.Get<IFileService>().GetRootPath(), "databases");
        }

        static SQLDataBase database;

        public static SQLDataBase Database
        {
            get
            {
                if (database == null)
                {
                    database = new SQLDataBase(DbPath);
                }
                return database;
            }
            set => database = value;
        }


        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();

            Directory.CreateDirectory(RootPath);
            Debug.WriteLine("\n===========================");
            Debug.WriteLine($"Root path: {RootPath}");
            Debug.WriteLine($"Db path: {DbPath}");
            Debug.WriteLine($"Db Name: {DbName}");
            Debug.WriteLine("===========================\n");
        }

        protected override void OnStart()
        {
            // Start App with SelectDbPage
            Shell.Current.GoToAsync($"//{nameof(Views.SelectDbPage)}");
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }



    }
}
